<div align="center">

# One order, four projection styles

### Compare keyed lines, product joins, event counters, and a state-derived count.

**Chronicle · Fundamentals · In-process specifications · Workbench PDL preview**

[Back to all samples](../../README.md)

</div>

---

## The scenario

An order has a customer, shipment status, last-update time, and one active line per product. Adding the same product again **replaces its quantity**, rather than adding a second line or accumulating quantities. An adjustment replaces an existing line's quantity; removal deletes that line; cancellation deletes the whole order.

`ItemCount` means **distinct active lines**. That requirement deliberately exposes a difference: the reducer derives it correctly from its next state, while the projections' event counters are only an approximation. The projections join product names; the stream-local reducer cannot. These are trade-offs, not four equivalent implementations.

| Stream | Events |
| --- | --- |
| Order (`OrderId`) | `OrderPlaced`, `ItemAddedToOrder`, `ItemQuantityAdjusted`, `ItemRemovedFromOrder`, `OrderShipped`, `OrderCancelled` |
| Product (`ProductId`) | `ProductRegistered`, `ProductRenamed` |

Both identities derive from `EventSourceId<Guid>`. Names are `ConceptAs<string>` values; `CustomerName` is classified `[PII]`. `LastUpdated` comes from `EventContext.Occurred`, not from the projection's processing clock. `FromEvery` includes events handled only by children, such as `ItemQuantityAdjusted`.

## The four files side by side

| Model-bound attributes | Fluent `IProjectionFor<T>` | Self-reducing read model | PDL |
| --- | --- | --- | --- |
| [`ModelBoundOrder.cs`](./ModelBoundOrder.cs) | [`FluentOrder.cs`](./FluentOrder.cs) | [`ReducedOrder.cs`](./ReducedOrder.cs) | [`Order.pdl`](./Order.pdl) |
| Mapping beside each property | Separate definition; clean read model | Static `Reduce` methods beside the state | Declaration for Workbench preview |

Each uses a distinct read-model type so the definitions coexist. PDL targets the separately registered [`PdlOrder`](./PdlOrder.cs) schema; the host does **not** register a fourth code-owned projection.

| Capability | Model-bound | Fluent | Reducer | PDL preview |
| --- | --- | --- | --- | --- |
| Customer and shipment status | Attributes | AutoMap and constant setter | Handlers | `from` mappings |
| Product-keyed lines and quantity replacement | `ChildrenFrom`, AutoMap | `Children`, `IdentifiedBy`, `UsingKey` | Upsert by product id | `children … identified by` |
| Product name from another stream | `Join` attributes on the child | Child `Join` uses `IdentifiedBy` implicitly; do not call `On` | Not available; names stay null | Child `join` declarations |
| Root timestamp from child-only events | `FromEvery` | `FromEvery` | Explicit `context.Occurred` in each handler | `every` |
| Distinct active line count after a duplicate add | **No:** counts add/remove events | **No:** counts add/remove events | **Yes:** count next-state lines | **No:** same event-counter approximation |
| Child removal and order cancellation | `RemovedWith` | `RemovedWith` | Filter lines; return null to delete | `remove with` |
| Best fit | Mapping stays readable beside the model | Mapping needs its own structure | Coordinated prior-state calculations | Explore declarations against recorded history |

## Counters count events, not children

The model-bound and fluent versions increment `ItemCount` on `ItemAddedToOrder` and decrement it on `ItemRemovedFromOrder`. This is the closest released counter primitive, **not an implementation of the distinct-line requirement**:

| History for one product | Active lines | Projection counter | Reducer count |
| --- | ---: | ---: | ---: |
| Add quantity 2 | 1 | 1 | 1 |
| Add quantity 5 for the same product | 1 | 2 | 1 |
| Remove that product | 0 | 1 | 0 |

The specs assert these differences explicitly: `should_count_the_add_event_twice` for the projections and `should_have_one_line` for the reducer. They do not pretend the projection counter is correct.

The released `19.59.1` API defines [`CountAttribute<T>`](https://github.com/Cratis/Chronicle/blob/v19.59.1/Source/Clients/DotNET/Projections/ModelBound/CountAttribute.cs) as counting event occurrences. The fluent [`CountBuilder`](https://github.com/Cratis/Chronicle/blob/v19.59.1/Source/Clients/DotNET/Projections/CountBuilder.cs) emits the same count expression. [Projection functions](https://www.cratis.io/chronicle/projections/declarative/functions/) describes increment/decrement as unconditional changes of one. None inspects whether a child key already exists.

### When a reducer is the better choice

Choose a reducer when the result depends on the complete next state, not just the triggering event. Here it upserts a line, snapshots the resulting collection, and derives `ItemCount` from its length. A repeated product remains one line; removal leaves zero. The reducer never mutates the prior collection.

The pure fold lives on `ReducedOrder` as public static `Reduce(event, current?[, EventContext])` overloads. No reducer instance or manual registration is needed. Returning null for cancellation still deletes the model; updates to absent orders still return null.

### Separate-reducer alternative

Use a separate `IReducerFor<T>` class when a fold needs injected services, replay-instance behavior, or a model contract kept separate from its transitions. The placement handler would then move off the record into this class (excerpt; the other handlers move in the same way):

```csharp
public class OrderReducer : IReducerFor<ReducedOrder>
{
    public ReducedOrder Placed(OrderPlaced @event, ReducedOrder? current, EventContext context) =>
        new(new OrderId(Guid.Parse(context.EventSourceId)), @event.CustomerName, false, 0, context.Occurred, []);
}
```

This is an alternative, not an additional builder: remove the model's static `Reduce` methods before registering a separate reducer for it. The running sample uses only the self-reducing form because the fold has no dependencies or instance behavior.

Both forms process an order's stream: neither can join `ProductRegistered` or `ProductRenamed` on the product's stream. The sample leaves `ProductName` null rather than querying another read model or hiding a cross-stream lookup in a handler. If you need both exact distinct-line counts and product names, reconsider the event/read-model design; none of these four definitions supplies both here.

## Run the specifications — no Docker

Install the [.NET 10 SDK](https://dotnet.microsoft.com/download), then run from the repository root:

```bash
dotnet run --project Chronicle/ProjectionStyles/ProjectionStyles.Specs/ProjectionStyles.Specs.csproj --configuration Debug --
```

[`ProjectionStyles.Specs`](./ProjectionStyles.Specs/) uses `ReadModelScenario<T>` to discover each C# style and drive the same histories in memory: product joins before children, a child-only last event, duplicate adds, quantity adjustment, removal, and cancellation. Additional specs cover placement, shipment, and a rename after the line exists. A direct reducer spec checks an explicit occurrence time and preservation of prior state.

These are projection/dispatch specifications, **not live-kernel integration tests**. The harness substitutes storage, sinks, observer lifecycle, join-key resolution, and event context. Synthetic projection contexts advance by a tick per event; the timestamp specs use that to distinguish the last child event from the previous event without sleeping. Compliance encryption/release is not run in-process, so the `[PII]` classification is not a claim of compliance coverage.

PDL is **not covered by these specs**. `ReadModelScenario` accepts projection builders or discovers C# artifacts; it has no PDL declaration entry point. The declaration uses the released Chronicle language, but previewing it requires the server-side compiler and Workbench. See the [PDL reference](https://www.cratis.io/chronicle/projections/projection-declaration-language/).

## Optional: populate a local kernel and preview PDL

Docker and a browser are needed only for this path. The compose file pins the development image to `19.59.1`, matching the repository's Chronicle packages, and publishes only loopback port 35000. Stop another local kernel using that port first. This is disposable sample data and development authentication, not a production deployment.

From the repository root:

```bash
docker compose -f Chronicle/ProjectionStyles/docker-compose.yml up -d
dotnet run --project Chronicle/ProjectionStyles/ProjectionStyles.csproj --urls http://localhost:5088
```

In another terminal:

```bash
curl --request POST http://localhost:5088/orders/run
```

The endpoint registers the PDL schema, records a product registration and rename before the order, then records placement, two adds of the same product, an adjustment, and shipment. It awaits observer completion before reading the three C# results. Look for one line of quantity 3 in each: projection `itemCount` is 2, reducer `itemCount` is 1, projection name is `Camp mug`, reducer name is null. Each request uses fresh ids and appends another history.

1. Open the local Workbench at `https://localhost:35000/` and use its development login. The local development certificate may need to be trusted.
2. Select event store **ProjectionStylesSample**, namespace **Default**, and **Projections**.
3. Choose **New**, paste [`Order.pdl`](./Order.pdl) into the declaration editor, and choose **Preview**.
4. Inspect the returned order for the id from the HTTP response. Expect the same keyed line and event-counter approximation as the fluent definition. Do not choose Save: this is a read-only preview against a code-registered schema.

The header's `ProjectionStyles.PdlOrder` is the registered identifier, not its display name. Property paths are PascalCase to match the .NET registered schemas; `$eventContext.occurred` uses the PDL context spelling. The released [Workbench implementation](https://github.com/Cratis/Chronicle/blob/v19.59.1/Source/Workbench/Features/EventStore/General/Projections/Projections.tsx) exposes New and Preview separately from Save.

Stop the host with Ctrl+C. To remove this sample's development container and its unmounted event history:

```bash
docker compose -f Chronicle/ProjectionStyles/docker-compose.yml down
```

The sample intentionally does not add commands, validation, authorization, a frontend, or lifecycle guards for arbitrary invalid histories. Its reducer ignores updates to absent orders; declarative updates can create state, so late updates after cancellation are not claimed equivalent. The optional HTTP/Workbench path requires a live kernel and is outside the in-process specification gate.

## Explore next

- [Choose a read-model style](https://www.cratis.io/chronicle/projections/choosing-a-read-model-style/)
- [Model-bound children](https://www.cratis.io/chronicle/projections/model-bound/children/) and [joins](https://www.cratis.io/chronicle/projections/model-bound/joins/)
- [Fluent children](https://www.cratis.io/chronicle/projections/declarative/children/) and [joins](https://www.cratis.io/chronicle/projections/declarative/joins/)
- [Reducers](https://www.cratis.io/chronicle/reducers/) and [read-model scenarios](https://www.cratis.io/chronicle/testing/read-models/scenario/)
- [Back to all samples](../../README.md)

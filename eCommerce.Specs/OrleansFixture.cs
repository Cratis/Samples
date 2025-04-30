// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle;
using Cratis.Chronicle.Concepts.EventSequences;
using Cratis.Chronicle.Concepts.Observation;
using Cratis.Chronicle.Diagnostics.OpenTelemetry;
using Cratis.Chronicle.Grains;
using Cratis.Chronicle.Grains.EventSequences;
using Cratis.Chronicle.Grains.Observation;
using Cratis.Chronicle.Projections;
using Cratis.Chronicle.Reactors;
using Cratis.Chronicle.Reducers;
using Cratis.Chronicle.Setup;
using Cratis.Chronicle.Storage;
using Cratis.Chronicle.Storage.EventSequences;
using Cratis.DependencyInjection;
using Cratis.Json;
using DotNet.Testcontainers.Networks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Orleans.Storage;
using EventSequenceId = Cratis.Chronicle.Concepts.EventSequences.EventSequenceId;
using IEventSequence = Cratis.Chronicle.Grains.EventSequences.IEventSequence;

namespace eCommerce.Specs;

public class OrleansFixture(GlobalFixture globalFixture) : WebApplicationFactory<Startup>
{
#if DEBUG
    bool _backupPerformed;
#endif
    string _name = string.Empty;

    protected override IHostBuilder CreateHostBuilder()
    {
        var builder = Host.CreateDefaultBuilder();

        var chronicleOptions = new Cratis.Chronicle.Concepts.Configuration.ChronicleOptions();

        builder.UseCratisMongoDB(
            mongo =>
            {
                mongo.Server = "mongodb://localhost:27018";
                mongo.Database = "orleans";
            });
        builder.UseCratisApplicationModel();
        builder.ConfigureLogging(_ => _.ClearProviders());
        builder
            .UseDefaultServiceProvider(_ => _.ValidateOnBuild = false)
            .ConfigureServices((ctx, services) =>
            {
                services.AddRules()
                    .AddUnitOfWork()
                    .AddAggregates()
                    .AddCompliance()
                    .AddCausation()
                    .AddCratisChronicleClient()
                    .AddHostedService<ChronicleClientStartupTask>();
                services.AddCratisApplicationModelMeter();
                services.AddSingleton(Globals.JsonSerializerOptions);
                services.AddBindingsByConvention();
                services.AddSelfBindings();
                services.AddChronicleTelemetry(ctx.Configuration);
                services.AddControllers();
                services.AddMvc();
                ctx.Configuration.Bind(chronicleOptions);

                ConfigureServices(services);
            });
        builder.AddCratisChronicle();

        builder.UseOrleans(silo =>
            {
                silo
                    .UseLocalhostClustering()
                    .AddCratisChronicle(
                        options => options.EventStoreName = Constants.EventStore,
                        chronicleBuilder => chronicleBuilder.WithMongoDB(chronicleOptions.Storage.ConnectionDetails, Constants.EventStore));
            })
            .UseConsoleLifetime();

        // For some weird reason we need this https://stackoverflow.com/questions/69974249/no-app-configured-error-while-using-webapplicationfactory-for-running-integrat
        builder.ConfigureWebHostDefaults(b => b.Configure(app => { }));

        return builder;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSolutionRelativeContentRoot("eCommerce.Specs");
        builder.Configure(app =>
        {
            app.UseCratisChronicle();
            app.UseCratisApplicationModel();
            app.UseRouting();
            app.UseEndpoints(_ => _.MapControllers());
        });
    }

    public INetwork Network => GlobalFixture.Network;
    public MongoDBDatabase EventStoreDatabase => GlobalFixture.EventStore;
    public MongoDBDatabase EventStoreForNamespaceDatabase => GlobalFixture.EventStoreForNamespace;
    public MongoDBDatabase ReadModelsDatabase => GlobalFixture.ReadModels;

    public IEventStore EventStore => Services.GetRequiredService<IEventStore>();
    public IChronicleClient ChronicleClient => Services.GetRequiredService<IChronicleClient>();
    public IEventStoreStorage EventStoreStorage => Services.GetRequiredService<IStorage>().GetEventStore(Constants.EventStore);
    public IEventStoreNamespaceStorage GetEventStoreNamespaceStorage(Cratis.Chronicle.Concepts.EventStoreNamespaceName? namespaceName = null) => EventStoreStorage.GetNamespace(namespaceName ?? Cratis.Chronicle.Concepts.EventStoreNamespaceName.Default);
    public IEventSequenceStorage GetEventLogStorage(Cratis.Chronicle.Concepts.EventStoreNamespaceName? namespaceName = null) => GetEventStoreNamespaceStorage(namespaceName).GetEventSequence(EventSequenceId.Log);

    public IGrainFactory GrainFactory => Services.GetRequiredService<IGrainFactory>();

    public TStorage GetGrainStorage<TStorage>(string key)
        where TStorage : IGrainStorage => (TStorage)Services.GetRequiredKeyedService<IGrainStorage>(key);
    public EventSequencesStorageProvider GetEventSequenceStatesStorage() => GetGrainStorage<EventSequencesStorageProvider>(WellKnownGrainStorageProviders.EventSequences);
    public IEventSequence EventLogSequenceGrain => GetEventSequenceGrain(EventSequenceId.Log);
    public IEventSequence GetEventSequenceGrain(EventSequenceId id) => Services.GetRequiredService<IGrainFactory>().GetGrain<IEventSequence>(CreateEventSequenceKey(id));
    public EventSequenceKey CreateEventSequenceKey(EventSequenceId id) => new(id, Constants.EventStore, Cratis.Chronicle.Concepts.EventStoreNamespaceName.Default);

    public IObserver GetObserverForReactor<T>() => GrainFactory
        .GetGrain<IObserver>(
            new ObserverKey(
                typeof(T).GetReactorId().Value,
                Constants.EventStore,
                Cratis.Chronicle.Concepts.EventStoreNamespaceName.Default,
                EventSequenceId.Log));

    public IObserver GetObserverForReducer<T>() => GrainFactory
        .GetGrain<IObserver>(
            new ObserverKey(
                typeof(T).GetReducerId().Value,
                Constants.EventStore,
                Cratis.Chronicle.Concepts.EventStoreNamespaceName.Default,
                EventSequenceId.Log));

    public IObserver GetObserverForProjection<TProjection>() => GrainFactory
        .GetGrain<IObserver>(
            new ObserverKey(typeof(TProjection).GetProjectionId().Value,
                Constants.EventStore,
                Cratis.Chronicle.Concepts.EventStoreNamespaceName.Default,
                EventSequenceId.Log));

    public GlobalFixture GlobalFixture { get; } = globalFixture;

    public void SetName(string name) => _name = name;

    protected void EnsureBuilt()
    {
        Services.GetRequiredService<IEventStore>();
    }

    protected override void Dispose(bool disposing)
    {
#if DEBUG
        if (!_backupPerformed)
        {
            GlobalFixture.PerformBackup(_name);
            _backupPerformed = true;
        }
#endif
        GlobalFixture.RemoveAllDatabases().GetAwaiter().GetResult();
        base.Dispose(false);
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
    }

    public virtual IEnumerable<Type> EventTypes { get; } = [];
    public virtual IEnumerable<Type> Projections { get; } = [];
    public virtual IEnumerable<Type> Adapters { get; } = [];
    public virtual IEnumerable<Type> Reactors { get; } = [];
    public virtual IEnumerable<Type> Reducers { get; } = [];
    public virtual IEnumerable<Type> ReactorMiddlewares { get; } = [];
    public virtual IEnumerable<Type> ComplianceForTypesProviders { get; } = [];
    public virtual IEnumerable<Type> ComplianceForPropertiesProviders { get; } = [];
    public virtual IEnumerable<Type> Rules { get; } = [];
    public virtual IEnumerable<Type> AdditionalEventInformationProviders { get; } = [];
    public virtual IEnumerable<Type> AggregateRoots { get; } = [];
    public virtual IEnumerable<Type> AggregateRootStateTypes { get; } = [];
    public virtual IEnumerable<Type> ConstraintTypes { get; } = [];
    public virtual IEnumerable<Type> UniqueConstraints { get; } = [];
    public virtual IEnumerable<Type> UniqueEventTypeConstraints { get; } = [];
    public void Initialize()
    {
    }
}

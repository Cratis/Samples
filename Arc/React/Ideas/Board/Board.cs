// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Arc.React.Ideas.Board;

using System.Reactive.Subjects;
using System.Threading;
using Cratis.Arc.Authorization;
using Cratis.Arc.Commands.ModelBound;
using Cratis.Arc.Queries.ModelBound;

/// <summary>
/// Captures a new idea in the current-state board.
/// </summary>
/// <param name="Id">The identifier assigned by the client.</param>
/// <param name="Title">The concise idea title.</param>
/// <param name="Summary">The detail that makes the idea useful.</param>
[Command, AllowAnonymous]
public record CaptureIdea(IdeaId Id, IdeaTitle Title, IdeaSummary Summary)
{
    /// <summary>
    /// Stores the idea directly without appending an event.
    /// </summary>
    /// <param name="store">The current-state idea store.</param>
    public void Handle(IdeaStore store) => store.Capture(new(Id, Title, Summary));
}

/// <summary>
/// Represents an idea shown on the board.
/// </summary>
/// <param name="Id">The idea identifier.</param>
/// <param name="Title">The concise idea title.</param>
/// <param name="Summary">The useful idea detail.</param>
[ReadModel, AllowAnonymous]
public record Idea(IdeaId Id, IdeaTitle Title, IdeaSummary Summary)
{
    /// <summary>
    /// Observes all captured ideas and pushes the current board on each change.
    /// </summary>
    /// <param name="store">The current-state idea store.</param>
    /// <returns>A live sequence containing the current ideas.</returns>
    public static ISubject<IEnumerable<Idea>> ObserveIdeas(IdeaStore store) => store.Observe();
}

/// <summary>
/// Holds the current idea board in memory for this focused sample.
/// </summary>
public sealed class IdeaStore : IDisposable
{
    readonly Lock _gate = new();
    readonly BehaviorSubject<IEnumerable<Idea>> _ideas = new(Array.Empty<Idea>());

    /// <summary>
    /// Gets a snapshot of the current ideas.
    /// </summary>
    public IReadOnlyList<Idea> Current
    {
        get
        {
            lock (_gate)
            {
                return _ideas.Value.ToArray();
            }
        }
    }

    /// <summary>
    /// Captures an idea and publishes a fresh board snapshot.
    /// </summary>
    /// <param name="idea">The idea to capture.</param>
    public void Capture(Idea idea)
    {
        lock (_gate)
        {
            _ideas.OnNext(new[] { idea }.Concat(_ideas.Value).ToArray());
        }
    }

    /// <summary>
    /// Observes the current board and every subsequent change.
    /// </summary>
    /// <returns>The live idea sequence.</returns>
    public ISubject<IEnumerable<Idea>> Observe() => _ideas;

    /// <inheritdoc/>
    public void Dispose() => _ideas.Dispose();
}

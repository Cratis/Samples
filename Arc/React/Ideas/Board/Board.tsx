// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useDialog } from '@cratis/arc.react/dialogs';
import { withViewModel } from '@cratis/arc.react.mvvm';
import { Toolbar, ToolbarButton } from '@cratis/components/Toolbar';
import { BoardViewModel } from './BoardViewModel';
import { CaptureIdeaDialog } from './CaptureIdeaDialog';
import { Idea } from './Idea';
import { ObserveIdeas } from './ObserveIdeas';
import './Board.css';

interface IdeaCardProps {
    idea: Idea;
    sequence: number;
}

const IdeaCard = ({ idea, sequence }: IdeaCardProps) => (
    <article className="idea-card">
        <div className="idea-card__number">{String(sequence).padStart(2, '0')}</div>
        <div>
            <h3>{idea.title}</h3>
            <p>{idea.summary}</p>
        </div>
    </article>
);

export const Board = withViewModel(BoardViewModel, ({ viewModel }) => {
    const [ideasResult] = ObserveIdeas.use();
    const [CaptureDialog, showCaptureDialog] = useDialog(CaptureIdeaDialog);
    const ideas = viewModel.filter(ideasResult.data ?? []);
    const capturedCount = ideasResult.data?.length ?? 0;

    return (
        <main className="idea-loom">
            <header className="app-header">
                <a className="brand" href="/" aria-label="Idea Loom home">
                    <span className="brand__mark">IL</span>
                    <span>Idea Loom</span>
                </a>
                <span className="architecture-pill"><i className="pi pi-bolt" /> Arc CQRS · no Chronicle</span>
            </header>

            <section className="hero">
                <div className="hero__copy">
                    <span className="eyebrow">A focused Arc + React sample</span>
                    <h1>Shape the next<br /><em>small win.</em></h1>
                    <p>
                        Capture a useful idea, watch the live query update, and trace one strongly typed contract from C# to React.
                    </p>
                </div>
                <div className="hero__metrics" aria-label="Sample characteristics">
                    <div><strong>{capturedCount}</strong><span>ideas captured</span></div>
                    <div><strong>Live</strong><span>observable query</span></div>
                    <div><strong>Direct</strong><span>current-state write</span></div>
                </div>
            </section>

            <section className="board-surface" aria-labelledby="board-title">
                <div className="board-surface__heading">
                    <div>
                        <span className="eyebrow">Working set</span>
                        <h2 id="board-title">Ideas worth a conversation</h2>
                    </div>
                    <Toolbar orientation="horizontal">
                        <ToolbarButton
                            icon="pi pi-plus"
                            text="Capture idea"
                            title="Capture idea"
                            onClick={() => { void showCaptureDialog(); }}
                        />
                    </Toolbar>
                </div>

                <label className="search-box">
                    <i className="pi pi-search" aria-hidden="true" />
                    <span className="visually-hidden">Search ideas</span>
                    <input
                        type="search"
                        value={viewModel.searchTerm}
                        placeholder="Search title or summary"
                        onChange={event => viewModel.setSearchTerm(event.target.value)}
                    />
                </label>

                {ideasResult.isPerforming && !ideasResult.hasData && (
                    <div className="board-state"><i className="pi pi-spin pi-spinner" /> Connecting to the live board…</div>
                )}

                {!ideasResult.isPerforming && capturedCount === 0 && (
                    <div className="empty-state">
                        <span className="empty-state__icon"><i className="pi pi-sparkles" /></span>
                        <h3>Give the board its first spark</h3>
                        <p>Capture one concrete improvement. Arc will validate it, run the command, and push the new read model here.</p>
                        <button type="button" onClick={() => { void showCaptureDialog(); }}>Capture the first idea</button>
                    </div>
                )}

                {capturedCount > 0 && ideas.length === 0 && (
                    <div className="board-state">No ideas match “{viewModel.searchTerm}”. Try a broader phrase.</div>
                )}

                {ideas.length > 0 && (
                    <div className="idea-grid">
                        {ideas.map((idea, index) => (
                            <IdeaCard key={idea.id.toString()} idea={idea} sequence={index + 1} />
                        ))}
                    </div>
                )}
            </section>

            <footer>
                <span>Cratis Arc</span>
                <span>React 19</span>
                <span>Generated contracts</span>
                <span>In-memory current state</span>
            </footer>
            <CaptureDialog />
        </main>
    );
});

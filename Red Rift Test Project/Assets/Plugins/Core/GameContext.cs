using System.Collections.Immutable;

namespace GameCore
{
    public class GameContext
    {
        public GameState State { get; }
        public ImmutableQueue<ViewAction> Actions { get; }

        public GameContext(GameState state, ImmutableQueue<ViewAction> actions = null)
        {
            State = state;
            Actions = actions ?? ImmutableQueue<ViewAction>.Empty;
        }

        public GameContext AddViewAction(ViewAction a)
        {
            return new GameContext(State, Actions.Enqueue(a));
        }
    }
}
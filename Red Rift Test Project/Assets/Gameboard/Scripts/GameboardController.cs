using Gameboard.Scripts;
using GameCore;
using GameCore.Logic;

public class GameBoardController
{
    public GameState State { get; private set; }
    public IGameBoardView View { get; }

    public GameBoardController(GameState state)
    {
        State = state;
    }

    public void StartGame()
    {
        var ctx = new GameContext(State).StartGame();
        State = ctx.State;
        ProcessActions();
    }

    private void ProcessActions(ViewAction[] actions)
    {
        d
    }
}

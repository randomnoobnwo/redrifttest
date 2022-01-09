namespace GameCore.GameMoves
{
    public class GameMove
    {
        
    }

    public class PlayCardMove : GameMove
    {
        public int CardId { get; }

        public PlayCardMove(int cardId)
        {
            CardId = cardId;
        }
    }

    public class RandomChangeMove : GameMove
    {
        public bool TillTheEnd;

        public RandomChangeMove(bool tillTheEnd)
        {
            TillTheEnd = tillTheEnd;
        }
    }
}
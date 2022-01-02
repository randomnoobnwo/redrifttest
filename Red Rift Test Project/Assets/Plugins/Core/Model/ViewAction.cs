namespace GameCore
{
    public class ViewAction
    {
        
    }

    public class MoveCardsAction : ViewAction
    {
        public int[] Cards;
        public CardLocation Target;
    }
}
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

    public class CardIntAttributeUpdate : ViewAction
    {
        public int Card;
        public int OldValue;
        public int NewValue;
    }
    
    public class CardAttackUpdate : CardIntAttributeUpdate {}
    public class CardHealthUpdate : CardIntAttributeUpdate {}
    public class CardManaCostUpdate : CardIntAttributeUpdate {}
}
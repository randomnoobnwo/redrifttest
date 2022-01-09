using System.Linq;

namespace GameCore.Logic
{
    public static class InputsLogic
    {
        public static Input GetInput(this GameState state)
        {
            return new MainInput()
            {
                PlayableCards = state.GetCardsInLoc(CardLocation.Hand).Where(c => state.GetCardCost(c) <= state.Fire).ToList(),
                Fire = state.Fire,
                DeckCount = state.GetCardsInLoc(CardLocation.Deck).Count,
                HandCount = state.GetCardsInLoc(CardLocation.Hand).Count,
                PlayCount = state.GetCardsInLoc(CardLocation.Play).Count
            };
        }
    }
}
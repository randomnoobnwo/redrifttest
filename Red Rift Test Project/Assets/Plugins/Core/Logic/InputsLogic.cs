using System.Linq;

namespace GameCore.Logic
{
    public static class InputsLogic
    {
        public static Input GetInput(this GameState state)
        {
            return new MainInput()
            {
                PlayableCards = state.CardLocationMap.GetLocCards(CardLocation.Hand).Where(c => state.GetCardCost(c) <= state.Fire).ToList()
            };
        }
    }
}
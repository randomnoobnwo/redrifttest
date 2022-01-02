using System;

namespace GameCore.Logic
{
    public static class StateLogic
    {
        public static CardState GetCardState(this GameState s, int id)
        {
            CardState v;
            return s.CardStateMap.TryGetValue(id, out v) ? s.CardStateMap[id] : CardState.Empty;
        }

        public static int GetCardCost(this GameState s, int id)
        {
            var cardBase = s.GetCardBase(id);

            return s.GetCardState(id).GetCost(cardBase);
        }

        public static CardBase GetCardBase(this GameState s, int id)
        {
            if (s.CardBaseMap.TryGetValue(id, out CardBase value))
                return value;

            throw new Exception("Card not found");
        }
    }
}
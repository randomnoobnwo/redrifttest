using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace GameCore.Logic
{
    public static class StateLogic
    {
        public static ImmutableList<int> GetCardsInLoc(this GameState state, CardLocation loc)
        {
            return state.CardLocationMap.GetLocCards(loc);
        }       
        
        public static ImmutableDictionary<int, CardState> GetCardStatesInLoc(this GameState state, CardLocation loc)
        {
            return ImmutableDictionary<int, CardState>.Empty
                .AddRange(state.CardLocationMap.GetLocCards(loc).Select(c => new KeyValuePair<int, CardState>(c, state.GetCardState(c))));
        }
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
        
        public static int GetCardHealth(this GameState s, int id)
        {
            var cardBase = s.GetCardBase(id);

            return s.GetCardState(id).GetHealth(cardBase);
        }

        public static int CountCardMods(this GameState s, int id)
        {
            return s.GetCardState(id).Modifiers.Count;
        }

        public static CardBase GetCardBase(this GameState s, int id)
        {
            if (s.CardBaseMap.TryGetValue(id, out CardBase value))
                return value;

            throw new Exception("Card not found");
        }

        public static GameState AddCardBase(this GameState state, CardBase card, CardLocation loc = CardLocation.Deck)
        {
            var nextCardId = state.CardBaseMap.IsEmpty ? 1 : state.CardBaseMap.Keys.Max() + 1;
            return state.With(cardBaseMap: state.CardBaseMap.Add(nextCardId, card))
                .With(stringBaseMap: state.StringBaseMap.Add(card.Id, card))
                .With(cardLocationMap: state.CardLocationMap.AddCard(nextCardId, loc));
        }
    }
}
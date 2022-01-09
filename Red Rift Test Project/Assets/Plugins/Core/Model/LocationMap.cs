using System;
using System.Collections.Immutable;

namespace GameCore
{
    public class LocationMap
    {
        public ImmutableDictionary<int, CardLocation> cardMap = ImmutableDictionary<int, CardLocation>.Empty;
        
        public ImmutableDictionary<CardLocation, ImmutableList<int>> locMap = ImmutableDictionary<CardLocation, ImmutableList<int>>.Empty;
        
        public static LocationMap Empty = new LocationMap();
        
        public LocationMap AddCard(int id, CardLocation loc)
        {
            return new LocationMap()
            {
                cardMap = cardMap.Add(id, loc),
                locMap = locMap.Update(
                    loc, 
                    cardList => cardList.Add(id))
            };
        }
        
        public LocationMap MoveCard(int id, CardLocation loc)
        {
            var oldLoc = cardMap[id];

            return new LocationMap() {
                cardMap = cardMap.SetItem(id, loc),
                locMap = locMap
                    .Update(oldLoc, l => l.Remove(id))
                    .Update(loc, l => l.Add(id))
            };
        }
        
        public CardLocation GetCardLoc(int id)
        {
            return cardMap[id];
        }

        public ImmutableList<int> GetLocCards(CardLocation loc)
        {
            ImmutableList<int> v;
            return locMap.TryGetValue(loc, out v) ? v: ImmutableList<int>.Empty;
        }
    }
    
    static class DictionaryHelper
    {
        public static ImmutableDictionary<TA, ImmutableList<TB>> Update<TA, TB>(this ImmutableDictionary<TA, ImmutableList<TB>> d, TA key, Func<ImmutableList<TB>, ImmutableList<TB>> f)
        {
            ImmutableList<TB> v;
            return d.TryGetValue(key, out v) ? d.SetItem(key, f(v)) : d.SetItem(key, f(ImmutableList<TB>.Empty));
        }
    }
}
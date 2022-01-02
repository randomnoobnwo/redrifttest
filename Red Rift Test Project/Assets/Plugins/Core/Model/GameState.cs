using System;
using System.Collections.Immutable;

namespace GameCore
{
    public class GameState
    {
        public ImmutableDictionary<int, CardBase> CardBaseMap { get; }
        public ImmutableDictionary<string, CardBase> StringBaseMap { get; }

        public ImmutableDictionary<int, CardState> CardStateMap { get; }
        public LocationMap CardLocationMap { get; }
        
        public int RandomSeed { get; }
        public int Fire { get; }

        public GameState(ImmutableDictionary<int, CardBase> cardBaseMap = null,
            ImmutableDictionary<string, CardBase> stringBaseMap = null,
            ImmutableDictionary<int, CardState> cardStateMap = null,
            LocationMap cardLocationMap = null,
            int? fire = null,
            int? randomSeed = null)
        {
            CardBaseMap = cardBaseMap ?? ImmutableDictionary<int, CardBase>.Empty;
            StringBaseMap = stringBaseMap ?? ImmutableDictionary<string, CardBase>.Empty;
            CardStateMap = cardStateMap ?? ImmutableDictionary<int, CardState>.Empty;
            CardLocationMap = cardLocationMap ?? LocationMap.Empty;
            Fire = fire ?? 0;
            RandomSeed = randomSeed ?? (int) DateTime.UtcNow.Ticks;
        }

        public GameState With(ImmutableDictionary<int, CardBase> cardBaseMap = null,
            ImmutableDictionary<string, CardBase> stringBaseMap = null,
            ImmutableDictionary<int, CardState> cardStateMap = null,
            LocationMap cardLocationMap = null,
            int? fire = null,
            int? randomSeed = null)
        {
            return new GameState(
                cardBaseMap ?? CardBaseMap,
                stringBaseMap ?? StringBaseMap,
                cardStateMap ?? CardStateMap,
                cardLocationMap ?? CardLocationMap,
                fire ?? Fire,
                randomSeed ?? RandomSeed);
        }
    }
}
using System.Collections.Immutable;
using System.Linq;

namespace GameCore.Logic
{
    public static class CardStateLogic
    {
        public static int GetHealth(this CardState s, CardBase b)
        {
            return b.ManaCost + s.GetValueFromMods(CardMod.Health);
        }
        public static int GetCost(this CardState s, CardBase b)
        {
            return b.ManaCost + s.GetValueFromMods(CardMod.ManaCost);
        }
        
        public static int GetValueFromMods(this CardState s, string key)
        {
            return s.Modifiers.Where(b => b.OfType(key)).Sum(mod => mod.IntMod ?? 0);
        }

        public static CardState AddMod(this CardState state, CardMod mod)
        {
            return state.Mutate(state.Modifiers.Add(mod));
        }

        public static CardState ClearMods(this CardState state)
        {
            return state.Mutate(ImmutableList<CardMod>.Empty);
        }
    }
}
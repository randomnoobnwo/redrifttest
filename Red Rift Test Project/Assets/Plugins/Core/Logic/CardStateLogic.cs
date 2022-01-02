using System.Linq;

namespace GameCore.Logic
{
    public static class CardStateLogic
    {
        public static int GetCost(this CardState s, CardBase b)
        {
            return b.ManaCost + s.GetCostFromMods();
        }
        
        private static int GetCostFromMods(this CardState s)
        {
            return s.Modifiers.Where(b => b.OfType(CardMod.ManaCost)).Sum(mod => mod.IntMod ?? 0);
        }
    }
}
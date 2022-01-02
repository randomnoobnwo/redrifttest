using System.Collections.Immutable;
using UnityEngine.Experimental.XR.Interaction;

namespace GameCore
{
    public class CardState
    {
        public ImmutableList<CardMod> Modifiers;

        private CardState(ImmutableList<CardMod> mods = null)
        {
            Modifiers = mods ?? ImmutableList<CardMod>.Empty;
        }

        public static CardState Empty => new CardState();

        public CardState Mutate(ImmutableList<CardMod> mods)
        {
            return new CardState(mods);
        }
    }
}
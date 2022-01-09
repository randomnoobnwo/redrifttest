using System;
using System.Linq;

namespace GameCore.Logic
{
    public static class GameLogic
    {
        public static GameContext StartGame(this GameContext ctx)
        {
            ctx = ctx.RandomInt(4, 6, out int cardsToDraw);
            return ctx.DrawCards(cardsToDraw);
        }

        public static GameContext DrawCards(this GameContext ctx, int cardsCount)
        {
            var cardsDrawn = ctx.State.CardLocationMap.GetLocCards(CardLocation.Deck).Take(cardsCount).ToArray();
            return ctx.MoveCards(cardsDrawn, CardLocation.Hand);
        }

        public static GameContext PlayCard(this GameContext ctx, int cardId)
        {
            var cardCost = ctx.State.GetCardCost(cardId);

            return ctx.WithState(s => s.With(fire: s.Fire - Math.Max(0 , cardCost)))
                .MoveCards(new[] {cardId}, CardLocation.Play);
        }

        public static GameContext RandomChange(this GameContext ctx)
        {
            var handCards = ctx.State.GetCardsInLoc(CardLocation.Hand);
            var minMods = handCards.Min(s => ctx.State.CountCardMods(s));

            var cardId = handCards.FirstOrDefault(s => ctx.State.CountCardMods(s) == minMods);

            if (cardId is default(int))
                return ctx;

            ctx = ctx.RandomInt(0, 11, out int attrChange);

            attrChange -= 6;

            ctx = ctx.RandomInt(0, 2, out int randomAttribute);

            CardMod randomMod;

            switch (randomAttribute)
            {
                case 0:
                    randomMod = CardMod.AttackMod(attrChange);
                    break;
                case 1:
                    randomMod = CardMod.HealthMod(attrChange);
                    break;
                default:
                    randomMod = CardMod.ManaCostMod(attrChange);
                    break;
            }

            ctx = ctx.AddModToCard(cardId, randomMod);

            ctx = ctx.MoveCards(
                ctx.State.GetCardsInLoc(CardLocation.Hand).Where(c => ctx.State.GetCardHealth(c) <= 0).ToArray(),
                CardLocation.Deck);

            return ctx;
        }

        private static GameContext RemoveAllMods(this GameContext ctx, int cardId)
        {
            var cardState = ctx.State.GetCardState(cardId);

            var oldAttackValue = cardState.GetValueFromMods(CardMod.Attack);
            var oldHealthValue = cardState.GetValueFromMods(CardMod.Health);
            var oldCostValue = cardState.GetValueFromMods(CardMod.ManaCost);
            
            cardState = cardState.ClearMods();

            var newAttackValue = cardState.GetValueFromMods(CardMod.Attack);
            var newHealthValue = cardState.GetValueFromMods(CardMod.Health);
            var newCostValue = cardState.GetValueFromMods(CardMod.ManaCost);

            ctx = ctx.WithState(s => s.With(cardStateMap: s.CardStateMap.SetItem(cardId, cardState)));

            return ctx.AddViewAction(ctx.GetActionFromMod(cardId, CardMod.Attack, oldAttackValue, newAttackValue))
                .AddViewAction(ctx.GetActionFromMod(cardId, CardMod.Health, oldHealthValue, newHealthValue))
                .AddViewAction(ctx.GetActionFromMod(cardId, CardMod.ManaCost, oldCostValue, newCostValue));
        }

        private static GameContext AddModToCard(this GameContext ctx, int cardId, CardMod mod)
        {
            var cardState = ctx.State.GetCardState(cardId);

            var oldCardValue = cardState.GetValueFromMods(mod.Key);
            
            cardState = cardState.AddMod(mod);

            var newCardValue = cardState.GetValueFromMods(mod.Key);

            ctx = ctx.WithState(s => s.With(cardStateMap: s.CardStateMap.SetItem(cardId, cardState)));

            return ctx.AddViewAction(ctx.GetActionFromMod(cardId, mod.Key, oldCardValue, newCardValue));
        }

        private static ViewAction GetActionFromMod(this GameContext ctx, int cardId, string modKey, int oldValue, int newValue)
        {
            var baseCard = ctx.State.GetCardBase(cardId);
            switch (modKey)
            {
                case CardMod.Health:
                    return new CardHealthUpdate()
                    {
                        Card = cardId,
                        OldValue = baseCard.HealthPoints + oldValue,
                        NewValue = baseCard.HealthPoints + newValue
                    };
                case CardMod.Attack:
                    return new CardAttackUpdate()
                    {
                        Card = cardId,
                        OldValue = baseCard.Attack + oldValue,
                        NewValue = baseCard.Attack + newValue
                    };               
                case CardMod.ManaCost:
                    return new CardManaCostUpdate()
                    {
                        Card = cardId,
                        OldValue = baseCard.ManaCost + oldValue,
                        NewValue = baseCard.ManaCost + newValue
                    };
                default:
                    throw new Exception("Unknown mod key");
            }
        }

        private static GameContext MoveCards(this GameContext ctx, int[] cards, CardLocation loc)
        {
            if (cards.Length == 0)
                return ctx;
            
            ctx = cards.Aggregate(ctx,
                (current, card) =>
                    current.WithState(s => s.With(cardLocationMap: s.CardLocationMap.MoveCard(card, loc))));

            if (loc == CardLocation.Deck)
            {
                ctx = cards.Aggregate(ctx, (current, cardId) => current.RemoveAllMods(cardId));
            }

            return ctx.AddViewAction(new MoveCardsAction()
            {
                Cards = cards,
                Target = loc
            });
        }

        private static GameContext WithState(this GameContext ctx, Func<GameState, GameState> f)
        {
            return new GameContext(f(ctx.State), ctx.Actions);
        }

        private static GameContext RandomInt(this GameContext ctx, int startInclusive, int endInclusive, out int result)
        {
            var random = new Random(ctx.State.RandomSeed);
            result = random.Next(startInclusive, endInclusive + 1);

            return new GameContext(ctx.State.With(randomSeed: NextSeed(ctx.State.RandomSeed)));
        }

        private static int NextSeed(int seed)
        {
            // https://nuclear.llnl.gov/CNP/rng/rngman/node4.html
            return Math.Abs(Convert.ToInt32((seed * 2862933555777941757L + 3037000493L) >> 32));
        }
    }
}
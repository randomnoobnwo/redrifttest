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

        public static GameContext MoveCards(this GameContext ctx, int[] cards, CardLocation loc)
        {
            ctx = cards.Aggregate(ctx, (current, card) => current.WithState(s => s.With(cardLocationMap: s.CardLocationMap.MoveCard(card, loc))));
            
            return ctx.AddViewAction(new MoveCardsAction()
            {
                Cards = cards,
                Target = loc
            });
        }

        public static GameContext WithState(this GameContext ctx, Func<GameState, GameState> f)
        {
            return new GameContext(f(ctx.State), ctx.Actions);
        }

        public static GameContext RandomInt(this GameContext ctx, int startInclusive, int endInclusive, out int result)
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
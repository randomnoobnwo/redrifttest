using System;
using System.Linq;
using Gameboard.Scripts.Inputs;
using GameCore;
using GameCore.GameMoves;

namespace Gameboard.Scripts
{
    public static class InputController
    {
        public static MainInputView GetInputView(GameBoardController controller, Input input,
            Func<int, ICardView> getViewCard)
        {
            if (input is MainInput mainInput)
                return new MainInputView()
                {
                    Fire = mainInput.Fire,
                    CardVisuals = mainInput.PlayableCards.Select(c => new ViewCardVisual()
                    {
                        Card = getViewCard(c),
                        Highlight = true
                    }).ToArray(),
                    CardInputs = mainInput.PlayableCards.Select(c => new ViewCardInput()
                    {
                        Card = getViewCard(c),
                        OnDrag = () => controller.ExecuteMove(new PlayCardMove(c))
                    }).ToArray(),
                    RandomChange = mainInput.HandCount == 0 ? (Action) null : () => controller.ExecuteMove(new RandomChangeMove(false)),
                    RandomChangeInfinite = mainInput.HandCount == 0 ? (Action) null : () => controller.ExecuteMove(new RandomChangeMove(true)),
                    Stats = $"Playable: {mainInput.PlayableCards.Count} Deck: {mainInput.DeckCount} Hand: {mainInput.HandCount} Play: {mainInput.PlayCount}"
                };

            throw new Exception($"Input {input} not supported");
        }
    }
}
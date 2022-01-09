using System;
using System.Collections.Generic;
using Gameboard.Scripts.Inputs;

namespace Gameboard.Scripts
{
    public interface IGameBoardView
    {
        void MoveCards(IEnumerable<ICardView> cards, ViewCardLocation loc);

        ICardView CreateCard(string id, string title, string description, int attack, int healthPoints, int manaCost,
            string artTextureUrl);

        void SetupInput(MainInputView input);
        void NoInput();
        void UpdateCardHealth(ICardView cardView, int oldValue, int newValue);
        void UpdateCardAttack(ICardView cardView, int oldValue, int newValue);
        void UpdateCardManaCost(ICardView cardView, int oldValue, int newValue);
        void EnqueueAction(Action action);
    }
}
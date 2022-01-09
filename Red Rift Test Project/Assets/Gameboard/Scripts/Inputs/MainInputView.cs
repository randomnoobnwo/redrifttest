using System;

namespace Gameboard.Scripts.Inputs
{
    public class MainInputView
    {
        public ViewCardInput[] CardInputs;
        public ViewCardVisual[] CardVisuals;
        public Action RandomChange;
        public Action RandomChangeInfinite;
        public Action DrawCards;
        public int Fire;
        public string Stats;
    }

    public class ViewCardVisual
    {
        public ICardView Card;
        public bool Highlight;
    }

    public class ViewCardInput
    {
        public ICardView Card;
        public Action OnDrag;
    }
}
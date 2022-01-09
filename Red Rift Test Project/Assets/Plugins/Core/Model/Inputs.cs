using System.Collections.Generic;

namespace GameCore
{
    public class Input
    {
        
    }

    public class MainInput : Input
    {
        public List<int> PlayableCards { get; set; }
        public int Fire { get; set; }
        public int DeckCount { get; set; }
        public int HandCount { get; set; }
        public int PlayCount { get; set; }
    }
}
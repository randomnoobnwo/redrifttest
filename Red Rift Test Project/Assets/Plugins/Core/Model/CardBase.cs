namespace GameCore
{
    public class CardBase
    {
        public string Id { get; }
        public string Title { get; }
        public string Description { get; }
        public int Attack { get; }
        public int HealthPoints { get; }
        public int ManaCost { get; }
        public string ArtTexture { get; }
    }
}
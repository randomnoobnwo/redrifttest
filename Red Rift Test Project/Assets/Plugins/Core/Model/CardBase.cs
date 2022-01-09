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
        public string ArtTextureUrl { get; }

        public CardBase(string id, string title, string description, int attack, int healthPoints, int manaCost,
            string artTextureUrl)
        {
            Id = id;
            Title = title;
            Description = description;
            Attack = attack;
            HealthPoints = healthPoints;
            ManaCost = manaCost;
            ArtTextureUrl = artTextureUrl;
        }
    }
}
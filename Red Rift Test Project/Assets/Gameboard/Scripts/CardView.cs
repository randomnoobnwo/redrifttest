using UnityEngine;

namespace Gameboard.Scripts
{
    public class CardView : MonoBehaviour, ICardView
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Attack { get; set; }
        public int HealthPoints { get; set; }
        public int ManaCost { get; set; }
        public string ArtTextureUrl { get; set; }

        public void Init(string id, string title, string description, int attack, int healthPoints, int manaCost,
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

    public interface ICardView
    {
        
    }
}
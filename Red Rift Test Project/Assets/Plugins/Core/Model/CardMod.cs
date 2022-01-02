namespace GameCore
{
    public class CardMod
    {
        public string Key { get; }
        public int? IntMod { get; }

        public const string Health = "health";
        public const string Attack = "attack";
        public const string ManaCost = "manaCost";

        public CardMod(string key, int modValue)
        {
            Key = key;
            IntMod = modValue;
        }

        public bool OfType(string key)
        {
            return Key.Equals(key);
        }

        public static CardMod HealthMod(int mod)
        {
            return new CardMod(Health, mod);
        }

        public static CardMod AttackMod(int mod)
        {
            return new CardMod(Attack, mod);
        }

        public static CardMod ManaCostMod(int mod)
        {
            return new CardMod(ManaCost, mod);
        }
    }
}
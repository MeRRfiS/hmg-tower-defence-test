namespace TowerDefence.Characters
{
    public class CharacterModel
    {
        public int Health { get; set; }
        public int Damage { get; set; }
        public float DurationBetweenAttacks { get; set; }
        public float Speed  { get; set; }
        public float Range { get; set; }
        public float DetectionRadius { get; set; }

        public CharacterModel(CharacterConfig config)
        {
            Health = config.Health;
            Damage = config.Damage;
            DurationBetweenAttacks = config.DurationBetweenAttacks;
            Speed = config.Speed;
            Range = config.Range;
            DetectionRadius = config.DetectionRadius;
        }
    }
}
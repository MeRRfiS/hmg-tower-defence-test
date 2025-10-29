namespace TowerDefence.Characters
{
    public class CharacterModel
    {
        public int Health { get; set; }
        public int Damage { get; set; }
        public float DurationBetweenAttacks { get; set; }
        public float Speed  { get; set; }
        public float MinRange { get; set; }
        public float MaxRange { get; set; }
        public float DetectionRadius { get; set; }

        public CharacterModel(CharacterConfig config)
        {
            Health = config.Health;
            Damage = config.Damage;
            DurationBetweenAttacks = config.DurationBetweenAttacks;
            Speed = config.Speed;
            MinRange = config.MinRange;
            MaxRange = config.MaxRange;
            DetectionRadius = config.DetectionRadius;
        }
    }
}
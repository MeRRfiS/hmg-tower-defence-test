namespace TowerDefence.Characters
{
    public struct AttackEvent
    {
        public Character Attacker;
        public Character Target;
    }

    public struct GetDamagedEvent 
    {
        public Character Target;
        public int Damage;
    }

    public struct DeathEvent 
    {
        public Character Character;
    }
}
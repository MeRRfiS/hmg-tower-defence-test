using UnityEngine;

namespace TowerDefence.Characters
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "TowerDefence/Game/CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {
        [Header("Character Settings")]
        [SerializeField] private int _health = 100;
        [SerializeField] private int _damage = 5;
        [SerializeField] private float _durationBetweenAttacks = 1f;
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _minRange = 1.5f;
        [SerializeField] private float _maxRange = 1.5f;
        [SerializeField] private float _detectionRadius = 5f;

        public int Health => _health;
        public int Damage => _damage;
        public float DurationBetweenAttacks => _durationBetweenAttacks;
        public float Speed => _speed;
        public float MinRange => _minRange;
        public float MaxRange => _maxRange;
        public float DetectionRadius => _detectionRadius;
    }
}
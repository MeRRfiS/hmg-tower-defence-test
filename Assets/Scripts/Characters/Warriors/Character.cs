using TowerDefence.Core;
using TowerDefence.Game;
using TowerDefence.GameObjects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace TowerDefence.Characters
{
    public class Character : MonoBehaviour, IClickable
    {
        [Header("Character Settings")]
        [SerializeField] protected CharacterConfig _characterConfig;
        [SerializeField] protected NavMeshAgent _agent;
        [SerializeField] private Transform _formationPosition;
        [SerializeField] private LayerMask _targetMask;
        [SerializeField] private bool _isEnemy;

        public bool IsEnemy => _isEnemy;
        public CharacterModel Model => _model;
        public LayerMask TargetMask => _targetMask;
        public NavMeshAgent Agent => _agent;
        public Vector3 FormationPosition => _formationPosition.position;

        protected CharacterModel _model;
        protected IEventBus _eventBus;
        protected ICharacterService _characterService;

        protected virtual void Awake()
        {
            _eventBus = Services.Get<IEventBus>();
            _characterService = Services.Get<ICharacterService>();

            _model = new CharacterModel(_characterConfig);
            _agent.speed = _model.Speed;

            _characterService.SetState(this, new SearchState(this));

            var gameStatsService = Services.Get<IGameStatsService>();
            gameStatsService.AddPlayerHealth(this);
        }

        public virtual void Attack(Character target)
        {
            _eventBus.Publish(new GetDamagedEvent
            {
                Target = target,
                Damage = _model.Damage
            });
        }

        public virtual void GetDamage(int damage)
        {
            _model.Health -= damage;
            if(_model.Health <= 0)
            {
                _characterService.SetState(this, new DeathState(this));
            }
        }

        public virtual void Die()
        {
            _characterService.SetState(this, null);
            gameObject.SetActive(false); //Here must be pooling logic
        }

        public virtual void Click()
        {
            if (_isEnemy) return; //Enamy characters are not selectable

            var formationService = Services.Get<IFormationService>();
            formationService.SelectFormation(this);
        }
    }
}
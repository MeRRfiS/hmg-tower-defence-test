using TowerDefence.Core;
using UnityEngine;

namespace TowerDefence.Characters
{
    public class KitingState : ICharacterState
    {
        private readonly Character _target;

        public Character Character { get; set ; }

        private ICharacterService _characterService;

        public KitingState(Character character, Character target)
        {
            Character = character;
            _target = target;
        }

        public ICharacterState Clone()
        {
            var clone = new KitingState(Character, _target);

            return clone;
        }

        public void OnEnter()
        {
            _characterService = Services.Get<ICharacterService>();
        }

        public void OnExit()
        {
            
        }

        public void Tick(float deltaTime)
        {
            if (!Character.gameObject.activeSelf) return;

            if(_target == null || !_target.gameObject.activeSelf)
            {
                _characterService.SetState(Character, new SearchState(Character));
                return;
            }

            var directionToTarget = (_target.transform.position - Character.transform.position).normalized;
            var distanceMultiplier = Random.Range(Character.Model.MinRange, Character.Model.MaxRange); // Adding some randomness to the kiting distance
            var kitePosition = _target.transform.position - directionToTarget * distanceMultiplier;

            Character.Agent.SetDestination(kitePosition);

            if(Character.Agent.remainingDistance <= Character.Agent.stoppingDistance)
            {
                var formationService = Services.Get<IFormationService>();
                var formation = formationService.GetFormationByCharacter(Character);
                formationService.SetStateForAllMembers(formation, new MoveAndAttackState(Character, _target));
            }
        }
    }
}

using TowerDefence.Core;
using UnityEngine;

namespace TowerDefence.Characters
{
    public class AttackState : ICharacterState
    {
        private Character _target;
        private float _attackTimer;

        public Character Character { get; set; }

        public AttackState(Character character, Character target)
        {
            Character = character;
            _target = target;
        }

        public void OnEnter() { }

        public void OnExit() { }

        public void Tick(float deltaTime)
        {
            if (IsTargetDead()) return;
            if (!Character.gameObject.activeSelf) return;

            var distanceToTarget = Vector3.Distance(Character.transform.position, _target.transform.position);
            if (distanceToTarget > Character.Model.Range)
            {
                Character.Agent.SetDestination(_target.transform.position);
                return;
            }

            Character.Agent.ResetPath();
            _attackTimer += deltaTime;
            if (_attackTimer <= Character.Model.DurationBetweenAttacks) return;

            try 
            {
                var eventBus = Services.Get<IEventBus>();
                eventBus.Publish(new AttackEvent { Attacker = Character, Target = _target });
                _attackTimer = 0f;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"4: {ex}");
            }
        }

        public ICharacterState Clone()
        {
            var clone = new AttackState(Character, _target);

            return clone;
        }

        private bool IsTargetDead()
        {
            if(_target.Model.Health > 0)
            {
                return false;
            }

            _target = null;
            var characterService = Services.Get<ICharacterService>();
            var formationService = Services.Get<IFormationService>();
            var formation = formationService.GetFormationByCharacter(Character);

            if (formation != null)
            {
                formationService.SetStateForAllMembers(formation, new SearchState(Character));
                return true;
            }

            characterService.SetState(Character, new SearchState(Character));

            return true;
        }
    }
}

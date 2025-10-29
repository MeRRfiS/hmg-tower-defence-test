using TowerDefence.Core;
using UnityEngine;

namespace TowerDefence.Characters
{
    public class MoveAndAttackState : AttackState
    {
        public MoveAndAttackState(Character character, Character target) : base(character, target)
        {
        }

        public override void Tick(float deltaTime)
        {
            if (IsTargetDead())
            {
                Character.Agent.ResetPath();
                return;
            }
            if (!Character.gameObject.activeSelf) return;

            var distanceToTarget = Vector3.Distance(Character.transform.position, _target.transform.position);
            if (distanceToTarget > Character.Model.MaxRange)
            {
                Character.Agent.SetDestination(_target.transform.position);
                return;
            }

            // Handle Kiting
            if (distanceToTarget < Character.Model.MinRange && Character.Model.MinRange != Character.Model.MaxRange)
            {
                var characterService = Services.Get<ICharacterService>();
                characterService.SetState(Character, new KitingState(Character, _target));
                return;
            }

            Character.Agent.ResetPath();
            base.Tick(deltaTime);
        }

        public override ICharacterState Clone()
        {
            var clone = new MoveAndAttackState(Character, _target);

            return clone;
        }

        private bool IsTargetDead()
        {
            if (_target.Model.Health > 0)
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

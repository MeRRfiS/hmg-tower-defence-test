using TowerDefence.Core;
using UnityEngine;

namespace TowerDefence.Characters
{
    public class SearchState : ICharacterState
    {
        public Character Character { get; set; }

        public SearchState(Character character)
        {
            Character = character;
        }

        public virtual void OnEnter() 
        {
            Character.Agent.ResetPath();
        }

        public void OnExit() { }

        public virtual void Tick(float deltaTime)
        {
            var colliders = Physics.OverlapSphere(Character.transform.position,
                                                  Character.Model.DetectionRadius,
                                                  Character.TargetMask);
            if(colliders.Length == 0) return;
            var target = colliders[0].GetComponent<Character>();
            var characterService = Services.Get<ICharacterService>();
            var formationService = Services.Get<IFormationService>();
            var formation = formationService.GetFormationByCharacter(Character);

            if(formation != null)
            {
                formationService.SetStateForAllMembers(formation, new MoveAndAttackState(Character, target));
                return;
            }

            characterService.SetState(Character, new MoveAndAttackState(Character, target));
        }

        public virtual ICharacterState Clone()
        {
            var clone = new SearchState(Character);

            return clone;
        }
    }
}
using System.Collections.Generic;
using TowerDefence.Core;

namespace TowerDefence.Characters
{
    public interface IFormationService : IService
    {
        public void RegisterFormation(Formation formation, List<Character> members);
        public Formation GetFormationByCharacter(Character character);
        public void SelectFormation(Character character);
        public void SetStateForAllMembers(Formation formation, ICharacterState state);
    }
}
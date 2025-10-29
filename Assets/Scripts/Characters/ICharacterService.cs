using System;
using TowerDefence.Core;

namespace TowerDefence.Characters
{
    public interface ICharacterService : IService, IDisposable
    {
        public void SetState(Character character, ICharacterState state);
    }
}
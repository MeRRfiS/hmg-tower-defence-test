using TowerDefence.Core;

namespace TowerDefence.Characters
{
    public interface ICharacterState : IState
    {
        public Character Character { get; set; }

        public ICharacterState Clone();
    }
}

using TowerDefence.Core;

namespace TowerDefence.Characters
{
    public class DeathState : ICharacterState
    {
        public Character Character { get; set; }

        public DeathState(Character character)
        {
            Character = character;
        }

        public void OnEnter()
        {
            var eventBus = Services.Get<IEventBus>();
            eventBus.Publish(new DeathEvent { Character = Character });
        }

        public void OnExit() { }

        public void Tick(float deltaTime) { }

        public ICharacterState Clone()
        {
            var clone = new DeathState(Character);

            return clone;
        }
    }
}

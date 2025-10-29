using TowerDefence.Core;

namespace TowerDefence.Characters
{
    public class AttackState : ICharacterState
    {
        protected Character _target;
        private float _attackTimer;

        public Character Character { get; set; }

        public AttackState(Character character, Character target)
        {
            Character = character;
            _target = target;
        }

        public void OnEnter() { }

        public void OnExit() { }

        public virtual void Tick(float deltaTime)
        {
            _attackTimer += deltaTime;
            if (_attackTimer <= Character.Model.DurationBetweenAttacks) return;

            var eventBus = Services.Get<IEventBus>();
            eventBus.Publish(new AttackEvent { Attacker = Character, Target = _target });
            _attackTimer = 0f;
        }

        public virtual ICharacterState Clone()
        {
            var clone = new AttackState(Character, _target);

            return clone;
        }
    }
}

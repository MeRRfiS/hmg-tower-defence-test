using System.Collections.Generic;
using TowerDefence.Core;
using TowerDefence.Game;

namespace TowerDefence.Characters
{
    public class CharacterService : ICharacterService
    {
        private readonly Dictionary<Character, ICharacterState> _charactersState = new();
        private IEventToken _attackToken;
        private IEventToken _getDamageToken;
        private IEventToken _deathToken;

        private IEventBus _eventBus;
        private ITickDispatcher _tickDispatcher;

        public void Init()
        {
            _eventBus = Services.Get<IEventBus>();
            _tickDispatcher = Services.Get<ITickDispatcher>();

            _attackToken = _eventBus.Subscribe<AttackEvent>(Attack);
            _getDamageToken = _eventBus.Subscribe<GetDamagedEvent>(GetDamage);
            _deathToken = _eventBus.Subscribe<DeathEvent>(Death);
            _tickDispatcher.Subscribe(Tick);
        }

        public void SetState(Character character, ICharacterState state)
        {
            if (_charactersState.TryGetValue(character, out var oldState))
            {
                if (oldState == state) return;
                oldState?.OnExit();
            }

            _charactersState[character] = state;
            _charactersState[character]?.OnEnter();
        }

        private void Tick(float deltaTime)
        {
            if (_charactersState.Count == 0)
            {
                return;
            }

            var states = new List<ICharacterState>(_charactersState.Values);

            foreach (var characterState in states)
            {
                characterState?.Tick(deltaTime);
            }
        }

        private void Attack(AttackEvent evt)
        {
            if (evt.Attacker == null || evt.Target == null) return;

            evt.Attacker.Attack(evt.Target);
        }

        private void GetDamage(GetDamagedEvent evt)
        {
            if (evt.Target == null) return;

            evt.Target.GetDamage(evt.Damage);
        }

        private void Death(DeathEvent evt)
        {
            if(evt.Character == null) return;
            if(evt.Character.IsEnemy)
            {
                var gameStatsService = Services.Get<IGameStatsService>();
                gameStatsService.KillEnemy();
            }

            evt.Character.Die();
        }

        public void Dispose()
        {
            _tickDispatcher.Unsubscribe(Tick);
            _eventBus.Unsubscribe(_attackToken);
            _eventBus.Unsubscribe(_getDamageToken);
            _eventBus.Unsubscribe(_deathToken);
        }
    }
}
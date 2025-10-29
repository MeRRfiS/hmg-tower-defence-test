using TowerDefence.Characters;
using TowerDefence.Core;

namespace TowerDefence.Game
{
    public class GameStatsService : IGameStatsService
    {
        private int _killedEnemiesAmount;
        private int _allPlayerHealth;

        private IEventBus _eventBus;

        public void Init()
        {
            _eventBus = Services.Get<IEventBus>();
        }

        public void KillEnemy()
        {
            _killedEnemiesAmount++;
            _eventBus.Publish(new KillEnemyEvent { KilledAmount = _killedEnemiesAmount });
        }

        public void AddPlayerHealth(Character character)
        {
            if (character.IsEnemy) return;

            _allPlayerHealth += character.Model.Health;
            _eventBus.Publish(new StartGameEvent { PlayerHealth = _allPlayerHealth });
        }
    }
}

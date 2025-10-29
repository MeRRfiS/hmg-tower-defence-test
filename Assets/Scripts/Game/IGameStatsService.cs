using TowerDefence.Characters;
using TowerDefence.Core;

namespace TowerDefence.Game
{
    public interface IGameStatsService : IService
    {
        public void AddPlayerHealth(Character character);
        public void KillEnemy();
    }
}

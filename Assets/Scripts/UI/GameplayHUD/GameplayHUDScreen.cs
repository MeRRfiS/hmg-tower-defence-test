using TMPro;
using TowerDefence.Characters;
using TowerDefence.Core;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UI
{
    public class GameplayHUDScreen : BaseScreen
    {
        [SerializeField] private TMP_Text _enemyKillCountText;
        [SerializeField] private Slider _healthBar;

        private IEventBus _eventBus;
        private IEventToken _updateKillCountToken;
        private IEventToken _updateHealthBarToken;
        private IEventToken _setupHealthBarToken;

        protected override void Awake()
        {
            _eventBus = Services.Get<IEventBus>();
            _setupHealthBarToken = _eventBus.Subscribe<StartGameEvent>(SetupSliderValue);
            _updateKillCountToken = _eventBus.Subscribe<KillEnemyEvent>(UpdateEnemyKillCount);
            _updateHealthBarToken = _eventBus.Subscribe<GetDamagedEvent>(UpdateSliderValue);

            base.Awake();
        }

        protected override void OnDestroy()
        {
            _eventBus.Unsubscribe(_updateKillCountToken);
            _eventBus.Unsubscribe(_updateHealthBarToken);
            _eventBus.Unsubscribe(_setupHealthBarToken);

            base.OnDestroy();
        }

        public void SetupSliderValue(StartGameEvent evt)
        {
            _healthBar.maxValue = evt.PlayerHealth;
            _healthBar.value = evt.PlayerHealth;
        }

        public void UpdateSliderValue(GetDamagedEvent evt)
        {
            if (evt.Target.IsEnemy) return;

            _healthBar.value -= evt.Damage;
        }

        private void UpdateEnemyKillCount(KillEnemyEvent evt)
        {
            _enemyKillCountText.text = evt.KilledAmount.ToString();
        }
    }
}
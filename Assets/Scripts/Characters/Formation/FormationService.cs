using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowerDefence.Core;
using TowerDefence.Systems;
using UnityEngine;

namespace TowerDefence.Characters
{
    public class FormationService : IFormationService
    {
        private readonly Dictionary<Formation, List<Character>> _formations = new();
        private Formation _selectedFormation;

        private IInputService _inputService;
        private ICharacterService _characterService;

        public void Init()
        {
            _inputService = Services.Get<IInputService>();
            _characterService = Services.Get<ICharacterService>();
            var eventBus = Services.Get<IEventBus>();
            eventBus.Subscribe<DeathEvent>(RemoveCharacterFromFormations);
        }

        public void RegisterFormation(Formation formation, List<Character> members)
        {
            if (!_formations.ContainsKey(formation))
            {
                _formations.Add(formation, members);
            }
        }

        public void SelectFormation(Character character)
        {
            _selectedFormation = GetFormationByCharacter(character);

            if (_selectedFormation != null)
            {
                _inputService.OnTap += MoveFormation;
                return;
            }
        }

        public void SetStateForAllMembers(Formation formation, ICharacterState state)
        {
            if(_formations.TryGetValue(formation, out var members))
            {
                foreach (var character in members)
                {
                    var newState = state.Clone();
                    newState.Character = character;
                    _characterService.SetState(character, newState);
                }

                return;
            }

            Debug.LogWarning("Formation not registered.");
        }

        private async void MoveFormation(Vector2 position)
        {
            if(_selectedFormation != null)
            {
                SetStateForAllMembers(_selectedFormation, new MoveAndSearchState(null));
                var maxMemberSpeed = _formations[_selectedFormation].Max(member => member.Model.Speed);

                await MoveAnimationAsync(position, 2f * maxMemberSpeed);
            }

            _inputService.OnTap -= MoveFormation;
        }

        public Formation GetFormationByCharacter(Character character)
        {
            if (_formations.Count == 0)
            {
                Debug.LogWarning("No formations available to select.");
                return null;
            }

            foreach (var formation in _formations)
            {
                if (formation.Value.Contains(character))
                {
                    return formation.Key;
                }
            }

            return null;
        }

        private async Task MoveAnimationAsync(Vector2 position, float time)
        {
            var formationTransform = _selectedFormation.transform;
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var anim = DOTween.Sequence()
                    .Append(formationTransform.DOLookAt(hit.point, time))
                    .Join(formationTransform.DOMove(hit.point, time))
                    .OnComplete(() =>
                    {
                        _selectedFormation = null;
                    });

                await anim?.AsyncWaitForCompletion();
            }
        }

        private void RemoveCharacterFromFormations(DeathEvent evt)
        {
            if(evt.Character == null)
                return;

            var formation = GetFormationByCharacter(evt.Character);

            if (formation == null)
                return;

            if (_formations.TryGetValue(formation, out var characters))
            {
                if(characters.Contains(evt.Character))
                    characters.Remove(evt.Character);
            }
        }
    }
}
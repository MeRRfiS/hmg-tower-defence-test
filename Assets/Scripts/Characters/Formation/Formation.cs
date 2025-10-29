using System.Collections.Generic;
using TowerDefence.Core;
using UnityEngine;

namespace TowerDefence.Characters
{
    public class Formation : MonoBehaviour
    {
        [SerializeField] private List<Character> _members = new();

        private void Awake()
        {
            var formationService = Services.Get<IFormationService>();
            formationService.RegisterFormation(this, _members);
        }
    }
}
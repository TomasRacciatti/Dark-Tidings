using System.Collections.Generic;
using Items.Base;
using Managers.ObjectPool;
using TMPro;
using UnityEngine;

namespace Characters
{
    public class TargetDummy : Character
    {
        [SerializeField] private GameObject indicator;

        private string GetText(float value, List<ItemAmount> modifiers = null, string label = "")
        {
            string modifierNames = "";

            if (modifiers != null && modifiers.Count > 0)
            {
                foreach (var modifier in modifiers)
                {
                    modifierNames += $"{modifier.SoItem.name}, ";
                }
                modifierNames = modifierNames[..^2] + ".";
            }
            else
            {
                modifierNames = "None.";
            }

            return $"{label}: {value}\nModifiers: {modifierNames}";
        }

        private void ShowIndicator(string text)
        {
            GameObject obj = ObjectPoolManager.Instance.SpawnObject(indicator, transform.position + Vector3.up * 1,
                Quaternion.identity, 5f);
            var textMeshPro = obj.GetComponentInChildren<TextMeshProUGUI>();
            textMeshPro.text = text;
        }

        protected override void Awake()
        {
            base.Awake();
            _healthComponent.OnDamaged += (damage, modifiers) =>
                ShowIndicator(GetText(damage, modifiers, "Damage"));
            _healthComponent.OnHealed += (healing, modifiers) =>
                ShowIndicator(GetText(healing, modifiers, "Healed"));
            _healthComponent.OnDeath += _healthComponent.Revive;
            _healthComponent.OnDeath += () => ShowIndicator("Revived");
        }
    }
}
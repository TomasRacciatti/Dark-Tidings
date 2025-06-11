using Objects.Clues;
using UnityEngine;
using Interfaces;
using Items.Base;
using Managers;
using TMPro;

namespace Items.Tools
{
    public class Thermometer : Tool
    {
        [Header("UI")]
        [SerializeField] private TMP_Text text;

        [Header("Settings")]
        [SerializeField] private SO_ThermClue defaultTemp;
        [SerializeField] private LayerMask clueLayerMask;
        [SerializeField] private float cooldownTime = 1f;
        [SerializeField] private float lerpSpeed = 0.5f;
        [SerializeField] private float detectionRadius = 1f;

        private float _temperature = 25f;
        private float _targetTemperature = 25f;
        private Cooldown _cooldown = new();

        private void OnEnable()
        {
            UpdateTemperatureDisplay();
        }

        public override void Use(UseType useType)
        {
            
        }

        private void Update()
        {
            UpdateTargetTemperature();
            UpdateTemperature();
        }
        
        private void UpdateTargetTemperature()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, clueLayerMask);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out Clue clue) && clue.GetClueProvided is SO_ThermClue tempClue)
                {
                    TryUpdateTargetTemperature(tempClue.GetValue);
                    return;
                }
            }
            
            TryUpdateTargetTemperature(defaultTemp.GetValue);
        }
        
        private void UpdateTemperature()
        {
            _temperature = Mathf.Lerp(_temperature, _targetTemperature, Time.deltaTime * lerpSpeed);
            UpdateTemperatureDisplay();
        }

        private void UpdateTemperatureDisplay()
        {
            if (text != null)
                text.SetText($"{_temperature:F1} \u00b0C");
        }

        private void TryUpdateTargetTemperature((int min, int max) tempRange)
        {
            if (!_cooldown.IsReady) return;
            _targetTemperature = Random.Range(tempRange.min, tempRange.max);
            _cooldown.StartCooldown(cooldownTime);
        }
    }
}
using System;
using Objects.Clues;
using UnityEngine;
using Interfaces;
using Patterns;
using TMPro;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Items.Tools
{
    public class Thermometer : Tool
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private SO_ThermClue defaultTemp;
        [SerializeField] private LayerMask clueLayerMask;
        [SerializeField] private float cooldownTime;
        [SerializeField] private float lerpSpeed = 0.5f;
        
        private float _temperature = 25;
        private float _targetTemperature = 25;
        private readonly Cooldown _cooldown = new();

        private void OnEnable()
        {
            UpdateTemperatureDisplay();
        }

        public override void Use()
        {
            
        }

        private void Update()
        {
            CheckTemperature();
        }

        private void CheckTemperature()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 1, clueLayerMask);
            foreach (var hit in hits)
            {
                Clue clue = hit.GetComponent<Clue>();
                if (clue)
                {
                    SO_Clue type = clue.GetClueProvided;
                    if (type is SO_ThermClue tempClue)
                    {
                        UpdateTargetTemperature(tempClue.GetValue);
                        UpdateTemperature();
                        return;
                    }
                }
            }
            
            UpdateTargetTemperature(defaultTemp.GetValue);
            UpdateTemperature();
        }

        private void UpdateTemperature()
        {
            _temperature = Mathf.Lerp(_temperature, _targetTemperature, Time.deltaTime * lerpSpeed);
            UpdateTemperatureDisplay();
        }

        private void UpdateTemperatureDisplay()
        {
            text.SetText(_temperature.ToString("F1") + " \u00b0C");
        }

        private void UpdateTargetTemperature((int, int) temp)
        {
            if (_targetTemperature >= temp.Item1 && _targetTemperature <= temp.Item2)
            {
                if (_cooldown.IsReady)
                {
                    NewTargetTemperature(temp);
                }
                return;
            }
            NewTargetTemperature(temp);
        }

        private void NewTargetTemperature((int, int) temp)
        {
            _targetTemperature = Random.Range((float)temp.Item1, temp.Item2);
            _cooldown.StartCooldown(cooldownTime);
        }
    }
}

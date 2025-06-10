using System;
using System.Collections.Generic;
using Items.Base;
using Managers;
using Objects.Clues;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items.Tools
{
    public class GeigerCounter : Tool
    {
        [Header("Settings")] [SerializeField] private LayerMask clueLayerMask;
        [SerializeField] private float cooldownTime = 4f;
        [SerializeField] private float lerpSpeed = 5f;
        [SerializeField] private float detectionRadius = 1f;

        [Header("Needle Reference")] [SerializeField]
        private Transform needleTransform;

        [Header("Needle Rotation")] [SerializeField]
        private float minAngle = -90f; // Aguja a la izquierda

        [SerializeField] private float maxAngle = 90f; // Aguja a la derecha

        private float _radiation = 0f;
        private float _displayedRadiation = 0f; // Para interpolación visual
        //private Cooldown _cooldown = new();


        private GeigerMode _currentMode = GeigerMode.Off;

        public override void Use(UseType useType)
        {
            if (useType == UseType.Default)
            {
                _currentMode = (GeigerMode)(((int)_currentMode + 1) % System.Enum.GetValues(typeof(GeigerMode)).Length);
            }
        }

        private void Update()
        {
            UpdateClueMode();
            UpdateNeedle();
        }

        private void UpdateClueMode()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 1, clueLayerMask);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out Clue clue) && clue.GetClueProvided is SO_RadiationClue radClue)
                {
                    SetRadiation(radClue.GetValue);
                    return;
                }
            }

            SetRadiation((0, 0));
        }

        private void SetRadiation((int min, int max) tempRange)
        {
            _radiation = Random.Range((float)tempRange.min, tempRange.max);
            //_cooldown.StartCooldown(cooldownTime);
        }

        private void UpdateNeedle()
        {
            if (_currentMode == GeigerMode.Off)
            {
                _displayedRadiation = Mathf.Lerp(_displayedRadiation, 0f, Time.deltaTime * lerpSpeed);
                RotateNeedle(0f);
                return;
            }

            float maxRange = GetMaxRadiationRange(_currentMode);
            _displayedRadiation = Mathf.Lerp(_displayedRadiation, _radiation, Time.deltaTime * lerpSpeed);

            float t = Mathf.Clamp01(_displayedRadiation / maxRange);
            float targetAngle = Mathf.Lerp(minAngle, maxAngle, t);

            RotateNeedle(targetAngle);
        }

        private void RotateNeedle(float angle)
        {
            needleTransform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }

        private float GetMaxRadiationRange(GeigerMode mode)
        {
            return mode switch
            {
                GeigerMode.To1 => 1f,
                GeigerMode.To10 => 10f,
                GeigerMode.To100 => 100f,
                _ => 1f
            };
        }
    }

    public enum GeigerMode
    {
        Off,
        To1,
        To10,
        To100
    }
}
using System;
using System.Collections.Generic;
using Items.Base;
using UnityEngine;
using Objects.Clues;
using Managers;

namespace Items.Tools
{
    public class Compass : Tool
    {
        [Header("References")]
        [SerializeField] private Transform needle;
        [SerializeField] private Transform targetDirection;
        [SerializeField] private LayerMask clueLayerMask;

        [Header("General Parameters")]
        [SerializeField] private float rotationSpeed = 2.5f;
        [SerializeField] private Vector3 standardDirection = Vector3.forward;

        [Header("Special Modes")]
        [SerializeField] private float spinSpeed = 360f;
        [SerializeField] private float shakeFrequency = 12f;
        [SerializeField] private float shakeSpeed = 100f;
        [SerializeField] private float shakeAngle = 20f;
        [SerializeField] private float randomInterval = 3f;

        private Vector3 _direction;
        private Cooldown _randomCooldown = new();
        private CompassMode _currentMode = CompassMode.Default;
        private Action _currentRotationBehavior;
        private readonly Dictionary<CompassMode, Action> _modeBehaviors = new();

        private void Start()
        {
            _direction = standardDirection;

            _modeBehaviors[CompassMode.Default] = DefaultRotation;
            _modeBehaviors[CompassMode.Flip] = FlipRotation;
            _modeBehaviors[CompassMode.Spin] = SpinRotation;
            _modeBehaviors[CompassMode.Shake] = ShakeRotation;
            _modeBehaviors[CompassMode.Random] = RandomRotation;

            SetRotationBehavior(CompassMode.Default);
        }
        
        public override void Use(UseType useType)
        {
            
        }

        private void Update()
        {
            UpdateClueMode();
            _currentRotationBehavior?.Invoke();
        }

        private void UpdateClueMode()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 1, clueLayerMask);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out Clue clue) && clue.GetClueProvided is SO_CompassClue compassClue)
                {
                    if (_currentMode != compassClue.GetValue)
                    {
                        SetRotationBehavior(compassClue.GetValue);
                    }
                    return;
                }
            }

            if (_currentMode != CompassMode.Default)
                SetRotationBehavior(CompassMode.Default);
        }

        private void SetRotationBehavior(CompassMode newMode)
        {
            _currentMode = newMode;
            _currentRotationBehavior = _modeBehaviors.TryGetValue(newMode, out var action) ? action : DefaultRotation;

            if (newMode != CompassMode.Random)
                _direction = standardDirection;
        }

        private void DefaultRotation()
        {
            RotateNeedle(GetDirection(), rotationSpeed);
        }

        private void FlipRotation()
        {
            RotateNeedle(-GetDirection(), rotationSpeed);
        }

        private void SpinRotation()
        {
            needle.localRotation *= Quaternion.Euler(0f, spinSpeed * Time.deltaTime, 0f);
        }

        private void ShakeRotation()
        {
            float noise = Mathf.PerlinNoise(Time.time * shakeFrequency, 0f) * 2f - 1f;
            var jitterRotation = Quaternion.Euler(0f, noise * shakeAngle, 0f);
            var jitteredDirection = jitterRotation * GetDirection();
            RotateNeedle(jitteredDirection, shakeSpeed);
        }

        private void RandomRotation()
        {
            if (_randomCooldown.IsReady)
            {
                _direction = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f) * Vector3.forward;
                _randomCooldown.StartCooldown(randomInterval);
            }
            RotateNeedle(_direction, rotationSpeed);
        }

        private void RotateNeedle(Vector3 direction, float speed)
        {
            if (direction.sqrMagnitude > 0.001f)
            {
                Vector3 localDir = needle.parent.InverseTransformDirection(direction.normalized);
                Quaternion targetRot = Quaternion.LookRotation(localDir, Vector3.up);
                targetRot = Quaternion.Euler(0f, targetRot.eulerAngles.y, 0f);
                needle.localRotation = Quaternion.Slerp(needle.localRotation, targetRot, Time.deltaTime * speed);
            }
        }

        private Vector3 GetDirection()
        {
            Vector3 dir = targetDirection ? targetDirection.position - transform.position : _direction;
            dir.y = 0f;
            return dir;
        }
    }
}
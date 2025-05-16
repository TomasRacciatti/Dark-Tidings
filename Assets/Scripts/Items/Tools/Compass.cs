using UnityEngine;
using Objects.Clues;
using Interfaces;

namespace Items.Tools
{
    public class Compass : Tool
    {
        public CompassMode mode = CompassMode.Normal;

        public LayerMask clueLayerMask;

        [SerializeField] private Transform baseTransform;
        [SerializeField] private Transform compassNeedle;

        [Header("Normal Mode")] [SerializeField]
        private float normalRotationSpeed = 2f;

        [SerializeField] private float normalJitterStrength = 1f;
        [SerializeField] private float normalJitterSpeed = 1f;

        [Header("Intense Shake Mode")] [SerializeField]
        private float intenseJitterStrength = 5f;

        [SerializeField] private float intenseJitterSpeed = 10f;

        [Header("Crazy Spin Mode")] [SerializeField]
        private float spinSpeed = 360f; // degrees per second

        
        private float jitterTimer;

        public override void Use()
        {
            
        }

        void Update()
        {
            bool foundClue = false;
            Collider[] hits = Physics.OverlapSphere(transform.position,1, clueLayerMask);
            foreach (var hit in hits)
            {
                Clue clue = hit.GetComponent<Clue>();
                if (clue != null)
                {
                    SO_Clue type = clue.GetClueProvided;
                    if (type is SO_CompassClue compassClue)
                    {
                        mode = compassClue.GetValue;
                        break;
                    }
                }
            }
            if (!foundClue)
            {
                mode = CompassMode.Normal;
            }
            
            switch (mode)
            {
                case CompassMode.Normal:
                    UpdateNormal();
                    break;
                case CompassMode.Shake:
                    UpdateShake(intenseJitterStrength, intenseJitterSpeed);
                    break;
                case CompassMode.Spin:
                    UpdateSpin();
                    break;
            }
        }

        private void UpdateNormal()
        {
            Vector3 localNorth = GetFlatNorth();
            if (localNorth.sqrMagnitude < 0.001f) return;

            jitterTimer += Time.deltaTime * normalJitterSpeed;
            Vector3 jitter = GetJitter(normalJitterStrength);
            Vector3 jitteredDirection = (localNorth + jitter).normalized;

            RotateNeedle(jitteredDirection, normalRotationSpeed);
        }

        private void UpdateShake(float strength, float speed)
        {
            Vector3 localNorth = GetFlatNorth();
            if (localNorth.sqrMagnitude < 0.001f) return;

            jitterTimer += Time.deltaTime * speed;
            Vector3 jitter = GetJitter(strength);
            Vector3 jitteredDirection = (localNorth + jitter).normalized;

            RotateNeedle(jitteredDirection, normalRotationSpeed * 2.5f);
        }

        private void UpdateSpin()
        {
            compassNeedle.localRotation *= Quaternion.Euler(0f, spinSpeed * Time.deltaTime, 0f);
        }

        private Vector3 GetFlatNorth()
        {
            Vector3 worldNorth = new Vector3(0f, 0f, 1f);
            Vector3 projectedNorth = Vector3.ProjectOnPlane(worldNorth, baseTransform.up);
            Vector3 localNorth = baseTransform.InverseTransformDirection(projectedNorth.normalized);
            return new Vector3(localNorth.x, 0f, localNorth.z).normalized;
        }

        private Vector3 GetJitter(float strength)
        {
            return new Vector3(
                (Mathf.PerlinNoise(jitterTimer, 0f) - 0.5f),
                0f,
                (Mathf.PerlinNoise(0f, jitterTimer) - 0.5f)
            ) * strength;
        }

        private void RotateNeedle(Vector3 direction, float speed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            compassNeedle.localRotation =
                Quaternion.Slerp(compassNeedle.localRotation, targetRotation, Time.deltaTime * speed);
        }
    }
}
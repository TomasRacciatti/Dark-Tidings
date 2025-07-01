using System;
using System.Collections.Generic;
using Characters.Player;
using Inventory.Model;
using Items.Base;
using Managers;
using Managers.ObjectPool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Items.Weapons
{
    public class Crossbow : ItemEquippable
    {
        [Header("Setup")]
        [SerializeField] private GameObject _boltPrefab;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private SO_Item _bolt;
        [SerializeField] private GameObject _boltObject;

        [Header("Animators")]
        [SerializeField] private Animator handsAnimator;
        [SerializeField] private Animator crossbowAnimator;
        [SerializeField] private Animator boltAnimator; // Usar si se necesita

        [Header("Aiming")]
        [SerializeField] private float recoilStrength = 0.2f;
        [SerializeField] private float aimAccuracySpeed = 0.4f;
        [SerializeField] private float maxAccuracy = 1f;
        [SerializeField] private float minAccuracy = 0.5f;
        [SerializeField] private float spreadAngle = 15f;
        [SerializeField] private float minSpread = 150f;
        [SerializeField] private float maxSpread = 4f;
        
        private float _accuracy = 0.5f;
        private bool _isAiming = false;
        private bool _isCharged = false;
        private bool _isReloading = false;
        private float _timeReload = 1.5f;

        private ItemAmount _boltType = new(null, 0);
        private InventorySystem _boltInventorySystem;

        private void Start()
        {
            _boltInventorySystem = GameManager.Canvas.inventoryManager.boltsInventorySystem;
            if (_boltType.IsEmpty)
            {
                _boltObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            GameManager.Canvas.crosshairCrossbow.gameObject.SetActive(true);
            Animate("Reload1");
        }

        private void OnDisable()
        {
            StopAiming();
            if (GameManager.Canvas == null) return;
            GameManager.Canvas.crosshairCrossbow.gameObject.SetActive(false);
        }
        
        private void Update()
        {
            float targetAccuracy = _isAiming ? maxAccuracy : minAccuracy;
            _accuracy = Mathf.MoveTowards(_accuracy, targetAccuracy, Time.deltaTime * aimAccuracySpeed);
            float spread = Mathf.Lerp(minSpread, maxSpread, Mathf.InverseLerp(minAccuracy, maxAccuracy, _accuracy));
            GameManager.Canvas.crosshairCrossbow.sizeDelta = new Vector2(spread, spread);
        }

        private void Animate(string anim)
        {
            handsAnimator.SetTrigger(anim);
            crossbowAnimator.SetTrigger(anim);
            boltAnimator.SetTrigger(anim);
        }

        public override void Use(UseType useType)
        {
            switch (useType)
            {
                case UseType.Default:
                    Fire();
                    break;
                case UseType.Aim:
                    StartAiming();
                    break;
                case UseType.StopAim:
                    StopAiming();
                    break;
                case UseType.Reload1:
                    Reload1();
                    break;
                case UseType.Reload2:
                    if (!_isCharged) return;
                    if (!_boltType.IsEmpty) return;
                    if (Reload2())
                    {
                        Animate("Reload2");
                    }
                    break;
                case UseType.Reload3:
                    if (!_isCharged) return;
                    bool hasBolt = false;
                    if (!_boltType.IsEmpty)
                    {
                        hasBolt = true;
                        _boltInventorySystem.AddItem(ref _boltType);
                    }
                    if (!_boltType.IsEmpty) GameManager.Player.AddItem(ref _boltType);
                    _boltType = new ItemAmount();
                    _boltInventorySystem.TransferIndexToIndex(_boltInventorySystem, 0, 1);
                    if (Reload2())
                    {
                        Animate(hasBolt ? "Change" : "Reload2");
                        if (hasBolt)
                        {
                            _boltObject.SetActive(true);
                        }
                    }
                    break;
            }
        }
        
        private void Reload1()
        {
            if (!_boltType.IsEmpty) return;
            if (!_boltInventorySystem.HasItem(_bolt)) return;
            Animate("Reload1");
            _boltObject.SetActive(true);
            _isCharged = true;
            _isReloading = true;
        }
        
        private bool Reload2()
        {
            ItemAmount bolt = _boltInventorySystem.GetFirstSoItem(_bolt);
            if (bolt.IsEmpty) return false;

            // Creamos una copia con cantidad 1 para removerla
            _boltType = new ItemAmount(bolt);
            _boltType.SetAmount(1);
            
            ItemAmount boltToRemove = new ItemAmount(bolt.SoItem, 1, bolt.Modifiers);
            _boltInventorySystem.RemoveItem(ref boltToRemove);
            _isReloading = true;
            Invoke(nameof(FinishReloading), _timeReload);
            return true;
        }

        private void FinishReloading()
        {
            _isReloading = false;
        }
        
        private void Fire()
        {
            //if (!_isAiming) return;
            if (_isReloading) return;
            if (_boltType.IsEmpty)
                return;

            // Dirección de la cámara
            Transform cameraTransform = GameManager.Player.GetComponent<PlayerController>().mainCamera.transform;
            Vector3 rayOrigin = cameraTransform.position;
            Vector3 rayDirection = cameraTransform.forward;

            // Calcular punto objetivo
            Vector3 targetPoint;
            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, 1000f))
            {
                targetPoint = hit.point;
            }
            else
            {
                // Si no golpea nada, usar dirección a distancia máxima
                targetPoint = rayOrigin + rayDirection * 1000f;
            }

            // Calcular dirección desde el punto de disparo
            Vector3 direction = (targetPoint - _firePoint.position).normalized;

            // Agregar desviación según precisión
            float currentSpread = (1f - _accuracy) * spreadAngle;
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * currentSpread;

            Quaternion spreadRotation = Quaternion.Euler(randomOffset.x, randomOffset.y, 0f);
            Vector3 spreadDirection = spreadRotation * direction;

            Quaternion finalRotation = Quaternion.LookRotation(spreadDirection);

            // Instanciar y configurar bolt
            CameraShake1.Instance.Shake(transform.right, recoilStrength);

            GameObject boltInstance = ObjectPoolManager.Instance.SpawnObject(_boltPrefab, _firePoint.position, finalRotation, 10f);
            boltInstance.GetComponent<Bolt>().SetModifiers(_boltType.Modifiers);
            Animate("Fire");
            _boltObject.SetActive(false);

            _boltType = new ItemAmount();
            _isCharged = false;
        }
        
        private void StartAiming()
        {
            _isAiming = true;
        }

        private void StopAiming()
        {
            _isAiming = false;
        }
    }
}
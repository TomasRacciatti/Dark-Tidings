using System;
using System.Collections.Generic;
using Characters.Player;
using Inventory.Model;
using Items.Base;
using Managers;
using Managers.ObjectPool;
using UnityEngine;

namespace Items.Weapons
{
    public class Crossbow : ItemEquippable
    {
        [SerializeField] private GameObject _boltPrefab;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private SO_Item _bolt;
        [SerializeField] private float recoilStrength = 0.2f;
        private bool _isAiming = false;

        private ItemAmount _boltType = new();
        private InventorySystem _inventorySystem;
        private InventorySystem _boltInventorySystem;
        
        [SerializeField] private float aimAccuracySpeed = 2f; // velocidad de aumento
        [SerializeField] private float maxAccuracy = 1f;      // 100% de precisión
        [SerializeField] private float minAccuracy = 0.5f;    // sin apuntar
        [SerializeField] private float spreadAngle = 5f;      // desvío máximo

        private float _accuracy = 0.5f;

        private void Start()
        {
            _inventorySystem = GameManager.Player.inventory;
            _boltInventorySystem = GameManager.Canvas.inventoryManager.boltsInventorySystem;
        }

        private void OnEnable()
        {
            GameManager.Canvas.crosshairUI.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            StopAiming();
            if (GameManager.Canvas == null) return;
            GameManager.Canvas.crosshairUI.gameObject.SetActive(false);
        }
        
        private void Update()
        {
            float targetAccuracy = _isAiming ? maxAccuracy : minAccuracy;
            _accuracy = Mathf.MoveTowards(_accuracy, targetAccuracy, Time.deltaTime * aimAccuracySpeed);
            GameManager.Canvas.crosshairUI.sizeDelta = new Vector2(50 / _accuracy, 50 / _accuracy);
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
                    if (!_boltType.IsEmpty) return;
                    Reload2();
                    break;
                case UseType.Reload3:
                    if (!_boltType.IsEmpty) _boltInventorySystem.AddItem(ref _boltType);
                    if (!_boltType.IsEmpty) GameManager.Player.AddItem(ref _boltType);
                    _boltType = new ItemAmount();
                    _boltInventorySystem.TransferIndexToIndex(_boltInventorySystem, 0, 1);
                    Reload2();
                    break;
            }
        }
        
        private void Reload1()
        {
            if (!_boltType.IsEmpty) return;
            Debug.Log("Reload animation started:");
        }
        
        private void Reload2()
        {
            ItemAmount bolt = _boltInventorySystem.GetFirstSoItem(_bolt);
            if (bolt.IsEmpty) return;

            // Creamos una copia con cantidad 1 para removerla
            _boltType = new ItemAmount(bolt);
            _boltType.SetAmount(1);
            
            ItemAmount boltToRemove = new ItemAmount(bolt.SoItem, 1, bolt.Modifiers);
            _boltInventorySystem.RemoveItem(ref boltToRemove);
        }
        
        private void Fire()
        {
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

            _boltType = new ItemAmount();
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
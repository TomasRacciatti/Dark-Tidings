using System;
using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon")] [SerializeField] public string weaponName = "Weapon";
    [SerializeField] protected int _damage = 10;
    [SerializeField] protected float _firerate = 6f; //bullets per second
    [SerializeField] private int _magazineSize = 13;
    [SerializeField] protected float _reloadTime = 2.5f;
    [SerializeField] private bool _automatic = false;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _bulletPrefab;

    private int _bullets;
    private float _timeBetweenShots;
    private bool _isReloading, _isShooting;
    private Cooldown _cooldown = new Cooldown();

    private Coroutine shootingCoroutine;

    public event Action OnShoot;
    public event Action OnReload;

    private void Start()
    {
        if (_firePoint == null) Debug.LogError($"{weaponName}: FirePoint no asignado.");
        if (_bulletPrefab == null) Debug.LogError($"{weaponName}: BulletPrefab no asignado.");

        _timeBetweenShots = 1f / _firerate;
        _bullets = _magazineSize;
        StartShooting();//sacar es solo prueba
    }

    public void StartShooting()
    {
        if (_isShooting || shootingCoroutine != null) return;
        _isShooting = true;
        if (_isReloading) return;
        shootingCoroutine = StartCoroutine(ShootCoroutine());
    }

    public void StopShooting()
    {
        _isShooting = false;
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    private IEnumerator ShootCoroutine()
    {
        while (_isShooting && !_isReloading)
        {
            if (!_cooldown.IsReady)
            {
                yield return null;
                continue;
            }
            
            Shoot();
            _bullets--;
            _cooldown.StartCooldown(_timeBetweenShots);
            
            if (_bullets <= 0)
            {
                StartCoroutine(Reload());
                yield break;
            }

            yield return new WaitUntil(() => _cooldown.IsReady);
        }
    }

    protected virtual void Shoot()
    {
        OnShoot?.Invoke(); //effects, anims
    }

    public IEnumerator Reload()
    {
        if (_isReloading) yield break;
        _isReloading = true;
        OnReload?.Invoke(); //effects, anims
        yield return new WaitForSeconds(_reloadTime);
        _bullets = _magazineSize;
        _isReloading = false;
        if (_automatic && _isShooting)
        {
            if (shootingCoroutine != null) StopCoroutine(shootingCoroutine);
            shootingCoroutine = StartCoroutine(ShootCoroutine());
        }
    }

    protected void CreateRay()
    {
        //crear rays con el prefab
        Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
    }
}
using System;
using System.Collections;
using Interfaces;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon")] [SerializeField] public string weaponName = "Weapon";
    [SerializeField] protected int _damage = 10;
    [SerializeField] protected float _firerate = 6f; //bullets per second
    [SerializeField] private int _magazineSize = 13;
    [SerializeField] protected float _reloadTime = 2.5f;
    [SerializeField] private bool _automatic = false;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] private LayerMask _layerMask;

    //recoil tipo circular y para arriba
    [SerializeField] private float _recoveryTime = 0.25f;
    [SerializeField] private float _baseInaccuracy = 0f;
    [SerializeField] private float _incrementalInaccuracy = 10f;
    [SerializeField] private float _maxInaccuracy = 100f;

    [SerializeField] private ParticleSystem _shootingParticle;
    [SerializeField] private ParticleSystem _impactParticle;
    [SerializeField] private GameObject _trailRenderer;
    [SerializeField] private float _trailSpeed = 100f;
    [SerializeField] private AudioClip _fireSound;
    [SerializeField] private AudioClip _reloadSound;
    [SerializeField] private AudioClip _impactSound;

    private Animator _animator;
    private AudioSource _audioSource;

    private int _bullets;
    private float _timeBetweenShots;
    private bool _isReloading, _isShooting;
    private float _currentInaccuracy = 0f;
    private Cooldown _cooldown = new Cooldown();

    private Coroutine shootingCoroutine;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _timeBetweenShots = 1f / _firerate;
        _bullets = _magazineSize;
        StartShooting(); //sacar esto y que es automatica es solo prueba
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

            if (_bullets <= 0)
            {
                StartCoroutine(Reload());
                yield break;
            }
            
            Shoot();
            _bullets--;
            PlayShootEffects();
            _cooldown.StartCooldown(_timeBetweenShots);
            
            yield return new WaitUntil(() => _cooldown.IsReady);
        }
    }

    protected virtual void Shoot()
    {
        //anims y sonido
    }

    public IEnumerator Reload()
    {
        if (_isReloading) yield break;
        _isReloading = true;
        PlayReloadEffects();
        yield return new WaitForSeconds(_reloadTime);
        _bullets = _magazineSize;
        _isReloading = false;
        //anims y sonido
        if (_automatic && _isShooting)
        {
            if (shootingCoroutine != null) StopCoroutine(shootingCoroutine);
            shootingCoroutine = StartCoroutine(ShootCoroutine());
        }
    }

    protected void CreateRay(Vector3 direction)
    {
        GameObject trail = ObjectPoolManager.instance.SpawnObject(_trailRenderer, _firePoint.position, Quaternion.LookRotation(direction));
        if (trail.TryGetComponent<BulletRay>(out var bulletRay))
        {
            bulletRay.speed = _trailSpeed;
        }
        
        Vector3 hitPoint;
        if (Physics.Raycast(_firePoint.position, direction, out RaycastHit hit, 1000, _layerMask))
        {
            hitPoint = hit.point;
            //Instantiate(_impactParticle, hitPoint, Quaternion.LookRotation(hit.normal));
            if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(_damage);
            }
        }
        else
        {
            hitPoint = _firePoint.position + direction * 1000;
        }
        
        ObjectPoolManager.instance.ReturnObjectToPool(trail,Vector3.Distance(_firePoint.position, hitPoint)/_trailSpeed);
    }
    
    private void PlayShootEffects()
    {
        if (_fireSound) _audioSource.PlayOneShot(_fireSound);
        if (_shootingParticle) _shootingParticle.Play();
    }

    private void PlayReloadEffects()
    {
        if (_reloadSound) _audioSource.PlayOneShot(_reloadSound);
        _animator?.SetTrigger("Reload");
    }
}
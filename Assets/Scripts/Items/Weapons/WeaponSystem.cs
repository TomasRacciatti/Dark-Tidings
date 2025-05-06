
using Characters.Player;
using UnityEngine;
using Interfaces;

public class WeaponSystem : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private float _basePrecisionAngle = 5f; // Base cone angle in degrees
    [SerializeField] private float _maxRange = 100f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private Transform _shootPoint;
    
    [Header("Debug")]
    [SerializeField] private bool _showShootRay = true;
    [SerializeField] private float _rayDuration = 1f;
    
    private InputsEvents _inputEvents;
    private PlayerBackpackHandler _backpackHandler;
    private Backpack _backpack;
    private float _currentPrecisionAngle;
    
    private void Awake()
    {
        _inputEvents = GetComponent<InputsEvents>();
        _backpackHandler = GetComponent<PlayerBackpackHandler>();
        _backpack = GetComponentInChildren<Backpack>();
        
        _currentPrecisionAngle = _basePrecisionAngle;
    }
    
    private void Update()
    {
        // Get current precision angle based on whether the backpack is equipped
        UpdatePrecisionAngle();
        
        // Handle shooting input
        if (_inputEvents != null) // && _inputEvents.use
        {
            Shoot();
        }
    }
    
    private void UpdatePrecisionAngle()
    {
        if (_backpack != null && _backpack.IsEquipped())
        {
            BackpackStats backpackStats = _backpack.GetBackpackStats();
            if (backpackStats != null)
            {
                _currentPrecisionAngle = _basePrecisionAngle + backpackStats.AccuracyReductionDegrees;
            }
        }
        else
        {
            _currentPrecisionAngle = _basePrecisionAngle;
        }
    }
    
    private void Shoot()
    {
        if (_shootPoint == null)
        {
            // Use camera forward as fallback
            _shootPoint = Camera.main.transform;
        }
        
        // Calculate random direction within the precision cone
        Vector3 shootDirection = GetRandomDirectionWithinCone(_shootPoint.forward, _currentPrecisionAngle);
        
        RaycastHit hit;
        if (Physics.Raycast(_shootPoint.position, shootDirection, out hit, _maxRange))
        {
            // If we hit something with the IDamageable interface
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(_damage);
                Debug.Log($"Hit {hit.collider.gameObject.name} for {_damage} damage");
            }
            
            // Debug visualization
            if (_showShootRay)
            {
                Debug.DrawLine(_shootPoint.position, hit.point, Color.red, _rayDuration);
            }
        }
        else if (_showShootRay)
        {
            // Debug visualization for miss
            Debug.DrawRay(_shootPoint.position, shootDirection * _maxRange, Color.yellow, _rayDuration);
        }
    }
    
    private Vector3 GetRandomDirectionWithinCone(Vector3 forward, float angleDegrees)
    {
        // Get random direction within a cone
        float angle = Random.Range(0f, angleDegrees);
        float angleRadians = angle * Mathf.Deg2Rad;
        
        // Get random direction around the cone
        float randomAngleAround = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        
        // Calculate the direction vector
        Vector3 randomDirection = forward;
        
        // Apply the random cone deviation
        randomDirection = Quaternion.AngleAxis(angle, new Vector3(Mathf.Cos(randomAngleAround), Mathf.Sin(randomAngleAround), 0f)) * randomDirection;
        
        return randomDirection.normalized;
    }
    
    public float GetCurrentPrecisionAngle()
    {
        return _currentPrecisionAngle;
    }
    
    // Optional: Add a method to visualize the accuracy cone
    private void OnDrawGizmos()
    {
        if (_shootPoint == null) return;
        
        Gizmos.color = Color.cyan;
        
        // Draw the forward direction
        Gizmos.DrawRay(_shootPoint.position, _shootPoint.forward * 2f);
        
        // Draw the cone boundaries (simplified)
        Vector3 upBoundary = Quaternion.AngleAxis(_currentPrecisionAngle, _shootPoint.right) * _shootPoint.forward;
        Vector3 downBoundary = Quaternion.AngleAxis(-_currentPrecisionAngle, _shootPoint.right) * _shootPoint.forward;
        Vector3 leftBoundary = Quaternion.AngleAxis(-_currentPrecisionAngle, _shootPoint.up) * _shootPoint.forward;
        Vector3 rightBoundary = Quaternion.AngleAxis(_currentPrecisionAngle, _shootPoint.up) * _shootPoint.forward;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_shootPoint.position, upBoundary * 2f);
        Gizmos.DrawRay(_shootPoint.position, downBoundary * 2f);
        Gizmos.DrawRay(_shootPoint.position, leftBoundary * 2f);
        Gizmos.DrawRay(_shootPoint.position, rightBoundary * 2f);
    }
}
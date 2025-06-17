using Cinemachine;
using UnityEngine;

public class CameraShake1 : MonoBehaviour
{
    public static CameraShake1 Instance;
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        Instance = this;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(Vector3 dir, float strength)
    {
        _impulseSource.GenerateImpulseWithVelocity(dir.normalized * strength);
    }
}
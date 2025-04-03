using UnityEngine;

public class Cooldown
{
    private float _nextTime = 0;
    public bool IsReady => Time.time >= _nextTime;
    public void StartCooldown(float cooldownTime) => _nextTime = Time.time + cooldownTime;
    public void ResetCooldown() => _nextTime = Time.time;
    public float RemainingTime => Mathf.Max(0, _nextTime - Time.time);
    public void DeltaTime(float delta) => _nextTime = _nextTime + delta;
}
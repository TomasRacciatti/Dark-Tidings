using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items.Tools;
using UnityEngine;
using UnityEngine.Serialization;
using Compass = Items.Tools.Compass;


[System.Serializable]
public struct ToolCondition
{
    public enum Kind { LessThan, GreaterThan, MatchValue }
    public Kind kind;

    [Header("Numeric settings")]
    public float numericThreshold;

    [Header("Discrete settings (for compass)")]
    public CompassMode compassBehavior;
}

public class ToolThresholdTrigger : HorrorEventTrigger
{
    [Tooltip("Which tool(s) to watch")]
    [SerializeField] private Thermometer thermometer;
    [SerializeField] private Compass compass;

    [Tooltip("Check these conditions every X seconds")]
    [SerializeField] private float checkInterval = 0.5f;

    [Tooltip("Any of these going true will Fire()")]
    [SerializeField] private List<ToolCondition> conditions = new List<ToolCondition>();

    private void OnEnable()
    {
        if (thermometer == null) thermometer = GetComponent<Thermometer>();
        if (compass== null) compass = GetComponent<Compass>();
        StartCoroutine(Checker());
    }

    private IEnumerator Checker()
    {
        while (true)
        {
            if (hasFired && fireOnce) yield break;
            
            bool shouldFire = conditions.Any(cond =>
            {
                switch (cond.kind)
                {
                    case ToolCondition.Kind.LessThan:
                    {
                        float temp = thermometer.Temperature;
                        return temp < cond.numericThreshold;
                    }
                    case ToolCondition.Kind.GreaterThan:
                    {
                        float temp = thermometer.Temperature;
                        return temp > cond.numericThreshold;
                    }
                    case ToolCondition.Kind.MatchValue:
                    {
                        if (compass!= null)
                        {
                            var behav = compass.Behavior;
                            return behav == cond.compassBehavior;
                        }
                        else
                            return false;

                        break;
                    }
                        
                    default:
                        return false;
                }
            });

            if (shouldFire)
                Fire();

            yield return new WaitForSeconds(checkInterval);
        }
    }
}

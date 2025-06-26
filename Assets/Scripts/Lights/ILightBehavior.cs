using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILightBehavior
{
    void Enter();              // es como un OnEnable del estado
    void Exit();               // es como un OnDisable del estado
    void UpdateBehavior();
}

using System;
using UnityEngine;

[Serializable]
public class DelayHomingParameters : BaseParameters
{

    [Header("Homing")]
    public float turnSpeed;
    public bool decreaseHomingSpeed;
    public float homingSpeedRate;
    public float delayTime;
}

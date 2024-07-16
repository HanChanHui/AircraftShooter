using System;
using UnityEngine;

[Serializable]
public class HomingParameters : BaseParameters
{
    [Header("Aiming")]
    public Transform targetTransform;

    [Header("Homing")]
    public float turnSpeed;
    public bool decreaseHomingSpeed;
    public float homingSpeedRate;
}

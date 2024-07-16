using System;
using UnityEngine;

[Serializable]
public class AimingParameters : BaseParameters
{
    [Header("Nway")]
    public float angleRange;
    public int count;

    [Header("Aiming")]
    public Transform targetTransform;
    public bool stopAttackCooltime;
    public float targetfixedAngle;
}

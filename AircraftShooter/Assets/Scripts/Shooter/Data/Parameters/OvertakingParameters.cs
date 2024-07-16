using System;
using UnityEngine;

[Serializable]
public class OvertakingParameters : BaseParameters
{
    [Header("Nway")]
    public float angleRange;
    public int count;

    [Header("Spreading")]
    public float groupSpeed;
    public float groupCount;

    [Header("Overtaking")]
    public float groupAngle;
    public float groupInterval;
}

using System;
using UnityEngine;

[Serializable]
public class RollingNwayParameters : BaseParameters
{
    [Header("Forward")]
    public float forwardAngleSpeed;

    [Header("Nway")]
    public float angleRange;
    public int count;

    [Header("Rolling N-way")]
    public int nWayCount;
}

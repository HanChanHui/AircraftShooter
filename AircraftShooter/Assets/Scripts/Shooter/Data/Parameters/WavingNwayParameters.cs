using System;
using UnityEngine;

[Serializable]
public class WavingNwayParameters : BaseParameters
{
    [Header("Forward")]
    public float forwardAngleSpeed;

    [Header("Nway")]
    public float angleRange;
    public int count;

    [Header("Waving N-way")]
    public float wavingAngleRange;
    public int cycle;
}

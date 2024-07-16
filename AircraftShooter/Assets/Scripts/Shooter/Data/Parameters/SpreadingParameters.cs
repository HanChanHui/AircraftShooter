using System;
using UnityEngine;

[Serializable]
public class SpreadingParameters : BaseParameters
{
    [Header("Nway")]
    public float angleRange;
    public int count;

    [Header("Spreading")]
    public float groupSpeed;
    public float groupCount;
}

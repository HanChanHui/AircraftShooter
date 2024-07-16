using System;
using UnityEngine;

[Serializable]
public class RandomSpreadingParameters : BaseParameters
{
    [Header("Nway")]
    public float angleRange;
    public int count;

    [Header("Spreading")]
    public float speedRange;
}

using System;
using UnityEngine;

[Serializable]
public class ArcParameters : BaseParameters
{
    [Header("Aiming")]
    public Transform targetTransform;

    [Header("Arc")]
    public float arrivalTime;
    public float height;
}

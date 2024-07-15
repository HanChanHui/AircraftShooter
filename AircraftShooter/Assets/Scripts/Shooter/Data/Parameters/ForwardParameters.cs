using System;
using UnityEngine;

[Serializable]
public class ForwardParameters : BaseParameters
{
    [Header("Forward")]
    public float forwardAngleSpeed;

    [Header("Nway")]
    public float angleRange;
    public int count;
}

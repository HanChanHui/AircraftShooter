using System;
using UnityEngine;

[Serializable]
public class PlacedParameters : BaseParameters
{
   
    [Header("Placed")]
    public float moveTime;
    public float stopTime;
    public float placedStopSpeed;
}

using System;
using UnityEngine;

[Serializable]
public class CustomShapeParameters : BaseParameters
{
    [Header("Custom Shape")]
    public float rot;
    public int vertex;
    public float sup;
    public bool rotateBulletCore;
    public Vector3 rotateBulletOffset;
}

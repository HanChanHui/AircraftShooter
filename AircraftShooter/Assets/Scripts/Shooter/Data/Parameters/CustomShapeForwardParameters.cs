using System;
using UnityEngine;

[Serializable]
public class CustomShapeForwardParameters : BaseParameters
{
    [Header("CustomShapeForward")]
    public Consts.ShapeType shapeType;
    public int vertexShape;
    public float radius;
    public float angleSpeed;
    public int segments;
    public int circleAngle;
}

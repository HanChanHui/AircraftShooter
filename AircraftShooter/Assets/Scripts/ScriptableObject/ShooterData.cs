using System.Collections.Generic;
using UnityEngine;


public class ShootingData
{
    [Header("Basic")]
    public Shooter.ShootingType shootingType;

    [Header("Bullet")]
    public Consts.BulletType bulletType;
    public int power;
    public float speed;
    public float speedRate;
    public float angle;
    public float angleRate;
    public float startDistance;
    public float lifeTime;

    [Header("Custom Shape")]
    public float rot;
    public int vertex;
    public float sup;
    public bool rotateBulletCore;
    public Vector3 rotateBulletOffset;

    [Header("Forward")]
    public float forwardAngleSpeed;

    [Header("Nway")]
    public float angleRange;
    public int count;

    [Header("Multiple")]
    public float multipleStep;
    public float multipleRange;

    [Header("Homing")]
    public float turnSpeed;
    public bool decreaseHomingSpeed;
    public float homingSpeedRate;
    public float delayTime;

    [Header("Rolling N-way")]
    public int nWayCount;

    [Header("Waving N-way")]
    public float wavingAngleRange;
    public int cycle;

    [Header("Placed")]
    public float moveTime;
    public float stopTime;
    public float placedStopSpeed;

    [Header("Aiming")]
    public Transform targetTransform;
    public bool stopAttackCooltime;
    public float targetfixedAngle;

    [Header("Spreading")]
    public float groupSpeed;
    public float groupCount;
    public float speedRange;

    [Header("Overtaking")]
    public float groupAngle;
    public float groupInterval;

    [Header("Arc")]
    public float arrivalTime;
    public float height;

    [Header("CustomShapeForward")]
    public Consts.ShapeType shapeType;
    public int vertexShape;
    public float radius;
    public float angleSpeed;
    public int segments;
    public int circleAngle;

    [Header("AttackCoolTime")]
    public float attackCooltime = 0f;
    public float attackTime = 0f;
    public float stopAttackDelay = 0f;
    public float attackTimeReset = 0f;
    public float attackFixedTime = 0f;
}

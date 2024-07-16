using System;
using Consts;
using UnityEngine;

[Serializable]
public class BaseParameters
{
    [Header("Basic")]
    public ShootingType shootingType;

    [Header("Bullet")]
    public BulletType bulletType;
    public int power;
    public float speed;
    public float speedRate;
    public float angle;
    public float angleRate;
    public float startDistance;
    public float lifeTime;
    
    [Header("AttackCoolTime")]
    public float attackCooltime = 0f;
    public float attackTime = 0f;
    public float stopAttackDelay = 0f;
    public float attackTimeReset = 0f;
    public float attackFixedTime = 0f;
}

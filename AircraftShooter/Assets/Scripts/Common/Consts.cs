using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Consts
{
     public enum ShootingType {
        None,
        Forward,
        Directional,
        Nway,
        Circle,
        RandomNway,
        RandomCircle,
        RollingNway,
        WavingNway,
        CircleWavingNway,
        Spreading,
        RandomSpreading,
        Overtaking,
        Multiple,
        Homing,
        DelayHoming,
        RandomHoming,
        Placed,
        Aiming,
        AimingDirectional,
        Cross,
        RandomDirectional,
        Arc,
        CustomShape,
        Pattern,
        CustomShapeForward,
        ShooterShooter,
        GoblinFire,
    };

     public enum BulletType 
     {
        Bullet,
        HomingBullet,
        DelayHomingBullet,
        ArcBullet,
        ShapeBullet,
        ChildBullet,
        ShooterBullet,
        GoblinFireBullet,
    }

     public enum ShapeType
    {
        Circle,
        Polygon,
    }
}
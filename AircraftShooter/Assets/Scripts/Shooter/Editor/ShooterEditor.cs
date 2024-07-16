using Consts;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Shooter))]
public class ShooterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Shooter shooter = (Shooter)target;

        EditorGUILayout.LabelField("Pattern Type", EditorStyles.boldLabel);
        var newType = (ShootingType)EditorGUILayout.EnumPopup("Shooting Type", shooter.shootingType);
        // ShootingType selection
        StopPattern(newType, shooter);
        EditorGUILayout.LabelField(" ", EditorStyles.boldLabel);

        // 기본 인스펙터 필드
        EditorGUILayout.LabelField("Basic", EditorStyles.boldLabel);
        shooter.muzzle = (Transform)EditorGUILayout.ObjectField("Muzzle", shooter.muzzle, typeof(Transform), true);

        EditorGUILayout.LabelField(" ", EditorStyles.boldLabel);

        // Bullet fields
        EditorGUILayout.LabelField("Bullet", EditorStyles.boldLabel);
        shooter.bulletType = (Consts.BulletType)EditorGUILayout.EnumPopup("Bullet Type [총알 타입]", shooter.bulletType);
        shooter.power = EditorGUILayout.IntField("Power [데미지]", shooter.power);
        shooter.speed = EditorGUILayout.FloatField("Speed [총알 속도]", shooter.speed);
        shooter.speedRate = EditorGUILayout.FloatField("Speed Rate [총알 가속도]", shooter.speedRate);
        shooter.angle = EditorGUILayout.FloatField("Angle [총알 발사 각도]", shooter.angle);
        shooter.angleRate = EditorGUILayout.FloatField("Angle Rate [총알 회전 가속도]", shooter.angleRate);
        shooter.startDistance = EditorGUILayout.FloatField("Start Distance [총알 발사 시작 거리]", shooter.startDistance);
        shooter.lifeTime = EditorGUILayout.FloatField("Life Time [총알 생존시간]", shooter.lifeTime);
        

        EditorGUILayout.LabelField(" ", EditorStyles.boldLabel);
        // 선택된 ShootingType에 따라 다른 필드 표시
        switch (shooter.shootingType)
        {
            case ShootingType.Forward:
                shooter.bulletType = Consts.BulletType.Bullet;
                EditorGUILayout.LabelField("Forward", EditorStyles.boldLabel);
                shooter.forwardAngleSpeed = EditorGUILayout.FloatField("Forward Angle Speed", shooter.forwardAngleSpeed);
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                break;

            case ShootingType.Nway:
                shooter.bulletType = Consts.BulletType.Bullet;
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                break;

            case ShootingType.Circle:
                shooter.bulletType = Consts.BulletType.Bullet;
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                break;
            
            case ShootingType.RandomNway:
                shooter.bulletType = Consts.BulletType.Bullet;
                EditorGUILayout.LabelField("Forward", EditorStyles.boldLabel);
                shooter.forwardAngleSpeed = EditorGUILayout.FloatField("Forward Angle Speed", shooter.forwardAngleSpeed);
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                break;

            case ShootingType.Multiple:
                shooter.bulletType = Consts.BulletType.Bullet;
                EditorGUILayout.LabelField("Multiple", EditorStyles.boldLabel);
                shooter.multipleStep = EditorGUILayout.FloatField("Multiple Step", shooter.multipleStep);
                shooter.multipleRange = EditorGUILayout.FloatField("Multiple Range", shooter.multipleRange);
                break;

            case ShootingType.CustomShape:
                shooter.bulletType = Consts.BulletType.Bullet;
                EditorGUILayout.LabelField("Custom Shape", EditorStyles.boldLabel);
                shooter.rot = EditorGUILayout.FloatField("Rotation", shooter.rot);
                shooter.vertex = EditorGUILayout.IntSlider("Vertex", shooter.vertex, 3, 7);
                shooter.sup = EditorGUILayout.Slider("Sup", shooter.sup, 1, 5);
                shooter.rotateBulletCore = EditorGUILayout.Toggle("Rotate Bullet Core", shooter.rotateBulletCore);
                shooter.rotateBulletOffset = EditorGUILayout.Vector3Field("Rotate Bullet Offset", shooter.rotateBulletOffset);
                break;

            case ShootingType.Homing:
                shooter.bulletType = Consts.BulletType.HomingBullet;
                EditorGUILayout.LabelField("Aiming", EditorStyles.boldLabel);
                shooter.targetTransform = (Transform)EditorGUILayout.ObjectField("Target Transform", shooter.targetTransform, typeof(Transform), true);
                EditorGUILayout.LabelField("Homing", EditorStyles.boldLabel);
                shooter.turnSpeed = EditorGUILayout.FloatField("Turn Speed", shooter.turnSpeed);
                shooter.decreaseHomingSpeed = EditorGUILayout.Toggle("Decrease Homing Speed", shooter.decreaseHomingSpeed);
                shooter.homingSpeedRate = EditorGUILayout.FloatField("Homing Speed Rate", shooter.homingSpeedRate);
                break;

            case ShootingType.DelayHoming:
                shooter.bulletType = Consts.BulletType.DelayHomingBullet;
                EditorGUILayout.LabelField("Homing", EditorStyles.boldLabel);
                shooter.turnSpeed = EditorGUILayout.FloatField("Turn Speed", shooter.turnSpeed);
                shooter.decreaseHomingSpeed = EditorGUILayout.Toggle("Decrease Homing Speed", shooter.decreaseHomingSpeed);
                shooter.homingSpeedRate = EditorGUILayout.FloatField("Homing Speed Rate", shooter.homingSpeedRate);
                shooter.delayTime = EditorGUILayout.FloatField("Delay Time", shooter.delayTime);
                break;
            
            case ShootingType.RandomHoming:
                shooter.bulletType = Consts.BulletType.HomingBullet;
                EditorGUILayout.LabelField("Homing", EditorStyles.boldLabel);
                shooter.turnSpeed = EditorGUILayout.FloatField("Turn Speed", shooter.turnSpeed);
                shooter.decreaseHomingSpeed = EditorGUILayout.Toggle("Decrease Homing Speed", shooter.decreaseHomingSpeed);
                shooter.homingSpeedRate = EditorGUILayout.FloatField("Homing Speed Rate", shooter.homingSpeedRate);
                break;

            case ShootingType.RollingNway:
                shooter.bulletType = Consts.BulletType.Bullet;
                EditorGUILayout.LabelField("Forward", EditorStyles.boldLabel);
                shooter.forwardAngleSpeed = EditorGUILayout.FloatField("Forward Angle Speed", shooter.forwardAngleSpeed);
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                EditorGUILayout.LabelField("Rolling N-way", EditorStyles.boldLabel);
                shooter.nWayCount = EditorGUILayout.IntField("N-way Count", shooter.nWayCount);
                break;

            case ShootingType.WavingNway:
                shooter.bulletType = Consts.BulletType.Bullet;
                EditorGUILayout.LabelField("Forward", EditorStyles.boldLabel);
                shooter.forwardAngleSpeed = EditorGUILayout.FloatField("Forward Angle Speed", shooter.forwardAngleSpeed);
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                EditorGUILayout.LabelField("Waving N-way", EditorStyles.boldLabel);
                shooter.wavingAngleRange = EditorGUILayout.FloatField("Waving Angle Range", shooter.wavingAngleRange);
                shooter.cycle = EditorGUILayout.IntField("Cycle", shooter.cycle);
                break;

            case ShootingType.CircleWavingNway:
                shooter.bulletType = Consts.BulletType.Bullet;
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                EditorGUILayout.LabelField("Waving N-way", EditorStyles.boldLabel);
                shooter.wavingAngleRange = EditorGUILayout.FloatField("Waving Angle Range", shooter.wavingAngleRange);
                shooter.cycle = EditorGUILayout.IntField("Cycle", shooter.cycle);
                break;

            case ShootingType.Placed:
                shooter.bulletType = Consts.BulletType.Bullet;
                EditorGUILayout.LabelField("Placed", EditorStyles.boldLabel);
                shooter.moveTime = EditorGUILayout.FloatField("Move Time", shooter.moveTime);
                shooter.stopTime = EditorGUILayout.FloatField("Stop Time", shooter.stopTime);
                shooter.placedStopSpeed = EditorGUILayout.FloatField("Placed Stop Speed", shooter.placedStopSpeed);
                break;

            case ShootingType.Aiming:
                shooter.bulletType = Consts.BulletType.Bullet;
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                EditorGUILayout.LabelField("Aiming", EditorStyles.boldLabel);
                shooter.targetTransform = (Transform)EditorGUILayout.ObjectField("Target Transform", shooter.targetTransform, typeof(Transform), true);
                shooter.stopAttackCooltime = EditorGUILayout.Toggle("Stop Attack Cooltime", shooter.stopAttackCooltime);
                shooter.targetfixedAngle = EditorGUILayout.FloatField("Target Fixed Angle", shooter.targetfixedAngle);
                break;

            case ShootingType.Spreading:
            shooter.bulletType = Consts.BulletType.Bullet;
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                EditorGUILayout.LabelField("Spreading", EditorStyles.boldLabel);
                shooter.groupSpeed = EditorGUILayout.FloatField("Group Speed", shooter.groupSpeed);
                shooter.groupCount = EditorGUILayout.FloatField("Group Count", shooter.groupCount);
                break;
            
            case ShootingType.RandomSpreading:
                shooter.bulletType = Consts.BulletType.Bullet;
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                shooter.speedRange = EditorGUILayout.FloatField("Speed Range", shooter.speedRange);
                break;

            case ShootingType.Overtaking:
                shooter.bulletType = Consts.BulletType.Bullet;
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                EditorGUILayout.LabelField("Spreading", EditorStyles.boldLabel);
                shooter.groupSpeed = EditorGUILayout.FloatField("Group Speed", shooter.groupSpeed);
                shooter.groupCount = EditorGUILayout.FloatField("Group Count", shooter.groupCount);
                EditorGUILayout.LabelField("Overtaking", EditorStyles.boldLabel);
                shooter.groupAngle = EditorGUILayout.FloatField("Group Angle", shooter.groupAngle);
                shooter.groupInterval = EditorGUILayout.FloatField("Group Interval", shooter.groupInterval);
                break;

             case ShootingType.Cross:
                //shooter.bulletType = Consts.BulletType.ShapeBullet;
                break;

            case ShootingType.Arc:
                shooter.bulletType = Consts.BulletType.ArcBullet;
                EditorGUILayout.LabelField("Aiming", EditorStyles.boldLabel);
                shooter.targetTransform = (Transform)EditorGUILayout.ObjectField("Target Transform", shooter.targetTransform, typeof(Transform), true);
                EditorGUILayout.LabelField("Arc", EditorStyles.boldLabel);
                shooter.arrivalTime = EditorGUILayout.FloatField("Arrival Time", shooter.arrivalTime);
                shooter.height = EditorGUILayout.FloatField("Height", shooter.height);
                break;

            case ShootingType.Pattern:
                shooter.bulletType = Consts.BulletType.Bullet;
                EditorGUILayout.LabelField("Pattern", EditorStyles.boldLabel);
                shooter.pat_width = EditorGUILayout.IntField("Pat Width", shooter.pat_width);
                shooter.pat_height = EditorGUILayout.IntField("Pat Height", shooter.pat_height);
                break;

            case ShootingType.CustomShapeForward:
                shooter.bulletType = Consts.BulletType.ShapeBullet;
                EditorGUILayout.LabelField("CustomShapeForward", EditorStyles.boldLabel);
                shooter.shapeType = (Consts.ShapeType)EditorGUILayout.EnumPopup("Shape Type", shooter.shapeType);
                shooter.vertexShape = EditorGUILayout.IntField("VertexShape", shooter.vertexShape);
                shooter.radius = EditorGUILayout.FloatField("Radius", shooter.radius);
                shooter.segments = EditorGUILayout.IntField("Segments", shooter.segments);
                shooter.angleSpeed = EditorGUILayout.FloatField("AngleSpeed", shooter.angleSpeed);
                shooter.circleAngle = EditorGUILayout.IntField("CircleAngle", shooter.circleAngle);
                break;
             case ShootingType.ShooterShooter:
                shooter.bulletType = Consts.BulletType.ShooterBullet;
                break;

            // 다른 ShootingType에 대한 케이스 추가...
            default:
                EditorGUILayout.LabelField("Selected type has no custom fields.");
                break;
        }

        EditorGUILayout.LabelField(" ", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("AttackCoolTime", EditorStyles.boldLabel);
        shooter.attackCooltime = EditorGUILayout.FloatField("Attack Cooltime [총알 발사 시간]", shooter.attackCooltime);
        shooter.attackTime = EditorGUILayout.FloatField("Attack Time [총알 발사 간격 시간]", shooter.attackTime);
        shooter.stopAttackDelay = EditorGUILayout.FloatField("Stop Attack Delay [총알 발사 정지시간]", shooter.stopAttackDelay);
        shooter.attackFixedTime = EditorGUILayout.FloatField("Attack Fixed Time [정해진 시간동안 총알 발사]", shooter.attackFixedTime);

        EditorGUILayout.LabelField(" ", EditorStyles.boldLabel);
        if (GUILayout.Button("Start Shoot"))
        {
            shooter.StartShoot();
        }


        EditorGUILayout.LabelField(" ", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("File Save/Load", EditorStyles.boldLabel);
        shooter.jsonName = EditorGUILayout.TextField("File Name", shooter.jsonName);
        if (GUILayout.Button("Save Shooter"))
        {
            shooter.SaveParameters();
        }
        if (GUILayout.Button("Load Shooter"))
        {
            shooter.LoadParameters();
        }

        // 변경 사항 저장
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    void StopPattern(ShootingType _newType, Shooter _shooter)
    {
         if (_newType != _shooter.shootingType)
        {
            _shooter.RemoveType(_shooter.shootingType);
            _shooter.SetType(_newType);
            _shooter.shootingType = _newType;
        }
    }
}

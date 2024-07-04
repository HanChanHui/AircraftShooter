using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Shooter))]
public class ShooterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Shooter shooter = (Shooter)target;

        var newType = (Shooter.ShootingType)EditorGUILayout.EnumPopup("Shooting Type", shooter.shootingType);
        shooter.shootingType = newType;

        // 기본 인스펙터 필드
        EditorGUILayout.LabelField("Basic", EditorStyles.boldLabel);
        shooter.muzzle = (Transform)EditorGUILayout.ObjectField("Muzzle", shooter.muzzle, typeof(Transform), true);
        shooter.showMuzzleFlash = EditorGUILayout.Toggle("Show Muzzle Flash", shooter.showMuzzleFlash);

        shooter.shootingPattern = (ShootingPattern)EditorGUILayout.ObjectField("Shooting Pattern", shooter.shootingPattern, typeof(ShootingPattern), false);
        shooter.savePattern = EditorGUILayout.Toggle("Save Pattern", shooter.savePattern);
        shooter.loadPattern = EditorGUILayout.Toggle("Load Pattern", shooter.loadPattern);

        // Bullet fields
        EditorGUILayout.LabelField("Bullet", EditorStyles.boldLabel);
        shooter.bulletType = (Shooter.BulletType)EditorGUILayout.EnumPopup("Bullet Type", shooter.bulletType);
        shooter.power = EditorGUILayout.IntField("Power", shooter.power);
        shooter.speed = EditorGUILayout.FloatField("Speed", shooter.speed);
        shooter.speedRate = EditorGUILayout.FloatField("Speed Rate", shooter.speedRate);
        shooter.angle = EditorGUILayout.FloatField("Angle", shooter.angle);
        shooter.angleRate = EditorGUILayout.FloatField("Angle Rate", shooter.angleRate);
        shooter.startDistance = EditorGUILayout.FloatField("Start Distance", shooter.startDistance);
        shooter.lifeTime = EditorGUILayout.FloatField("Life Time", shooter.lifeTime);

        // ShootingType selection
        //var newType = (Shooter.ShootingType)EditorGUILayout.EnumPopup("Shooting Type", shooter.shootingType);
        if (newType != shooter.shootingType)
        {
            shooter.RemoveType(shooter.shootingType);
            shooter.SetType(newType);
        }
        

        EditorGUILayout.LabelField(" ", EditorStyles.boldLabel);
        // 선택된 ShootingType에 따라 다른 필드 표시
        switch (shooter.shootingType)
        {
            case Shooter.ShootingType.Forward:
                EditorGUILayout.LabelField("Forward", EditorStyles.boldLabel);
                shooter.forwardAngleSpeed = EditorGUILayout.FloatField("Forward Angle Speed", shooter.forwardAngleSpeed);
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                break;

            case Shooter.ShootingType.Nway:
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                break;

            case Shooter.ShootingType.Circle:
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                break;
            
            case Shooter.ShootingType.RandomNway:
                EditorGUILayout.LabelField("Forward", EditorStyles.boldLabel);
                shooter.forwardAngleSpeed = EditorGUILayout.FloatField("Forward Angle Speed", shooter.forwardAngleSpeed);
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                break;

            case Shooter.ShootingType.Multiple:
                EditorGUILayout.LabelField("Multiple", EditorStyles.boldLabel);
                shooter.multipleStep = EditorGUILayout.FloatField("Multiple Step", shooter.multipleStep);
                shooter.multipleRange = EditorGUILayout.FloatField("Multiple Range", shooter.multipleRange);
                break;

            case Shooter.ShootingType.CustomShape:
                EditorGUILayout.LabelField("Custom Shape", EditorStyles.boldLabel);
                shooter.rot = EditorGUILayout.FloatField("Rotation", shooter.rot);
                shooter.vertex = EditorGUILayout.IntSlider("Vertex", shooter.vertex, 3, 7);
                shooter.sup = EditorGUILayout.Slider("Sup", shooter.sup, 1, 5);
                shooter.rotateBulletCore = EditorGUILayout.Toggle("Rotate Bullet Core", shooter.rotateBulletCore);
                shooter.rotateBulletOffset = EditorGUILayout.Vector3Field("Rotate Bullet Offset", shooter.rotateBulletOffset);
                break;

            case Shooter.ShootingType.Homing:
                EditorGUILayout.LabelField("Homing", EditorStyles.boldLabel);
                shooter.turnSpeed = EditorGUILayout.FloatField("Turn Speed", shooter.turnSpeed);
                shooter.decreaseHomingSpeed = EditorGUILayout.Toggle("Decrease Homing Speed", shooter.decreaseHomingSpeed);
                shooter.homingSpeedRate = EditorGUILayout.FloatField("Homing Speed Rate", shooter.homingSpeedRate);
                break;

            case Shooter.ShootingType.DelayHoming:
                EditorGUILayout.LabelField("Homing", EditorStyles.boldLabel);
                shooter.turnSpeed = EditorGUILayout.FloatField("Turn Speed", shooter.turnSpeed);
                shooter.decreaseHomingSpeed = EditorGUILayout.Toggle("Decrease Homing Speed", shooter.decreaseHomingSpeed);
                shooter.homingSpeedRate = EditorGUILayout.FloatField("Homing Speed Rate", shooter.homingSpeedRate);
                shooter.delayTime = EditorGUILayout.FloatField("Delay Time", shooter.delayTime);
                break;
            
            case Shooter.ShootingType.RandomHoming:
                EditorGUILayout.LabelField("Homing", EditorStyles.boldLabel);
                shooter.turnSpeed = EditorGUILayout.FloatField("Turn Speed", shooter.turnSpeed);
                shooter.decreaseHomingSpeed = EditorGUILayout.Toggle("Decrease Homing Speed", shooter.decreaseHomingSpeed);
                shooter.homingSpeedRate = EditorGUILayout.FloatField("Homing Speed Rate", shooter.homingSpeedRate);
                break;

            case Shooter.ShootingType.RollingNway:
                EditorGUILayout.LabelField("Forward", EditorStyles.boldLabel);
                shooter.forwardAngleSpeed = EditorGUILayout.FloatField("Forward Angle Speed", shooter.forwardAngleSpeed);
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                EditorGUILayout.LabelField("Rolling N-way", EditorStyles.boldLabel);
                shooter.nWayCount = EditorGUILayout.IntField("N-way Count", shooter.nWayCount);
                break;

            case Shooter.ShootingType.WavingNway:
                EditorGUILayout.LabelField("Forward", EditorStyles.boldLabel);
                shooter.forwardAngleSpeed = EditorGUILayout.FloatField("Forward Angle Speed", shooter.forwardAngleSpeed);
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                EditorGUILayout.LabelField("Waving N-way", EditorStyles.boldLabel);
                shooter.wavingAngleRange = EditorGUILayout.FloatField("Waving Angle Range", shooter.wavingAngleRange);
                shooter.cycle = EditorGUILayout.IntField("Cycle", shooter.cycle);
                break;

            case Shooter.ShootingType.CircleWavingNway:
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                EditorGUILayout.LabelField("Waving N-way", EditorStyles.boldLabel);
                shooter.wavingAngleRange = EditorGUILayout.FloatField("Waving Angle Range", shooter.wavingAngleRange);
                shooter.cycle = EditorGUILayout.IntField("Cycle", shooter.cycle);
                break;

            case Shooter.ShootingType.Placed:
                EditorGUILayout.LabelField("Placed", EditorStyles.boldLabel);
                shooter.moveTime = EditorGUILayout.FloatField("Move Time", shooter.moveTime);
                shooter.stopTime = EditorGUILayout.FloatField("Stop Time", shooter.stopTime);
                shooter.placedStopSpeed = EditorGUILayout.FloatField("Placed Stop Speed", shooter.placedStopSpeed);
                break;

            case Shooter.ShootingType.Aiming:
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                EditorGUILayout.LabelField("Aiming", EditorStyles.boldLabel);
                shooter.targetTransform = (Transform)EditorGUILayout.ObjectField("Target Transform", shooter.targetTransform, typeof(Transform), true);
                shooter.stopAttackCooltime = EditorGUILayout.Toggle("Stop Attack Cooltime", shooter.stopAttackCooltime);
                shooter.targetfixedAngle = EditorGUILayout.FloatField("Target Fixed Angle", shooter.targetfixedAngle);
                break;

            case Shooter.ShootingType.Spreading:
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                EditorGUILayout.LabelField("Spreading", EditorStyles.boldLabel);
                shooter.groupSpeed = EditorGUILayout.FloatField("Group Speed", shooter.groupSpeed);
                shooter.groupCount = EditorGUILayout.FloatField("Group Count", shooter.groupCount);
                break;
            
            case Shooter.ShootingType.RandomSpreading:
                EditorGUILayout.LabelField("Nway", EditorStyles.boldLabel);
                shooter.angleRange = EditorGUILayout.FloatField("Angle Range", shooter.angleRange);
                shooter.count = EditorGUILayout.IntField("Count", shooter.count);
                shooter.speedRange = EditorGUILayout.FloatField("Speed Range", shooter.speedRange);
                break;

            case Shooter.ShootingType.Overtaking:
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

             case Shooter.ShootingType.Cross:

                break;

            case Shooter.ShootingType.Arc:
                EditorGUILayout.LabelField("Aiming", EditorStyles.boldLabel);
                shooter.targetTransform = (Transform)EditorGUILayout.ObjectField("Target Transform", shooter.targetTransform, typeof(Transform), true);
                EditorGUILayout.LabelField("Arc", EditorStyles.boldLabel);
                shooter.arrivalTime = EditorGUILayout.FloatField("Arrival Time", shooter.arrivalTime);
                shooter.height = EditorGUILayout.FloatField("Height", shooter.height);
                break;

             case Shooter.ShootingType.Pattern:
                EditorGUILayout.LabelField("Pattern", EditorStyles.boldLabel);
                shooter.pat_width = EditorGUILayout.IntField("Pat Width", shooter.pat_width);
                shooter.pat_height = EditorGUILayout.IntField("Pat Height", shooter.pat_height);
                break;

            // 다른 ShootingType에 대한 케이스 추가...
            default:
                EditorGUILayout.LabelField("Selected type has no custom fields.");
                break;
        }

        EditorGUILayout.LabelField(" ", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("AttackCoolTime", EditorStyles.boldLabel);
        shooter.attackCooltime = EditorGUILayout.FloatField("Attack Cooltime", shooter.attackCooltime);
        shooter.attackTime = EditorGUILayout.FloatField("Attack Time", shooter.attackTime);
        shooter.attackTimeReset = EditorGUILayout.FloatField("Attack TimeReset", shooter.attackTimeReset);
        shooter.stopAttack = EditorGUILayout.FloatField("Stop Attack", shooter.stopAttack);



        // 변경 사항 저장
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}

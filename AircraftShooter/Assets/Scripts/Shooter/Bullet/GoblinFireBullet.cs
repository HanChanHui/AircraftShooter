using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinFireBullet : Bullet
{
    public enum BulletStyle
    {
        ChildGoblinFire,
        Homing,
    }

    [SerializeField] Transform targetTransform;

    [Header("GoblinFire Bullet")]
    [SerializeField] float targetRange = 5.0f;

    [Header("Child GoblinFire Bullet")]
    [SerializeField] BulletStyle bulletStyle;
    [SerializeField] bool childCreate;
    [SerializeField] float childBulletSpeed;
    [SerializeField] float childTurnSpeed;
    [SerializeField] float childAngle;
    [SerializeField] float childLifeTime;
    [SerializeField] float childWavingRange;
    [SerializeField] float childWavingCycle;

    Rigidbody myRigidbody;
    int startDistance;
    float turnSpeed;
    bool findTarget;
    bool decreaseHomingSpeed;
    float homingSpeedRate;

    float wavingRange;
    float wavingCycle;
    float waveOffset;

    protected override void MyStart() {
        base.MyStart();
        myRigidbody = GetComponent<Rigidbody>();

        waveOffset = Random.Range(0f, Mathf.PI * 2);
    }

    public void Init(float turnSpeed, Transform target, float homingSpeedRate, float wavingRange, float wavingCycle, bool decreaseHomingSpeed = false) {
        this.turnSpeed = turnSpeed;

        if (target) {
            this.targetTransform = target;
            findTarget = true;
        } else {
            this.targetTransform = null;
            findTarget = false;
        }

        this.homingSpeedRate = homingSpeedRate;
        this.decreaseHomingSpeed = decreaseHomingSpeed;
        this.wavingRange = wavingRange;
        this.wavingCycle = wavingCycle;
    }

    public override void Create(Vector3 pos, Quaternion rot, int power, float speed, float speedRate, float angle, float angleRate, bool isCritical = false, float startDistance = 0, float lifeTime = 0) {
        myTransform.SetPositionAndRotation(pos, rot);
        this.isCritical = isCritical;
        this.power = power;
        this.speed = speed;
        this.speedRate = speedRate;

        myTransform.rotation *= Quaternion.AngleAxis(angle, Vector3.up);

        if (lifeTime > 0) {
            this.lifeTime = lifeTime;
        }

        isDead = false;


        Collider[] initialCollisions = Physics.OverlapSphere(myTransform.position, 0.1f,
                                     colliderMask);

        if (initialCollisions.Length > 0) {
            OnHitObject(initialCollisions[0], myTransform.position);
        }

        if (gameObject.activeSelf) {
            MoveStart();

            if (lifeTime > 0) {
                Invoke(nameof(Disappear), lifeTime);
            }
        }
    }

    protected override void Move() {
        speed += speedRate;

        if (decreaseHomingSpeed) {
            speed += homingSpeedRate;

            if (speed < 0) {
                speed = 0;
                decreaseHomingSpeed = false;
                Disappear();
            }
        }
        
        // **새로 추가된 부분: x축 주기적 변동 계산**
        float wave = wavingRange * Mathf.Sin(Time.time * wavingCycle + waveOffset);
        Vector3 waveMovement = myTransform.right * wave; // 로컬 x축을 기준으로 변동
        Vector3 movement = myTransform.forward * speed + waveMovement; // 로컬 x축 변동을 더한 이동 벡터
        myRigidbody.velocity = movement;

        if (findTarget && targetTransform) {
            Quaternion targetRotation = Quaternion.LookRotation(targetTransform.position - myTransform.position);
            Quaternion combinedRotation = Quaternion.RotateTowards(myTransform.rotation, targetRotation, turnSpeed);

            // 새로운 회전 적용
            myRigidbody.MoveRotation(combinedRotation);
        }
        else 
        {
            FindTarget();
        }

        CheckBulletCollision();
    }

    void FindTarget() {
        Collider[] hitColliders = Physics.OverlapSphere(myTransform.position, targetRange, colliderMask, QueryTriggerInteraction.Collide);
        for (int i = 0; i < hitColliders.Length; i++) {
            if (hitColliders[i].CompareTag("Player")) {
                targetTransform = hitColliders[i].transform;
                findTarget = true;
                break;
            }
        }
    }

    protected override void MoveStart() {
        StartCoroutine(nameof(CoFixedUpdate));
    }

    IEnumerator CoFixedUpdate() {
        while (true) {
            Move();

            yield return new WaitForFixedUpdate();
        }
    }

    protected override void Disappear() {

        base.Disappear();
    }

    protected override void MyDestroy() {
        base.MyDestroy();
        //switch(bulletStyle)
        //{
        //    case BulletStyle.ChildGoblinFire:
        //        if (childCreate)
        //        {
        //            GoblinFireShoot();
        //        }
        //        break;
        //     case BulletStyle.Homing:
        //        HomingShoot();
        //        break;
        //}
        
        myTransform.position = Vector3.zero;
    }

    void GoblinFireShoot() {
        float angleChange = -childAngle;
        for (int i = 0; i < 3; i++)
        {
            GoblinFireBullet bullet = HSPoolManager.Instance.NewItem<GoblinFireBullet>(Consts.BulletType.GoblinFireBullet.ToString());
            if (bullet)
            {
                Debug.Log(angleChange);
                bullet.Init(childTurnSpeed, targetTransform, homingSpeedRate, childWavingRange, childWavingCycle, decreaseHomingSpeed);
                bullet.CheckOutBound = this.CheckOutBound;
                bullet.Create(transform.position, transform.rotation, power,
                        childBulletSpeed, speedRate , angleChange, angleRate, isCritical, startDistance, childLifeTime);
                angleChange += childAngle;
                bullet.childCreate = false;
            }
        }
    }

    void HomingShoot()
    {
        float angleChange = -childAngle;
        for (int i = 0; i < 3; i++)
        {
            HomingBullet bullet = HSPoolManager.Instance.NewItem<HomingBullet>(Consts.BulletType.HomingBullet.ToString());
            if (bullet)
            {
                bullet.Init(childTurnSpeed, targetTransform, homingSpeedRate, decreaseHomingSpeed);
                bullet.CheckOutBound = this.CheckOutBound;
                bullet.Create(transform.position, transform.rotation, power,
                        childBulletSpeed, speedRate, angleChange, angleRate, isCritical, startDistance, childLifeTime);
                angleChange += childAngle;
            }
        }
    }
}

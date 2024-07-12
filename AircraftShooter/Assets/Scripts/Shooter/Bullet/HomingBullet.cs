using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : Bullet {
    [Header("Homing Bullet")]
    [SerializeField] float targetRange = 5.0f;
    [SerializeField] string shootSfx;
    [SerializeField] string hitSfx;

    [SerializeField] Transform targetTransform;
    Rigidbody myRigidbody;
    float turnSpeed;
    bool findTarget;
    bool decreaseHomingSpeed;
    float homingSpeedRate;

    protected override void MyStart() {
        base.MyStart();
        myRigidbody = GetComponent<Rigidbody>();
    }

    public void Init(float turnSpeed, Transform target, float homingSpeedRate, bool decreaseHomingSpeed = false) {
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

        myRigidbody.velocity = myTransform.forward * speed;

        if (findTarget && targetTransform) {
            Quaternion rot = Quaternion.LookRotation(targetTransform.position - myTransform.position);
            myRigidbody.MoveRotation(Quaternion.RotateTowards(myTransform.rotation, rot, turnSpeed));
        } else {
            FindTarget();
        }

        CheckBulletCollision();
    }

    void FindTarget() {
        Collider[] hitColliders = Physics.OverlapSphere(myTransform.position, targetRange, colliderMask, QueryTriggerInteraction.Collide);
        for (int i = 0; i < hitColliders.Length; i++) {
            if (hitColliders[i].CompareTag("Enemy")) {
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
        myTransform.position = Vector3.zero;
    }
}


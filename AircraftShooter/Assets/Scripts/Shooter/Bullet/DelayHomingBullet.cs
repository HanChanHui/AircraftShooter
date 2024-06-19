using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayHomingBullet : Bullet
{
        [Header("Delay Homing Bullet")]
        [SerializeField] string shootSfx;
        [SerializeField] string hitSfx;
        [SerializeField] float delayTime = 2f;
        [SerializeField] float targetSpeed = 10f;

        Transform targetTransform;
        Rigidbody myRigidbody;
        float turnSpeed;

        protected override void MyStart() {
            base.MyStart();
            myTransform = transform;
            myRigidbody = GetComponent<Rigidbody>();
        }

        public void Init(float turnSpeed, float delayTime) {
            this.turnSpeed = turnSpeed;
            this.delayTime = delayTime;
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

            if (delayTime > 0) {
                StartCoroutine(nameof(CoDelayAndSpeedUp));
            }

            StartCoroutine(nameof(CoFixedUpdate));
            isDead = false;
        }

        IEnumerator CoFixedUpdate() {
            while (true) {
                Move();

                yield return new WaitForFixedUpdate();
            }
        }

        IEnumerator CoDelayAndSpeedUp() {
            yield return new WaitForSeconds(delayTime);

            if (speedRate == 0) {
                yield break;
            }

            while (true) {
                speed += speedRate;

                if (speed >= targetSpeed) {
                    speed = targetSpeed;
                    break;
                }

                yield return new WaitForFixedUpdate();
            }
        }

        protected override void Move() {
            myRigidbody.velocity = myTransform.forward * speed;
            
            CheckCollisions();
        }
    }

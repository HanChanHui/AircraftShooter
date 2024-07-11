using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBullet : MonoBehaviour, IMemoryPool
{
    [Header("Basic")]
    [SerializeField] private string mpType;
    [SerializeField] private float speed;
    [SerializeField] private float angle;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float lifeTime;

    private Transform myTransform;
    [SerializeField] private Shooter shooter;

    [Header("AttackCoolTime")]
    public float attackCooltime = 0f;
    public float attackTime = 0f;
    public float attackTimeReset = 0f;
    public float stopAttack = 0f;
    public bool canAttack = true;


    public void MPStart()
    {
        myTransform = transform;
        shooter.Init();
        //childBullet.ParentBullet = this;
    }

    public void Create(Vector3 pos, Quaternion rot, float speed, float angle)
    {
        myTransform.position = pos;
        myTransform.rotation = rot;
        this.speed = speed;
        this.angle = angle;

        StartCoroutine("CoUpdate");

        if (shooter != null)
        {
            Debug.Log("실행");
            StartCoroutine(CoCheckDistance());
            StartCoroutine(CoStopAttackCooltime());
        }

        if (lifeTime > 0)
        {
            Invoke("MyDestroy", lifeTime);
        }
        else
        {
            Invoke("MyDestroy", 5.0f);
        }
    }

    public IEnumerator CoCheckDistance()
    {
        while (true)
        {
            if (canAttack)
            {
                StartCoroutine(CoAttack());
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator CoAttack()
    {
        shooter.Shoot();
        yield return new WaitForSeconds(attackTime);
        StartCoroutine(CoAttackCooltime());
        StartCoroutine(CoCheckDistance());
    }


    private IEnumerator CoAttackCooltime()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooltime);
        canAttack = true;
    }

    public IEnumerator CoStopAttackCooltime()
    {
        while (true)
        {
            attackCooltime = 0f;
            yield return new WaitForSeconds(stopAttack);
            attackCooltime = attackTimeReset;
            yield return new WaitForSeconds(attackCooltime);
        }
    }

    IEnumerator CoUpdate()
    {
        while (true)
        {
            Move();

            yield return null;
        }
    }

    private void Move()
    {
        float moveDistance = speed * Time.deltaTime;
        myTransform.Translate(Vector3.Normalize(Vector3.forward) * moveDistance);
        Quaternion rot = Quaternion.Euler(transform.rotation.x, angle, transform.rotation.z);
        myTransform.rotation = rot;
    }


    private void MyDestroy()
    {
        CancelInvoke();
        StopAllCoroutines();
        HSPoolManager.Instance.RemoveItem(mpType, gameObject);
    }
}


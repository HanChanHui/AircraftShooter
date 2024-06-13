using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtr : MonoBehaviour
{
    [Header("Shooter")]
    [SerializeField] protected Shooter basicShooter;

    [SerializeField] private bool canAttack = false;
    [SerializeField] private float attackCooltime = 0f;
    [SerializeField] private float attackTime = 0f;
    [SerializeField] private float stopAttackDelay = 0f;


    // public virtual void MPStart() {
    //     this.MyStart();
    // }

    // public virtual void MyStart()
    // {
        
    // }

    private void Start() 
    {
        basicShooter.Init();
        StartCoroutine(CoCheckDistance());
    }

    protected IEnumerator CoCheckDistance() 
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

    protected IEnumerator CoAttack() 
    {
            yield return new WaitForSeconds(attackTime);
            basicShooter.Shoot();
            yield return new WaitForSeconds(stopAttackDelay);


            StartCoroutine(CoAttackCooltime());
            StartCoroutine(CoCheckDistance());
    }

    protected IEnumerator CoAttackCooltime() {
            canAttack = false;
            yield return new WaitForSeconds(attackCooltime);
            canAttack = true;
        }
}

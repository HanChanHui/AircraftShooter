using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtr : MonoBehaviour
{
    [Header("Shooter")]
    [SerializeField] protected Shooter basicShooter;
    //[SerializeField] protected Shooter basicShooter2;

    [SerializeField] private bool canStopAttack = true;
    [SerializeField] private bool canAttack = false;
    [SerializeField] private float attackCooltime = 0f;
    [SerializeField] private float attackTime = 0f;
    [SerializeField] private float stopAttackDelay = 0f;

    private void Start() 
    {
        basicShooter.Init();
        //basicShooter2.Init();
        StartCoroutine(CoCheckDistance());
        StartCoroutine(CoStopAttackCooltime());
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
            //basicShooter2.Shoot();
            //yield return new WaitForSeconds(stopAttackDelay);


            StartCoroutine(CoAttackCooltime());
            StartCoroutine(CoCheckDistance());
            
    }

    protected IEnumerator CoAttackCooltime()
    {
            canAttack = false;
            yield return new WaitForSeconds(attackCooltime);
            canAttack = true;
    }

    protected IEnumerator CoStopAttackCooltime()
    {
        while(true)
        {
            attackCooltime = 0f;
            yield return new WaitForSeconds(stopAttackDelay);
            attackCooltime = 1f;
            yield return new WaitForSeconds(attackCooltime);
            //basicShooter.StopAttackCooltime(canStopAttack);
        }
    }
}

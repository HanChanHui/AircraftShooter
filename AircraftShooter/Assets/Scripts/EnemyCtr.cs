using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class EnemyCtr : MonoBehaviour
{
    [Header("Shooter")]
    [SerializeField] protected Shooter basicShooter;
    //[SerializeField] protected Shooter basicShooter2;

    [SerializeField] bool canAttack;
    //[SerializeField] private bool canStopAttack = true;

    private void Start() 
    {
        basicShooter.Init();
        canAttack = true;
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
            yield return new WaitForSeconds(basicShooter.attackTime);
            basicShooter.Shoot();
            //basicShooter2.Shoot();



            StartCoroutine(CoAttackCooltime());
            StartCoroutine(CoCheckDistance());
            
    }

    protected IEnumerator CoAttackCooltime()
    {
            canAttack = false;
            yield return new WaitForSeconds(basicShooter.attackCooltime);
            canAttack = true;
    }

    protected IEnumerator CoStopAttackCooltime()
    {
        while(true)
        {
            basicShooter.attackCooltime = 0f;
            yield return new WaitForSeconds(basicShooter.stopAttack);
            basicShooter.attackCooltime = basicShooter.attackTimeReset;
            yield return new WaitForSeconds(basicShooter.attackCooltime);
            //basicShooter.StopAttackCooltime(canStopAttack);
        }
    }

    private IEnumerator PlacedCircleShoot()
    {
        
        yield return null;
    }

    
}

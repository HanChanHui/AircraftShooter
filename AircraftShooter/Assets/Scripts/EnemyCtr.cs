using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class EnemyCtr : MonoBehaviour
{
    public List<Shooter> shooters = new List<Shooter>();

    private void Start()
    {
        foreach (var shooter in shooters)
        {
            if (shooter != null)
            {
                shooter.Init();
                StartCoroutine(shooter.CoCheckDistance());
                StartCoroutine(shooter.CoStopAttackCooltime());
            }
        }
    }

}

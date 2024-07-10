using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBullet : MonoBehaviour, IMemoryPool
{
    [Header("Basic")]
    [SerializeField] private string mpType;
    public CircleShapeBullet ParentBullet { get; set; }


    public void MPStart()
    {
        StartCoroutine(CoUpdate());

        if (ParentBullet.lifeTime > 0)
        {
            Invoke("MyDestroy", ParentBullet.lifeTime);
        }
        else
        {
            Invoke("MyDestroy", 5.0f);
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
        ParentBullet.transform.Rotate(Vector3.up, ParentBullet.angleSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        ParentBullet.RunChildrenTriggerEnter(other);
    }

    private void MyDestroy()
    {
        StopAllCoroutines();
        HSPoolManager.Instance.RemoveItem(mpType, gameObject);
    }
}

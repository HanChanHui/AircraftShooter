using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBullet : MonoBehaviour, IMemoryPool
{
    [Header("Basic")]
    [SerializeField] private string mpType;
    [SerializeField] private float lifeTime;
    [SerializeField] private float angleSpeed;
    private Transform parentTransform;
    public CircleShapeBullet ParentBullet { get; set; }

    private Transform myTransform;

    public void MPStart()
    {
        myTransform = transform;
    }

    public void Create(Vector3 pos, Transform parentpos, float angleSpeed, float lifeTime)
    {
        myTransform.position = pos;
        this.parentTransform = parentpos;
        this.angleSpeed = angleSpeed;
        this.lifeTime = lifeTime;

        gameObject.SetActive(true);
        StartCoroutine(CoUpdate());

        if (lifeTime > 0)
        {
            Invoke("MyDestroy", lifeTime);
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
        myTransform.RotateAround(parentTransform.position, Vector3.up, angleSpeed * Time.deltaTime);
    }

    private void MyDestroy()
    {
        StopAllCoroutines();
        HSPoolManager.Instance.RemoveItem(mpType, gameObject);
    }
}

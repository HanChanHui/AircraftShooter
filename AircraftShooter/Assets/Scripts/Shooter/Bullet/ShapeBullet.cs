using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeBullet : MonoBehaviour, IMemoryPool
{
    [Header("Basic")]
    [SerializeField] private string mpType;
    [SerializeField] private string hitEffect;
    [SerializeField] private int power;
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float lifeTime;
    [SerializeField] private bool showValueText;
    
    [SerializeField] private Transform bulletBody;
    [SerializeField] private ChildBullet childBullet;

    private Transform myTransform;

    private bool isDead;
    private bool isCritical;

    public void MPStart()
    {
        myTransform = transform;
        childBullet.ParentBullet = this;
    }

    public void Create(Vector3 pos, Quaternion rot, int power, float speed, bool isCritical) 
    {
        myTransform.position = pos;
        myTransform.rotation = rot;
        this.power = power;
        this.speed = speed;
        this.isCritical = isCritical;

        isDead = false;

        StartCoroutine("CoUpdate");
        if (lifeTime > 0) {
            Invoke("MyDestroy", lifeTime);
        } else {
            Invoke("MyDestroy", 5.0f);
        }
    }

    IEnumerator CoUpdate() {
        while (true) {
            Move();

            yield return null;
        }
    }

    private void Move() {
        myTransform.Translate(Vector3.forward * speed * Time.deltaTime);
        bulletBody.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
    }

    public void RunChildrenTriggerEnter(Collider other) {
        LivingEntity entity = other.GetComponent<LivingEntity> ();
        if (entity) 
        {
            entity.TakeDamage(power);   // ToDo: Fix it!
        }

        MyDestroy();
    }

    public void Stop() 
    {
        if (isDead) 
        {
            return;
        }

        MyDestroy();
    }

    private void MyDestroy() 
    {
        isDead = true;

        CancelInvoke();
        StopAllCoroutines();
        HSPoolManager.Instance.RemoveItem(mpType, gameObject);
    }
}


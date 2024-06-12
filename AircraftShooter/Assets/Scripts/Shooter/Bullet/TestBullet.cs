using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestBullet : MonoBehaviour, IMemoryPool
{
    Rigidbody rigid;

    [SerializeField]
    private float lifeTime = 10f;
    [SerializeField]
    private float speed;

    private Coroutine destroyCoroutine;
    public virtual void MPStart()
    {
        this.MyStart();
    }

    protected void MyStart() 
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        if (destroyCoroutine != null) 
        {
            StopCoroutine(destroyCoroutine);
            destroyCoroutine = null;
        }
    }
    private void OnEnable() 
    {
        destroyCoroutine = StartCoroutine(DestroyDelay(lifeTime));
    }

    private void Update() 
    {
        Move();
    }

    public void Move()
    {
        rigid.velocity = transform.forward * speed;
    }

    public void Create(Vector3 _pos, Quaternion _rot, float _speed)
    {
        transform.position = _pos;
        transform.rotation = _rot;
        speed = _speed;
    }

    protected virtual void Disappear()
    {
        //MyDestroy();
    }

    private IEnumerator DestroyDelay(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        MyDestroy();
    }
    protected virtual void MyDestroy() 
    {
        HSPoolManager.Instance.RemoveItem("Bullet", this.gameObject);
    }

}

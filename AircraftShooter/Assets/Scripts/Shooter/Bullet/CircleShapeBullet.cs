using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShapeBullet : MonoBehaviour, IMemoryPool
{
    [Header("Basic")]
    [SerializeField] private string mpType;
    [SerializeField] private string hitEffect;
    [SerializeField] private int power;
    [SerializeField] private float speed;
    [SerializeField] private float angle;
    [SerializeField] private float rotateSpeed;
    public float lifeTime;
    [SerializeField] private bool showValueText;

    [SerializeField] private float radius;
    [SerializeField] private int circleAngle;
    public float angleSpeed;
    [SerializeField] Shooter.BulletType bulletType;

    [SerializeField] private Transform bulletBody;
    //[SerializeField] private ChildBullet childBullet;

    private Transform myTransform;

    private bool isDead;
    private bool isCritical;

    public void MPStart()
    {
        myTransform = transform;
    }

    public void Init(Shooter.BulletType _bulletType, float _angleSpeed, int _circleAngle, float _radius)
    {
        this.bulletType = _bulletType;
        this.angleSpeed = _angleSpeed;
        this.circleAngle = _circleAngle;
        this.radius = _radius;
    }

    public void Create(Vector3 pos, Quaternion rot, int power, float speed, float angle, bool isCritical)
    {
        myTransform.position = pos;
        myTransform.rotation = rot;
        this.power = power;
        this.speed = speed;
        this.angle = angle;
        this.isCritical = isCritical;

        isDead = false;

        CirclePatterns();

        StartCoroutine("CoUpdate");
        if (lifeTime > 0)
        {
            Invoke("MyDestroy", lifeTime);
        }
        else
        {
            Invoke("MyDestroy", 5.0f);
        }
    }

    double DegreeToRadian(double _a)
    {
        double angled = System.Math.PI * (_a / 180f);
        return angled;
    }

    void CirclePatterns()
    {
        for (int i = 0; i < 360; i += circleAngle)
        {
            ChildBullet bullet = HSPoolManager.Instance.NewItem<ChildBullet>(bulletType.ToString());
            if (bullet)
            {
                bullet.ParentBullet = this;
                double angleRad = DegreeToRadian(i);
                float pointx = Mathf.Cos((float)angleRad) * radius;
                float pointz = Mathf.Sin((float)angleRad) * radius;
                Vector3 projposition = new Vector3(pointx, 0, pointz);
                bullet.transform.position = projposition;
                //bullet.transform.SetParent(this.transform);
                bullet.transform.parent = this.transform;
            }

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

        //angle += angleSpeed;
    }

    public void RunChildrenTriggerEnter(Collider other)
    {
        LivingEntity entity = other.GetComponent<LivingEntity>();
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

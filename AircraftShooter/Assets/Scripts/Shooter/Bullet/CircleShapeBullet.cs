using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShapeBullet : MonoBehaviour, IMemoryPool
{
    [Header("Basic")]
    [SerializeField] private string mpType;
    [SerializeField] private int power;
    [SerializeField] private float speed;
    [SerializeField] private float angle;
    [SerializeField] private float angleRate;
    [SerializeField] private float rotateSpeed;
    public float lifeTime;

    [SerializeField] private int vertexShape;
    [SerializeField] private float radius;
    [SerializeField] private int circleAngle;
    [SerializeField] private int segments;
    public float angleSpeed;

    [SerializeField] private Consts.ShapeType shapeType;

    float[] cosAngles;
    float[] sinAngles;

    private Transform myTransform;


    private bool isDead;

    public void MPStart()
    {
        myTransform = transform;
    }

    public void Init(float _angleSpeed, int _circleAngle, float _radius, int _vertexShape, int _segments)
    {
        this.angleSpeed = _angleSpeed;
        this.circleAngle = _circleAngle;
        this.radius = _radius;
        this.vertexShape = _vertexShape;
        this.segments = _segments;
    }

    public void Create(Consts.ShapeType type, Vector3 pos, Quaternion rot, int power, float speed, float angle, float angleRate, float lifeTime)
    {
        myTransform.position = pos;
        myTransform.rotation = rot;
        this.shapeType = type;
        this.power = power;
        this.speed = speed;
        this.angle = angle;
        this.angleRate = angleRate;
        this.lifeTime = lifeTime;

        isDead = false;

        if(shapeType == Consts.ShapeType.Circle)
        {
            CirclePatterns();
        }
        else if(shapeType == Consts.ShapeType.Polygon)
        {
            PolygonPattern(vertexShape);
        }
        

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
            double angleRad = DegreeToRadian(i);
            float pointx = Mathf.Cos((float)angleRad) * radius;
            float pointz = Mathf.Sin((float)angleRad) * radius;
            Vector3 projposition = new Vector3(pointx, 0, pointz);
            CreateChildShapeBullet(projposition);
        }
    }

    void PolygonPattern(int sides)
    {
        List<Vector3> vertices = new List<Vector3>();
        ChildAngle(sides);
        // 기본 꼭짓점 생성
        for (int i = 0; i < sides; i++)
        {
            float pointx = cosAngles[i] * radius;
            float pointz = sinAngles[i] * radius;
            Vector3 projposition = new Vector3(pointx, 0, pointz);
            vertices.Add(projposition);
            CreateChildShapeBullet(projposition);
        }

        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 start = vertices[i];
            Vector3 end = vertices[(i + 1) % vertices.Count];

            for (int j = 1; j < segments; j++)
            {
                float t = j / (float)segments;
                Vector3 point = Vector3.Lerp(start, end, t);
                CreateChildShapeBullet(point);
            }
        }
    }

    void ChildAngle(int _sides)
    {
        cosAngles = new float[_sides];
        sinAngles = new float[_sides];
        for (int i = 0; i < _sides; i++)
        {
            float angleRad = 2 * Mathf.PI * i / _sides;
            cosAngles[i] = Mathf.Cos(angleRad);
            sinAngles[i] = Mathf.Sin(angleRad);
        }
    }

    void CreateChildShapeBullet(Vector3 _point)
    {
        ChildBullet bullet = HSPoolManager.Instance.NewItem<ChildBullet>(Consts.BulletType.ChildBullet.ToString());

        if (bullet && bullet.gameObject.activeSelf)
        {
            bullet.ParentBullet = this;
            bullet.Create(_point, this.transform, angleSpeed, lifeTime);
            bullet.transform.parent = this.transform;
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

        angle += angleRate;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcBullet : Bullet, IMemoryPool {
    public enum Motion {
        Slerp,
        Parabola,
        Parabolic,
    };

    [Header("Basic")]
    [SerializeField] private Motion motionType;
    [SerializeField] private float Arcspeed;
    [SerializeField] private float height;
    [SerializeField] private int damage = 10;
    //[SerializeField] private int obstacleDamage = 1;
    [SerializeField] private float arrivalTime;
    [SerializeField] private float waitTime;
    [SerializeField] private float attackTime = 0.1f;
    //[SerializeField] private string mpType = "";
    //[SerializeField] private GameObject bullet;
    [SerializeField] private string targetTag = "Player";


    //private Transform myTransform;
    private SphereCollider sphereCollider;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 startRelCenter;
    private Vector3 endRelCenter;
    private Vector3 center;
    //private bool isCritical;
    private float gravity;

    // arc
    [Header("Parabola")]
    [SerializeField] private float maxHeight = 10.0f;
    // [SerializeField] private float maxHeightTime = 1.0f;
    private Vector3 velocity;
    private float endTime;

    protected override void MyStart() 
    {
        base.MyStart();
        myTransform = transform;
        sphereCollider = GetComponent<SphereCollider>();
    }
    

    public void Create(Vector3 startPos, Vector3 endPos, int damage, float arrivalTime = 0, float height = 1, bool isCritical = false) {
        myTransform.position = startPos;
        this.startPos = startPos;
        this.endPos = endPos;
        this.damage = damage;
        this.arrivalTime = arrivalTime;
        this.maxHeight = height;
        this.isCritical = isCritical;
        this.Arcspeed = speed;

        //bullet.SetActive(false);
        sphereCollider.enabled = false;

        switch (motionType) {
            case Motion.Slerp:
                RunSlerpMotion();
                break;
            case Motion.Parabola:
                RunParabolaMotion();
                break;
            case Motion.Parabolic:
                RunParabolicMotion();
                break;
        }
    }

    private void RunSlerpMotion() {
        center = (startPos + endPos) * 0.5f;
        center -= new Vector3(0, maxHeight, 0);
        startRelCenter = startPos - center;
        endRelCenter = endPos - center;

        StartCoroutine("CoSlerpShoot");
    }

    private void RunParabolaMotion() {
        float dHeight = endPos.y - startPos.y;
        float height = maxHeight - startPos.y;

        if (height < dHeight) {
            height += dHeight;
        }

        gravity = 2 * height / (arrivalTime * arrivalTime);

        velocity.y = Mathf.Sqrt(2 * gravity * height);

        float a = gravity;
        float b = -2 * velocity.y;
        float c = 2 * dHeight;

        endTime = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);

        velocity.x = -(startPos.x - endPos.x) / endTime;
        velocity.z = -(startPos.z - endPos.z) / endTime;

        StartCoroutine("CoParabolaShoot");
    }

    IEnumerator CoSlerpShoot() {
        float percent = 0;
        float moveRate = 1 / arrivalTime;

        yield return new WaitForSeconds(waitTime);

        //bullet.SetActive(true);

        while (percent < 1) 
        {
            percent += Time.deltaTime * moveRate;
            myTransform.position = Vector3.Slerp(startRelCenter, endRelCenter, percent);
            myTransform.position += center;

            yield return null;
        }

        //bullet.SetActive(false);


        // check collider
        sphereCollider.enabled = true;

        yield return new WaitForSeconds(attackTime);

        sphereCollider.enabled = false;

        MyDestroy();
    }

    IEnumerator CoParabolaShoot() {
        float time = 0;
        Vector3 pos = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(waitTime);

        //bullet.SetActive(true);

        while (time < endTime) {
            time += Time.deltaTime;

            pos.x = startPos.x + velocity.x * time;
            pos.y = startPos.y + velocity.y * time - 0.5f * gravity * time * time;
            pos.z = startPos.z + velocity.z * time;

            myTransform.position = pos;

            yield return null;
        }

        //bullet.SetActive(false);

        // check collider
        sphereCollider.enabled = true;

        yield return new WaitForSeconds(attackTime);

        sphereCollider.enabled = false;

        MyDestroy();
    }

    private void RunParabolicMotion() {
        StartCoroutine(CoParabolicShoot());
    }

    private IEnumerator CoParabolicShoot() {
        float distance = Vector3.Distance(endPos, startPos);
        float percent = 0;

        Debug.Log(endPos + " + " + startPos + " = " + distance);
        yield return new WaitForSeconds(waitTime);

        //bullet.SetActive(true);

        while (percent < 1f) {
            percent += Time.deltaTime * Arcspeed;
            float x = Mathf.Lerp(0, distance, percent);
            float y = height * Mathf.Sin(Mathf.Clamp01(percent) * Mathf.PI);
            myTransform.position = Vector3.Lerp(startPos, endPos, x / distance) + Vector3.up * y;
            yield return null;
        }

        //bullet.SetActive(false);

        sphereCollider.enabled = true;
        yield return new WaitForSeconds(attackTime);
        sphereCollider.enabled = false;

        MyDestroy();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag(targetTag) || other.CompareTag("Obstacle")) {
            LivingEntity damagableObject = other.GetComponent<LivingEntity>();
            if (damagableObject != null) {
                damagableObject.TakeDamage(damage, obstacleDamage, isCritical, true);
            }
        }
    }

    // private void MyDestroy() 
    // {
    //     HSPoolManager.Instance.RemoveItem(mpType, gameObject);
    // }
}
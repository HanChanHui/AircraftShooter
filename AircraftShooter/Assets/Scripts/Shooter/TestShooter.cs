using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShooter : MonoBehaviour
{
    [Header("Directional")]
    [SerializeField]
    private float angle;
    [SerializeField]
    private float angleRate;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float speedRate;
    private float time;

    

    private void Update() 
    {
        time += Time.deltaTime;
        
        if(time >= speedRate)
        {
            time = 0f;
            
            Directional();
        }
        
    }

    private void Directional()
    {
        TestBullet bullet = HSPoolManager.Instance.NewItem<TestBullet>("Bullet");

        if(bullet)
        {
            Quaternion rot = Quaternion.Euler(transform.rotation.x, angle, transform.rotation.z);
            bullet.Create(transform.position, rot, speed);
        }
        
    }
}

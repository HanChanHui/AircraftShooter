using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBullet : MonoBehaviour
{
     public ShapeBullet ParentBullet { get; set;}

    private void OnTriggerEnter(Collider other)
    {
        ParentBullet.RunChildrenTriggerEnter(other);
    }
}

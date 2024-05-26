using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform target;
    public float speed = 5;

    void Update()
    {

        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);
    }
}


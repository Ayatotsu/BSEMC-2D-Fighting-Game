using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float positionShakeSpeed = 0.1f;
    public Vector3 positionShakeRange = new Vector3(0.1f, 0.1f, 0.1f);

    private Vector3 position;

    void Start()
    {
        position = transform.localPosition;
    }

    void Update()
    {
        if (positionShakeSpeed > 0)
        {
            transform.localPosition = position + Vector3.Scale(SmoothRandom.GetVector3(positionShakeSpeed), positionShakeRange);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform targetCamera;

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (targetCamera == null) return;

        Vector3 direction = targetCamera.position - transform.position;
        direction.y = 0f;
        transform.forward = -direction.normalized;
    }
}

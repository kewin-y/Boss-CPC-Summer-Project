using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 offset;

    // Start is called before the first frame update
    private void Start() {

    }

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.position = playerTransform.position + offset;
    }
}

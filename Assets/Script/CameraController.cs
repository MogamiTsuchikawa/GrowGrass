using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float cameraMoveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A)) transform.position -= Vector3.right * cameraMoveSpeed;
        if (Input.GetKey(KeyCode.D)) transform.position += Vector3.right * cameraMoveSpeed;
        if (Input.GetKey(KeyCode.W)) transform.position += Vector3.forward * cameraMoveSpeed;
        if (Input.GetKey(KeyCode.S)) transform.position -= Vector3.forward * cameraMoveSpeed;

    }
}

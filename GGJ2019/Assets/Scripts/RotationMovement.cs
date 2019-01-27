using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMovement : MonoBehaviour
{
    public float rotationSpeed = 5f;
    private float rotationCurrent = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotationCurrent += rotationSpeed;
        transform.Rotate(new Vector3(1, 1, 1), rotationCurrent);
    }
}

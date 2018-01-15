using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateText : MonoBehaviour {

    Vector3 direction;

    void Update()
    {
        direction = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        transform.rotation = Quaternion.LookRotation(direction);
    }
}

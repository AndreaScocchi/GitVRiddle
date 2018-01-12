using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation: MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
    }
}

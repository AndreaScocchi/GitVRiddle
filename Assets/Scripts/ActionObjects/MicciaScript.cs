using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MicciaScript : MonoBehaviour {

    public UnityEvent Shot;
    public float TimeToShot = 5.0f;
    private float realTimeToShot;
    private bool started = false;

	void Start () {
        realTimeToShot = TimeToShot;
	}
	
	void Update () {
		if (started)
        {
            realTimeToShot -= Time.deltaTime;
            if (realTimeToShot < 0)
            {
                Shot.Invoke();
                realTimeToShot = TimeToShot;
                started = false;
                GetComponent<Torchelight>().StopLight();
            }
        }
	}

    public void StartCountdown ()
    {
        started = true;
        GetComponent<Torchelight>().StartLight();
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    public GameObject cannon_ball;
    public GameObject cannon_ball_start_point;
    public float force = 1.0f;
    private bool shooted;

    private void Start()
    {
        shooted = false;
    }

    public void Shoot()
    {
        //if (shooted)
        //    return;

        GameObject cannonball = GameObject.Instantiate(cannon_ball, cannon_ball_start_point.transform);
        cannonball.GetComponent<Rigidbody>().AddForce(cannon_ball_start_point.transform.forward * force);
        shooted = true;
    }
    
}

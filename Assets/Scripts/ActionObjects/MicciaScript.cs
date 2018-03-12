using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MicciaScript : MonoBehaviour {

    //Variabili pubbliche
    public GameObject cannon_ball;
    public GameObject cannon_ball_start_point;
    public float force = 1000.0f;
    public float TimeToShot = 5.0f;

    //Variabili private
    private bool shooted;
    private float realTimeToShot;
    private bool started;
    private Torchelight torche;
    
    void Start () {
        torche = GetComponent<Torchelight>();
        resetCannon();
    }

    //Update -> Gestisce il countdown se partito
    void Update()
    {
        if (started)
        {
            realTimeToShot -= Time.deltaTime;
            if (realTimeToShot < 0)
            {
                Shot();
                resetCannon();
            }
        }
    }

    //Spara la palla di cannone
    private void Shot()
    {
        GameObject cannonball = GameObject.Instantiate(cannon_ball, cannon_ball_start_point.transform);
        cannonball.GetComponent<Rigidbody>().AddForce(cannon_ball_start_point.transform.forward * force);
        shooted = true;
    }

    //Resetta il tutto
    private void resetCannon()
    {
        realTimeToShot = TimeToShot;
        started = false;
        torche.StopLight();
    }
    
    //Metodo generale di utilizzo oggetto -> Fa partire il countdown e accende la miccia
    public void UseObject()
    {
        if (started)
            return;

        started = true;
        torche.StartLight();
    }
}

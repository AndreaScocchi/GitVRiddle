using UnityEngine;
using System.Collections;

public class Torchelight : MonoBehaviour {
	
	public GameObject TorchLight;
	public GameObject MainFlame;
	public GameObject BaseFlame;
	public GameObject Etincelles;
	public GameObject Fumee;
	public float MaxLightIntensity;
	public float IntensityLight;

    public bool AlwaysOn = true;
    private bool lightOn = false;
	

	void Start () {
        if (AlwaysOn)
        {
            lightOn = true;
            TorchLight.GetComponent<Light>().intensity = IntensityLight;
            MainFlame.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 20f;
            BaseFlame.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 15f;
            Etincelles.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 7f;
            Fumee.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 12f;
        } 
	}
	

	void Update () {
        if (lightOn)
        {
            if (IntensityLight < 0) IntensityLight = 0;
            if (IntensityLight > MaxLightIntensity) IntensityLight = MaxLightIntensity;

            TorchLight.GetComponent<Light>().intensity = IntensityLight / 2f + Mathf.Lerp(IntensityLight - 0.1f, IntensityLight + 0.1f, Mathf.Cos(Time.time * 30));

            TorchLight.GetComponent<Light>().color = new Color(Mathf.Min(IntensityLight / 1.5f, 1f), Mathf.Min(IntensityLight / 2f, 1f), 0f);
            MainFlame.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 20f;
            BaseFlame.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 15f;
            Etincelles.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 7f;
            Fumee.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 12f;
        }
	}

    public void StartLight()
    {
        lightOn = true;
        TorchLight.GetComponent<Light>().intensity = IntensityLight;
        MainFlame.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 20f;
        BaseFlame.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 15f;
        Etincelles.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 7f;
        Fumee.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 12f;
    }

    public void StopLight()
    {
        lightOn = false;
        TorchLight.GetComponent<Light>().intensity = 0;
        MainFlame.GetComponent<ParticleSystem>().emissionRate = 0;
        BaseFlame.GetComponent<ParticleSystem>().emissionRate = 0;
        Etincelles.GetComponent<ParticleSystem>().emissionRate = 0;
        Fumee.GetComponent<ParticleSystem>().emissionRate = 0;
    }
}

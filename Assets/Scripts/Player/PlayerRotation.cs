using UnityEngine;

public class PlayerRotation: MonoBehaviour {

    public bool RotateX = false;
    private float XRotation = 0f;

    void Update () {
        if (RotateX)
        {
            XRotation = Camera.main.transform.eulerAngles.x;
            if (XRotation > 50 && XRotation < 90)
            {
                XRotation = 50f;
            }
            transform.rotation = Quaternion.Euler(XRotation, Camera.main.transform.eulerAngles.y, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        }
    }
}
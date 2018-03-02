using UnityEngine;
using UnityEngine.XR;

public class VRButtonHandle : MonoBehaviour {

    public float moveSpeed = 1.5f;
    public GameObject RightHandObject; 

    private Ray look;
    private RaycastHit hit;

    private bool ActionPresent = false;
    private bool TakeObjectPresent = false;
    private bool pressed = false;

    public object VRnode { get; private set; }

    void Update () {
        if (Input.GetMouseButton(0))
        {
            if (!pressed)
            {
                //look = Camera.main.ScreenPointToRay(Input.mousePosition);
                look = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                if (Physics.Raycast(look, out hit, 2.0F))
                {
                    if (hit.transform.tag == "Action")
                    {
                        ActionPresent = true;
                    }
                    if (hit.transform.tag == "TakeObject")
                    {
                        hit.transform.position = RightHandObject.transform.position;
                        hit.transform.rotation = RightHandObject.transform.rotation;
                        hit.transform.parent = RightHandObject.transform;
                        TakeObjectPresent = true;
                    }
                } 
            }
            if (!ActionPresent)
            {
                transform.position += new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * Time.deltaTime * moveSpeed;
            }
            pressed = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            pressed = false;
            ActionPresent = false;
            TakeObjectPresent = false;
        }
    }
}
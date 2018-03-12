using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class VRButtonHandle : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float maxIterationDistance = 2.0F;
    public GameObject RightHandObject;
    public Animator RightArmAnimator;

    private Ray look;
    private RaycastHit hit;
    private ActionObjectScript ObjectScript;

    //Stati
    private bool ActionPresent;
    private bool pressed;
    private ActionObjectScript RightObject;

    private void Start()
    {
        //Inizializzazioni
        ActionPresent = false;
        pressed = false;
        RightObject = null;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!pressed)
            {
                //Controlla la presenza di un oggetto iterattivo in direzione del puntatore entro la distanza "maxIterationDistance"
                look = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                if (Physics.Raycast(look, out hit, maxIterationDistance))
                {
                    if (hit.transform.tag.Equals("Action"))
                    {
                        ActionPresent = true;
                        IteractWithObject(hit.transform);
                    }
                }
            }
            //Se non c'è un azione mi muovo in avanti
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
        }
    }

    private void IteractWithObject(Transform obj)
    {
        ObjectScript = obj.GetComponent<ActionObjectScript>();
        switch (ObjectScript.getObjectType())
        {
            case ActionObjectScript.ObjType.Action:
            {
                UseObject();
                break;
            }
            case ActionObjectScript.ObjType.TakeObject:
            {
                TakeObject();
                break;
            }
        }
    }

    private void UseObject()
    {
        if (ObjectScript != null && (ObjectScript.getRequiredObject().Equals("") || ObjectScript.getRequiredObject().Equals(RightObject.getObjectName())))
        {
            StartCoroutine(UseWithDelay());
        }
        else
        {
            ObjectScript.ShowInfoText();
        }
    }

    private void TakeObject()
    {
        if (RightObject == null)
        {
            ObjectScript.TakeObject(RightHandObject.transform);

            //Alza il braccio destro di Furiog con animazione
            RightArmAnimator.SetBool("UP", true);

            RightObject = ObjectScript;
        }
        else
        {
            StartCoroutine(DropWithDelay());
        }
    }

    IEnumerator UseWithDelay()
    {
        RightArmAnimator.SetBool("USE", true);
        yield return new WaitForSeconds(0.7f);
        ObjectScript.UseObject();
        RightArmAnimator.SetBool("USE", false);
        yield return new WaitForSeconds(0.7f);
        RightArmAnimator.SetBool("UP", false);
        yield return new WaitForSeconds(1.0f);
        RightObject.SetUsed();
        RightObject = null;
    }

    IEnumerator DropWithDelay()
    {
        RightObject.DropObject();
        yield return new WaitForSeconds(0.2f);
        ObjectScript.TakeObject(RightHandObject.transform);
        RightObject = ObjectScript;
    }
}
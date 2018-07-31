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
        //Se non serve nessun oggetto per l'azione
        if (ObjectScript.getRequiredObject().Equals(""))
        {
            ObjectScript.UseObject();
        }
        //Se serve un oggetto
        else
        {
            //Se ho in mano l'oggetto giusto -> Lo uso
            if (ObjectScript.getRequiredObject().Equals((RightObject == null ? "" : RightObject.getObjectName())))
            {
                StartCoroutine(UseWithObject());
            }
            //Altrimenti mostro il testo per dire che oggetto mi serve
            else
            {
                ObjectScript.ShowInfoText();
            }
        }
    }

    private void TakeObject()
    {
        //Se non ho niente in mano lo prendo e alzo il braccio
        if (RightObject == null)
        {
            ObjectScript.TakeObject(RightHandObject.transform);
            RightArmAnimator.SetBool("UP", true);
            RightObject = ObjectScript;
        }
        //Altrimenti chiamo la routine temporizzata per droppare il vecchio oggetto e prendere il nuovo
        else
        {
            StartCoroutine(DropWithDelay());
        }
    }
    
    //Routine per effettuare un azione con oggetto e animazione
    IEnumerator UseWithObject()
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

    //Routine per droppare oggetto con animazione
    IEnumerator DropWithDelay()
    {
        RightObject.DropObject();
        yield return new WaitForSeconds(0.2f);
        ObjectScript.TakeObject(RightHandObject.transform);
        RightObject = ObjectScript;
    }
}
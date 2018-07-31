using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Per un oggetto iterabile sono richiesti:
/// - Tag "Action"
/// - Collider
/// - Oggetto EventTrigger con OnPointerEnter che punta a this.OnPointerEnter e uguale per OnPointerExit
/// - Questo script con tutti i dati pubblici popolati
/// </summary>
public class ActionObjectScript : MonoBehaviour {

    public enum ObjType { Action, TakeObject };
    
    // Variabili pubbliche 
    public ObjType ObjectType;
    public string ObjectName;
    public string RequiredObject;
    public string MainText;
    public GameObject MainTextObj;
    public GameObject InfoTextObj;
    public float DropForce = 4.0f;
    public UnityEvent Use;
    
    //Variabili private
    Vector3 textDirection;

    // Metodo chiamato quando un oggetto viene cliccato 
    public void UseObject()
    {
        Use.Invoke();
    }

    // GETTER --------------------------------------------
    public ObjType getObjectType()
    {
        return ObjectType;
    }
    public string getObjectName()
    {
        return ObjectName;
    }
    public string getRequiredObject()
    {
        return RequiredObject;
    }
    //---------------------------------------------------

    private void Start()
    {
        MainTextObj.GetComponent<TextMesh>().text = MainText;
        if (!RequiredObject.Equals(""))
        {
            InfoTextObj.GetComponent<TextMesh>().text = "Needed:" + RequiredObject;
        }
        else
        {
            InfoTextObj.GetComponent<TextMesh>().text = "";
        }
        MainTextObj.SetActive(false);
        InfoTextObj.SetActive(false);
    }

    // Aggiorna la rotazione del testo per mostrarla sempre di fronte a Furiog
    private void Update()
    {
        textDirection = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        MainTextObj.transform.rotation = Quaternion.LookRotation(textDirection);
        InfoTextObj.transform.rotation = Quaternion.LookRotation(textDirection);
    }

    // Mostra l'Info Text
    public void ShowInfoText()
    {
        if (transform.tag.Equals("Action"))
        {
            InfoTextObj.SetActive(true);
        }
    }

    // Oggetto puntato
    public void OnPointerEnter()
    {
        if (transform.tag.Equals("Action"))
        {
            MainTextObj.SetActive(true);
        }
    }

    // Oggetto non più puntato
    public void OnPointerExit()
    {
        if (transform.tag.Equals("Action"))
        {
            MainTextObj.SetActive(false);
            InfoTextObj.SetActive(false);
        }
    }

    public void TakeObject(Transform FuriogHand)
    {
        RemoveGravity();
        //Sposta l'oggetto selezionato nella mano destra di Furiog
        transform.position = FuriogHand.position;
        transform.rotation = FuriogHand.rotation;
        transform.parent = FuriogHand;
    }
    public void DropObject()
    {
        transform.parent = null;
        SetGravity();
        transform.GetComponent<Rigidbody>().AddForce(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * DropForce, ForceMode.Impulse);
    }

    public void SetUsed()
    {
        Torchelight torcheScript = transform.GetComponent<Torchelight>();
        if (torcheScript != null)
        {
            StartCoroutine(SwitchOffBeforeSetUsed(torcheScript));
        }
        else
        {
            DropObject();
            transform.tag = "Untagged";
        }
    }
    IEnumerator SwitchOffBeforeSetUsed(Torchelight torche)
    {
        torche.StopLight();
        yield return new WaitForSeconds(1.0f);
        DropObject();
        transform.tag = "Untagged";
    }
    
    public void DestroyObject()
    {
        Torchelight torcheScript = transform.GetComponent<Torchelight>();
        if (torcheScript != null)
        {
            StartCoroutine(SwitchOffBeforeDestroy(torcheScript));
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    IEnumerator SwitchOffBeforeDestroy(Torchelight torche)
    {
        torche.StopLight();
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }

    public void SetGravity()
    {
        transform.GetComponent<Rigidbody>().useGravity = true;
        transform.GetComponent<BoxCollider>().isTrigger = false;
    }
    public void RemoveGravity()
    {
        transform.GetComponent<Rigidbody>().useGravity = false;
        transform.GetComponent<BoxCollider>().isTrigger = true;
    }
}

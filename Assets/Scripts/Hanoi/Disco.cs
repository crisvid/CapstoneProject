using OculusSampleFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Disco : MonoBehaviour
{
    public Vector3 posicionOriginal; //Posición del disco cuando reposa en un poste
    public bool seMueve = false; //Indica si el disco puede moverse o no
    public GameObject posteInicialGO; //Poste al que regresa el disco cuando se hace un movimiento no válido
    private Poste posteInicialScript; //Script de posteInicialGO
    public GameObject posteFinalGO; //Poste al que se movió el disco cuando se hace un movimiento válido
    private Poste posteFinalScript; //Script de posteFinalGO
    public GameObject posteActual; //Poste temporal en el que se encuentra el disco actualmente
    public int movimientos; //Número de veces que se ha movido el disco (movimientos válidos)
    private OVRGrabbable _OVRGrabbable; //Script que habilita mover el disco a través de VR

    private void Start()//Start is called before the first frame update
    {
        movimientos = 0; //El juego inicia con el contador de movimientos en 0
        posicionOriginal = transform.position; //Todos los discos están apilados en el poste origen por orden de tamaño
        posteInicialGO = GameObject.Find("origen"); //El poste inicial de todos los discos al iniciar el juego es el poste origen
        posteInicialScript = posteInicialGO.GetComponent<Poste>();
        _OVRGrabbable = GetComponent<OVRGrabbable>();
    }

    private void Update()//Update is called once per frame
    {
        //Si el disco puede moverse se habilita dicho movimiento a través de VR
        if (seMueve)
        {
            _OVRGrabbable.grabPoints = new Collider[1];
            _OVRGrabbable.grabPoints.SetValue(GetComponent<BoxCollider>(), 0);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        else
        {
            _OVRGrabbable.grabPoints = new Collider[0];
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        }

        //Proceso de cambio de poste
        if (!posteFinalGO.name.Equals(posteInicialGO.name))
        {
            if (posteFinalScript.existeDisco(gameObject))
            {
                //
                if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)) //Solo se cuentan movimientos cuando el jugador suelta el disco
                {
                    Debug.Log("No se cuenta este movimiento");
                }
                else
                {
                    Debug.Log(gameObject.name + " cambió de poste");
                    posteInicialScript.discos.Remove(this.gameObject);
                    posteInicialGO = posteFinalGO;
                    posteInicialScript = posteFinalScript;
                    posicionOriginal = transform.position;
                    movimientos++; //Cuando un disco se mueve de un poste a otro se cuenta 1 movimiento 

                }
            }
            else
            {
                transform.position = posicionOriginal;
            }
        }
    }

    private void OnCollisionEnter(Collision other)//Se llama a este método cuando el disco ha comenzado a tocar otro cuerpo rígido o collider
    {
        if ((other.gameObject.isStatic && other.gameObject.tag == "piso") || posteActual == null)//Verifica que el disco solo pueda ser colocado en postes
        {
            transform.position = posicionOriginal;//Si el disco no es colocados en postes regresa a su posición original
        }
    }

    private void OnTriggerStay(Collider other)//Se llama a este método cada vez que el disco toca otro Collider
    {
        if (other.gameObject.tag.Equals("poste"))//Se verifica que el disco repose en un poste
        {
            posteFinalGO = other.gameObject;
            posteFinalScript = posteFinalGO.GetComponent<Poste>();
            transform.position = new Vector3(
                    other.gameObject.transform.position.x,
                    this.transform.position.y,
                    other.gameObject.transform.position.z);//Centra el disco en el poste donde reposa
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            posteActual = other.gameObject;//posteActual es asignado al poste que el disco toca actualmente 
        }
    }

    private void OnTriggerExit(Collider other)//Se llama a este método cuando el disco deja de tocar otro Collider
    {
        posteActual = null; //posteActual es nulo cuando el disco no toca ningún poste
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flechas : MonoBehaviour
{
    public GameObject flechaAdelante;   
    public GameObject flechaDerecha;
    public GameObject flechaIzquierda;
    public GameObject flechaAtras;
    public PlayerIA bot;    //Objeto que obtiene el estado del bot
    public Material red;    //Material rojo
    public Material green;  //Material Verde
    // Start is called before the first frame update
    void Start()
    {
        

        reiniciar();

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = bot.transform.position;
    }

    //Dependiendo de la dirección del Bot pintan la flecha de color verde
    public void Derecha()
    {
        flechaDerecha.GetComponent<MeshRenderer>().material = green;
    }
    public void Adelante()
    {
        flechaAdelante.GetComponent<MeshRenderer>().material = green;
    }
    public void Izquierda()
    {
        flechaIzquierda.GetComponent<MeshRenderer>().material = green;
    }
    public void Atras()
    {
        flechaAtras.GetComponent<MeshRenderer>().material = green;
    }

    //Se pinta de color rojo la flecha en la dirreción que el bot esta analiando

    public void AnalizarDerecha()
    {
        flechaDerecha.GetComponent<MeshRenderer>().enabled = true;
    }
    public void AnalizarAdelante()
    {
        flechaAdelante.GetComponent<MeshRenderer>().enabled = true;
    }
    public void AnalizarAtras()
    {
        flechaAtras.GetComponent<MeshRenderer>().enabled = true;
    }
    public void AnalizarIzquierda()
    {
        flechaIzquierda.GetComponent<MeshRenderer>().enabled = true;
    }

    //Reinicia el estado de las flechas
    public void reiniciar()
    {
        flechaAdelante.GetComponent<MeshRenderer>().enabled = false;
        flechaDerecha.GetComponent<MeshRenderer>().enabled = false;
        flechaIzquierda.GetComponent<MeshRenderer>().enabled = false;
        flechaAtras.GetComponent<MeshRenderer>().enabled = false;

        flechaAdelante.GetComponent<MeshRenderer>().material = red;
        flechaDerecha.GetComponent<MeshRenderer>().material = red;
        flechaIzquierda.GetComponent<MeshRenderer>().material = red;
        flechaAtras.GetComponent<MeshRenderer>().material = red;

        transform.rotation = bot.transform.rotation;
    }
}

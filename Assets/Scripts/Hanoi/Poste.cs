using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Poste : MonoBehaviour
{
    public List<GameObject> discos; //Lista que representa los discos apilados en este poste
    public float discoMenorTamX; //Disco de menor diámetro en la lista de discos apilados en este poste 

    void Start()// Start is called before the first frame update
    {
        discoMenorTamX = 8;
    }

    void Update()// Update is called once per frame
    {
        if (discos.Count != 0)
        {
            encontrarTamMenor();
        }
        else
        {
            discoMenorTamX = 8;
        }
    }

    private void OnTriggerEnter(Collider other)//Se llama a este método cuando un GameObject choca con otro GameObject
    {
        if (other.tag.Equals("disco"))//Verifica si se insertó un disco en el poste
        {
            if (!existeDisco(other.gameObject) && esMenor(other.transform.localScale.x))
            {
                discos.Add(other.gameObject);//No pueden repetirse discos en la pila y el disco insertado no puede ser de mayor diámetro que los de la pila en este poste
            }
        }
    }

    public bool existeDisco(GameObject compararGO)//Encuentra un disco en la ista de discos apilados en este poste 
    {
        foreach (GameObject disco in discos)
        {
            if (disco.name.Equals(compararGO.name))
            {
                return true;
            }
        }
        return false;
    }

    private bool esMenor(float tamX)//Comprueba si un disco es de menor diámetro que discoMenorTamX
    {
        if (tamX < discoMenorTamX)
        {
            return true;
        }
        return false;
    }

    public float encontrarTamMenor()//Encuentra el disco de menor diámetro en la lista de discos apilados en este poste 
    {
        discoMenorTamX = discos[0].transform.localScale.x;
        foreach (GameObject disco in discos)
        {
            if (disco.transform.localScale.x < discoMenorTamX)
            {
                discoMenorTamX = disco.transform.localScale.x;
            }
        }
        return discoMenorTamX;
    }

    private void OnTriggerExit(Collider other)//Se llama a este método cuando el poste deja de tocar otro Collider
    {
        if (other.tag.Equals("disco"))
        {
            if (!other.gameObject.GetComponent<Disco>().posteInicialGO.name.Equals(gameObject.name))
            {
                discos.Remove(other.gameObject);//Elimina disco de tamaño mayor a discoMenorTamX
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class HighlightLine : MonoBehaviour
{
    public Color highlightColor;    //Color con el que se reslta la línea del código
    public int line;    //Indica el número de línea que se esta resltaltando
    private int tmpLine;    //Guarda de manera temporal la línea que se esta resltando
    public TextMeshProUGUI highlightText;   //Texto que se utiliza para mostrar la llínea resltada
    public string espacio;  //Tamaño del resaltador


    // Start is called before the first frame update
    void Start()
    {
        tmpLine = line;
    }

    // Update is called once per frame
    void Update()
    {
        if (tmpLine != line)    //Ingresa sí el valor de la línea cambia
        {
            highlightText.text = "";
            for (int i = 1; i <= line; i++) //For que avanaza hasta la línea seleccionada
            {
                if (i == line)  //Ingresa cuando i llega al valor de la línea seleccionada
                {
                    highlightText.text = highlightText.text +
                        $"<mark={ToHex(highlightColor)}>" + espacio + "</mark><color=#FF0600><=</color>";   //Resltada la línea
                }
                highlightText.text = highlightText.text + "\n"; //Imprime un salto de línea por cada iteración
            }

            tmpLine = line;
        }

    }

    public static string ToHex(Color c) => "#" + ColorUtility.ToHtmlStringRGBA(c);  //Método para convertir el color seleccionado en Hexadecimal 

}


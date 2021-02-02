using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SierpinskiTriangleScript : MonoBehaviour
{
    public GameObject buscoGO; //Pirámide original para crear el fractal 
    public int grado = 0; //El "grado" del fractal es el número de veces que queremos dividir el triángulo en pedazos. 
    public int limite; //Grado máximo permitido
    public Text gradoTxt; //Caja de texto para mostrar el grado
    private int numClon; //AL crear nuevas pirámides para formar el fractal se usa esta variable para nombrarlas y distinguirlas
    public Toggle toggle; //Permite seleccionar un modo: debug si está encendido y juego si no
    public GameObject debugUI; //Ventana de debug
    public GameObject instruccionesGO; //Ventana de instrucciones del juego
    public GameObject animacionGO; //Pirámides con animación para mostrar la ejecución del juego en el modo debug
    public DebugTriangulo animacion; //Script que controla el debug
    public string[,] parametros; //Arreglo que guarda el valor de los parámetros para el debug por cada paso
    public int[] lineas; //Arreglo que guarda las líneas de código que se resaltarán el debug por cada paso

    void Start() // Start is called before the first frame update
    {
        gradoTxt.text = grado.ToString();
        parametros = new string[79, 3];
        lineas = new int[79];
        sierpinskiPasos(buscoGO, new Vector3(0, 0, 0), buscoGO.transform.localScale, 0);//Llama a la función recursiva para generar los pasos del debug
    }

    public void iniciarJuago()//Al aumentar o disminuir el grado se vuelve a crear el fractal de acuerdo a ese grado
    {
        destruirTriangulos();
        encenderTrianguloOriginal();
        numClon = 0;
        sierpinski(buscoGO, new Vector3(0, 0, 0), buscoGO.transform.localScale, grado);
        apagarTrianguloOriginal();
    }

    public void sierpinski(GameObject original, Vector3 posicion, Vector3 escala, int grado)//Algoritmo recursivo que crea el fractal
    {
        original = clonarTriangulo(original, posicion, escala);
        numClon++;
        if (grado > 0)
        {
            float x = original.transform.position.x;
            float y = original.transform.position.y;
            float z = original.transform.position.z;

            sierpinski(original, original.transform.position, escala / 2, grado - 1);
            sierpinski(original, new Vector3(x + escala.x / 2, y, z), escala / 2, grado - 1);
            sierpinski(original, new Vector3(x, y, z + escala.z / 2), escala / 2, grado - 1);
            sierpinski(original, new Vector3(x + escala.x / 2, y, z + escala.z / 2), escala / 2, grado - 1);
            sierpinski(original, new Vector3(x + escala.x / 4, y + escala.y / 2, z + escala.z / 4), escala / 2, grado - 1);

            original.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public GameObject clonarTriangulo(GameObject original, Vector3 posicion, Vector3 escala)//Crea un nueva nueva pirámide 
    {
        GameObject clonGO;
        clonGO = Instantiate(original, posicion, Quaternion.identity); //Clona el objeto originaly y devuelve el clon
        clonGO.transform.localScale = escala;
        clonGO.name = "Triangulo" + numClon;
        clonGO.tag = "Triangulo";
        return clonGO;
    }

    public void apagarTrianguloOriginal()//Desactiva pirámide original
    {
        buscoGO.SetActive(false);
    }

    public void encenderTrianguloOriginal()//Activa pirámide original
    {
        buscoGO.SetActive(true);
    }

    public void destruirTriangulos() //Borra todos las pirámides que forman el fractal en ese instante
    {
        GameObject[] triangulos = GameObject.FindGameObjectsWithTag("Triangulo");
        if (triangulos.Length > 0)
        {
            foreach (GameObject t in triangulos)
            {
                Destroy(t);
            }
        }
    }

    int cont = 0;

    public void aumentarGrado() //Permite al jugador aumentar el grado sin pasarse del límite
    {
        grado++;
        if (grado >= 0 && grado <= limite)
        {
            gradoTxt.text = grado.ToString();
            if (!toggle.isOn)
            {
                iniciarJuago();
            }
            else
            {
                animacion.selectAnimation(grado);
                cont = 0;
                Array.Clear(parametros, 0, parametros.Length);
                Array.Clear(lineas, 0, lineas.Length);
                sierpinskiPasos(buscoGO, new Vector3(0, 0, 0), buscoGO.transform.localScale, grado);
            }
        }
        else
        {
            Debug.Log("Grado fuera de rango " + grado);
            grado--;
        }
    }

    public void disminuirGrado() //Permite al jugador disminuir el grado sin pasarse del límite
    {
        grado--;
        if (grado >= 0 && grado <= limite)
        {
            gradoTxt.text = grado.ToString();
            if (!toggle.isOn)
            {
                iniciarJuago();
            }
            else
            {
                animacion.selectAnimation(grado);
                cont = 0;
                Array.Clear(parametros, 0, parametros.Length);
                Array.Clear(lineas, 0, lineas.Length);
                sierpinskiPasos(buscoGO, new Vector3(0, 0, 0), buscoGO.transform.localScale, grado);
            }
        }
        else
        {
            Debug.Log("Grado fuera de rango " + grado);
            grado++;
        }
    }

    public void llenarParametros(string posicion, string escala, string grado, int linea) //LLena los arreglos parametros y lineas
    {
        parametros[cont, 0] = posicion;
        parametros[cont, 1] = escala;
        parametros[cont, 2] = grado;
        lineas[cont] = linea;
        cont++;
    }

    public void sierpinskiPasos(GameObject original, Vector3 posicion, Vector3 escala, int grado) //Genera puntos de interrupción para el debug
    {
        //Crea el punto de interrupción en la línea 4
        llenarParametros(posicion.ToString("F"), escala.ToString(), grado.ToString(), 4);

        if (grado > 0)
        {
            //Crea el punto de interrupción en la línea 8
            llenarParametros(posicion.ToString(), escala.ToString(), grado.ToString(), 8);

            float x = posicion.x;
            float y = posicion.y;
            float z = posicion.z;

            //Crea el punto de interrupción en la línea 14
            llenarParametros(posicion.ToString(), escala.ToString(), grado.ToString(), 14);
            sierpinskiPasos(original, posicion, escala / 2, grado - 1);

            //Crea el punto de interrupción en la línea 20
            llenarParametros(posicion.ToString(), escala.ToString(), grado.ToString(), 20);
            sierpinskiPasos(original, new Vector3(x + escala.x / 2, y, z), escala / 2, grado - 1);

            //Crea el punto de interrupción en la línea 26
            llenarParametros(posicion.ToString(), escala.ToString(), grado.ToString(), 26);
            sierpinskiPasos(original, new Vector3(x, y, z + escala.z / 2), escala / 2, grado - 1);

            //Crea el punto de interrupción en la línea 32
            llenarParametros(posicion.ToString(), escala.ToString(), grado.ToString(), 32);
            sierpinskiPasos(original, new Vector3(x + escala.x / 2, y, z + escala.z / 2), escala / 2, grado - 1);

            //Crea el punto de interrupción en la línea 38
            llenarParametros(posicion.ToString(), escala.ToString(), grado.ToString(), 38);
            sierpinskiPasos(original, new Vector3(x + escala.x / 4, y + escala.y / 2, z + escala.z / 4), escala / 2, grado - 1);
        }
    }

    public void cambiarAModoDebug()//Cambia de modo de juego a modo debug y viceversa
    {
        if (toggle.isOn)
        {
            destruirTriangulos();
            apagarTrianguloOriginal();
            limite = 2;
        }
        else
        {
            encenderTrianguloOriginal();
            limite = 4;
        }
        grado = 0;
        gradoTxt.text = grado.ToString();

    }

    public void desplegarInstrucciones() //Muestra ventana de instrucciones del juego
    {
        instruccionesGO.SetActive(true);
    }

    public void cerrarInstrucciones() //Cierra ventana de instrucciones del juego
    {
        instruccionesGO.SetActive(false);
    }

    void Update() //Update is called once per frame
    {
        debugUI.SetActive(toggle.isOn);
        animacionGO.SetActive(toggle.isOn);
    }

}

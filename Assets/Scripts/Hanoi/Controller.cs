using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    //Postes
    private GameObject posteOrigenGO;
    private Poste posteOrigenScript;
    private GameObject posteDestinoGO;
    private Poste posteDestinoScript;
    private GameObject posteAuxiliarGO;
    private Poste posteAuxiliarScript;
    //Discos
    private GameObject disco1GO;
    private Disco disco1Script;
    private Vector3 posicionInicialDisco1; //Posición del disco al iniciar el juego, sirve para el reinicio del juego
    private GameObject disco2GO;
    private Disco disco2Script;
    private Vector3 posicionInicialDisco2; //Posición del disco al iniciar el juego, sirve para el reinicio del juego
    private GameObject disco3GO;
    private Disco disco3Script;
    private Vector3 posicionInicialDisco3; //Posición del disco al iniciar el juego, sirve para el reinicio del juego
    private GameObject disco4GO;
    private Disco disco4Script;
    private Vector3 posicionInicialDisco4; //Posición del disco al iniciar el juego, sirve para el reinicio del juego
    private GameObject disco5GO;
    private Disco disco5Script;
    private Vector3 posicionInicialDisco5; //Posición del disco al iniciar el juego, sirve para el reinicio del juego

    public int numDiscos; //Numero de discos para este juego, sirve para el algoritmo recursivo
    public int numTotalMovimientos; //Contador de movimientos válidos de todos los discos
    public List<string> instruccionesAyuda;//Lista con pasos para completar el juego
    public bool seMuevenDiscos; //Si es verdadero los discos pueden moverse
    public bool sumaMovimientos; //Si es verdadero se puede sumar los movimientos de los discos
    public string[,] parametros; //Arreglo que guarda el valor de los parámetros para el debug por cada paso
    public int[] lineas; //Arreglo que guarda las líneas de código que se resaltarán el debug por cada paso
    public GameObject instrucciones; //Instrucciones del juego
    public GameObject debugUI; //Ventana de debug
    public GameObject discosAnimados; //Discos animados para mostrar la ejecución del juego en el modo debug
    public GameObject ayudaGO; //Ventana con instrucciones de ayuda para completar el juego
    public GameObject ganarGO; //Ventana que aparece al ganar el juego
    public Text movimientosTxt; //Caja de texto para mostrar el número de movimientos de los discos
    public TextMeshProUGUI ayudaTxt; //Caja de texto para mostrar las instrucciones de ayuda paso a paso
    public Text movimientosAlGanarTxt; //Caja de texto para mostrar el número de movimientos con el que el jugador ganó
    public Button iniciarJuegoBtn, ayudaBtn, homeBtn, instruccionesBtn; //Botones del menú de opciones
    public Toggle toggle; //Permite seleccionar un modo: debug si está encendido y juego si no
    public Image colorMovimientos; //Imagen que cambia de color según el número de movimientos
    private Color colorOriginal; //Color original de colorMovimientos
    public Slider cargar; //Barra que indica al jugador que se está cargando el juego

    void Start() //Start is called before the first frame update
    {
        parametros = new string[122, 4];
        lineas = new int[122];

        //ACCEDER A POSTE ORIGEN
        posteOrigenGO = GameObject.Find("origen");
        posteOrigenScript = posteOrigenGO.GetComponent<Poste>();
        //ACCEDER A POSTE AUXILIAR
        posteAuxiliarGO = GameObject.Find("auxiliar");
        posteAuxiliarScript = posteAuxiliarGO.GetComponent<Poste>();
        //ACCEDER A POSTE DESTINO
        posteDestinoGO = GameObject.Find("destino");
        posteDestinoScript = posteDestinoGO.GetComponent<Poste>();
        //ACCEDER A DISCO 1
        disco1GO = GameObject.Find("Disco1");
        disco1Script = disco1GO.GetComponent<Disco>();
        posicionInicialDisco1 = disco1Script.transform.position;
        //ACCEDER A DISCO 2
        disco2GO = GameObject.Find("Disco2");
        disco2Script = disco2GO.GetComponent<Disco>();
        posicionInicialDisco2 = disco2Script.transform.position;
        //ACCEDER A DISCO 3
        disco3GO = GameObject.Find("Disco3");
        disco3Script = disco3GO.GetComponent<Disco>();
        posicionInicialDisco3 = disco3Script.transform.position;
        //ACCEDER A DISCO 4
        disco4GO = GameObject.Find("Disco4");
        disco4Script = disco4GO.GetComponent<Disco>();
        posicionInicialDisco4 = disco4Script.transform.position;
        //ACCEDER A DISCO 5
        disco5GO = GameObject.Find("Disco5");
        disco5Script = disco5GO.GetComponent<Disco>();
        posicionInicialDisco5 = disco5Script.transform.position;

        numDiscos = 5;
        numTotalMovimientos = 0;
        seMuevenDiscos = true;
        sumaMovimientos = true;
        instrucciones.SetActive(true); //Al iniciar el juego lo primero que aparece es la ventana de instrucciones
        ayudaGO.SetActive(false); //Apagar ventana de ayuda
        debugUI.SetActive(false); //Apagar debug
        discosAnimados.SetActive(false); //Apagar discos con animación
        instruccionesAyuda = new List<string>(); //Instancia la lista
        iniciarJuegoBtn.onClick.AddListener(() => iniciarJuego(iniciarJuegoBtn));
        ayudaBtn.onClick.AddListener(() => iniciarJuego(ayudaBtn));
        colorOriginal = colorMovimientos.color;

        debug();
    }

    void Update() //Update is called once per frame
    {
        //Los discos solo se activan en el modo de juego (cuando el modo debug está apagado)
        disco1GO.SetActive(!toggle.isOn);
        disco2GO.SetActive(!toggle.isOn);
        disco3GO.SetActive(!toggle.isOn);
        disco4GO.SetActive(!toggle.isOn);
        disco5GO.SetActive(!toggle.isOn);

        if (seMuevenDiscos)
        {
            moverMenor(disco1Script);
            moverMenor(disco2Script);
            moverMenor(disco3Script);
            moverMenor(disco4Script);
            moverMenor(disco5Script);
        }

        if (sumaMovimientos && !toggle.isOn) //No se puede contar movimientos hechos por el jugador en el modo debug
        {
            sumarMovimientos();
            cambiarColorMovimientos();
        }

        movimientosTxt.text = numTotalMovimientos.ToString();

        ganarJuago();
    }

    private void cambiarColorMovimientos()//Permite cambiar el color de colorMovimientos a rojo cuando el número de movimientos se pasa de 31
    {
        if (numTotalMovimientos > 31)
        {
            colorMovimientos.color = Color.red;
        }
        else
        {
            colorMovimientos.color = colorOriginal;
        }
    }

    private void moverMenor(Disco disco)//Permite al jugador mover solo el disco de menor diámetro (el último en la pila) en cada poste
    {
        float diametro = disco.transform.localScale.x;//Obtener diámetro del disco 
        if (posteOrigenScript.discoMenorTamX == diametro)//Permite mover el disco solo si es el menor en el poste origen
        {
            disco.seMueve = true;
        }
        else if (posteAuxiliarScript.discoMenorTamX == diametro)//Permite mover el disco solo si es el menor en el poste auxiliar
        {
            disco.seMueve = true;
        }
        else if (posteDestinoScript.discoMenorTamX == diametro)//Permite mover el disco solo si es el menor en el poste destino
        {
            disco.seMueve = true;
        }
        else
        {
            disco.seMueve = false;
        }
    }

    public void ganarJuago()//Si todos los discos se movieron al poste destino se gana el juego
    {
        if (posteDestinoScript.discos.Count == 5 && !toggle.isOn)
        {
            if (disco1GO.transform.position.y <= 2.5f)//Permite al disco 1 posicionarse adecuadamente antes de parar el juego
            {
                seMuevenDiscos = false;
                inmovilizarDiscos();
            }
            ayudaBtn.interactable = false;
            ganarGO.SetActive(true);
            ayudaGO.SetActive(false);
            movimientosAlGanarTxt.text = numTotalMovimientos.ToString() + " movimientos";
        }
        else
        {
            ganarGO.SetActive(false);
        }
    }

    public void iniciarJuego(Button button)//Reinicia el juego
    {
        cargar.gameObject.SetActive(true);
        iniciarJuegoBtn.interactable = false;
        ayudaBtn.interactable = false;
        homeBtn.interactable = false;
        instruccionesBtn.interactable = false;
        toggle.interactable = false;
        ayudaGO.SetActive(false);
        seMuevenDiscos = false;
        inmovilizarDiscos();
        instruccionesAyuda.Clear();
        index = 0;
        StartCoroutine(reiniciarDiscos());

        if (button.name.Equals(ayudaBtn.name))
        {
            ayudaGO.SetActive(true);
            hanoi(numDiscos, posteOrigenGO, posteAuxiliarGO, posteDestinoGO);
            desplegarInstruccionesAyuda();
        }
    }

    public void inmovilizarDiscos()//No permite al jugador mover los discos
    {
        disco1Script.seMueve = false;
        disco2Script.seMueve = false;
        disco3Script.seMueve = false;
        disco4Script.seMueve = false;
        disco5Script.seMueve = false;
    }

    IEnumerator reiniciarDiscos()//Devuelve los discos a su estado inicial: vuelven al poste origen y los movimientos se vuelven 0
    {
        movimientosTxt.enabled = false;
        if (posteOrigenScript.discos.Count > 0)
        {
            GameObject go = posteOrigenScript.discos[posteOrigenScript.discos.Count - 1];
            if (go.name.Equals("Disco1"))
            {
                disco1GO.SetActive(false);
                posteOrigenScript.discos.Remove(go);
            }
        }

        cargar.value = 0;
        yield return new WaitForSeconds(1);
        cargar.value = 0.2f;
        disco5Script.transform.position = posicionInicialDisco5;
        yield return new WaitForSeconds(1);
        cargar.value = 0.4f;
        disco4Script.transform.position = posicionInicialDisco4;
        yield return new WaitForSeconds(1);
        cargar.value = 0.6f;
        disco3Script.transform.position = posicionInicialDisco3;
        yield return new WaitForSeconds(1);
        cargar.value = 0.8f;
        disco2Script.transform.position = posicionInicialDisco2;
        yield return new WaitForSeconds(1);
        cargar.value = 0.9f;
        disco1GO.SetActive(true);
        disco1Script.transform.position = posicionInicialDisco1;
        yield return new WaitForSeconds(1);
        cargar.value = 1f;

        disco5Script.movimientos = 0;
        disco4Script.movimientos = 0;
        disco3Script.movimientos = 0;
        disco2Script.movimientos = 0;
        disco1Script.movimientos = 0;

        seMuevenDiscos = true;
        movimientosTxt.enabled = true;
        toggle.interactable = true;
        iniciarJuegoBtn.interactable = true;
        ayudaBtn.interactable = true;
        homeBtn.interactable = true;
        instruccionesBtn.interactable = true;
        cargar.gameObject.SetActive(false);
    }

    void sumarMovimientos()//Suma los movimientos validos de los discos que el jugador ha realizado
    {
        numTotalMovimientos = disco1Script.movimientos + disco2Script.movimientos + disco3Script.movimientos + disco4Script.movimientos + disco5Script.movimientos;
    }

    public void hanoi(int numDisco, GameObject posteOrigen, GameObject posteAuxiliar, GameObject posteDestino)//Algoritmo recursivo para resolver las torres de hanoi
    {
        if (numDisco == 1)
        {
            mover(numDisco, posteOrigen, posteDestino);
        }
        else
        {
            hanoi(numDisco - 1, posteOrigen, posteDestino, posteAuxiliar);
            mover(numDisco, posteOrigen, posteDestino);
            hanoi(numDisco - 1, posteAuxiliar, posteOrigen, posteDestino);
        }
    }

    public void mover(int numDisco, GameObject posteOrigen, GameObject posteDestino)//Crea cadenas de texto con Pasos para resolver el juego
    {
        string ayuda = " ";
        switch (numDisco)
        {
            case 1:
                ayuda = "Mover disco " +
                    "<color=orange>" + numDisco + "</color>" +
                    " desde " + posteOrigen.name +
                    " hasta " + posteDestino.name;
                break;
            case 2:
                ayuda = "Mover disco " +
                    "<color=blue>" + numDisco + "</color>" +
                    " desde " + posteOrigen.name +
                    " hasta " + posteDestino.name;
                break;
            case 3:
                ayuda = "Mover disco " +
                    "<color=green>" + numDisco + "</color>" +
                    " desde " + posteOrigen.name +
                    " hasta " + posteDestino.name;
                break;
            case 4:
                ayuda = "Mover disco " +
                    "<color=red>" + numDisco + "</color>" +
                    " desde " + posteOrigen.name +
                    " hasta " + posteDestino.name;
                break;
            case 5:
                ayuda = "Mover disco " +
                    "<color=yellow>" + numDisco + "</color>" +
                    " desde " + posteOrigen.name +
                    " hasta " + posteDestino.name;
                break;
            default:
                break;
        }
        instruccionesAyuda.Add(ayuda);
    }

    int cont = 0;
    public void llenarParametros(string numDisco, string po, string pa, string pd, int linea)//Llena los arreglos parametros y lineas
    {
        parametros[cont, 0] = numDisco;
        parametros[cont, 1] = po;
        parametros[cont, 2] = pa;
        parametros[cont, 3] = pd;
        lineas[cont] = linea;
        cont++;
    }

    private void debug()//Llama a la función recursiva para generar los pasos del debug
    {
        hanoiPasos(numDiscos, posteOrigenGO, posteAuxiliarGO, posteDestinoGO);
    }

    public void hanoiPasos(int numDisco, GameObject posteOrigen, GameObject posteAuxiliar, GameObject posteDestino)//Genera puntos de interrupción para el debug
    {
        //Crea el punto de interrupción en la línea 4
        llenarParametros(numDisco.ToString(), posteOrigen.name, posteAuxiliar.name, posteDestino.name, 4);

        if (numDisco == 1)
        {
            //Crea el punto de interrupción en la línea 7
            llenarParametros(numDisco.ToString(), posteOrigen.name, posteAuxiliar.name, posteDestino.name, 7);
        }
        else
        {
            //Crea el punto de interrupción en la línea 12
            llenarParametros(numDisco.ToString(), posteOrigen.name, posteAuxiliar.name, posteDestino.name, 12);
            hanoiPasos(numDisco - 1, posteOrigen, posteDestino, posteAuxiliar);
            llenarParametros(numDisco.ToString(), posteOrigen.name, posteAuxiliar.name, posteDestino.name, 12); //Sale de la llamada

            //Crea el punto de interrupción en la línea 15
            llenarParametros(numDisco.ToString(), posteOrigen.name, posteAuxiliar.name, posteDestino.name, 15);

            //Crea el punto de interrupción en la línea 18
            llenarParametros(numDisco.ToString(), posteOrigen.name, posteAuxiliar.name, posteDestino.name, 18);
            hanoiPasos(numDisco - 1, posteAuxiliar, posteOrigen, posteDestino);
            llenarParametros(numDisco.ToString(), posteOrigen.name, posteAuxiliar.name, posteDestino.name, 18);
            //Sale de la llamada
        }
    }

    public void desplegarInstruccionesAyuda()//Abrir instrucciones del de ayuda para resolver el juego
    {
        ayudaTxt.text = instruccionesAyuda[index];
    }

    public void desplegarInstrucciones()//Abrir instrucciones del juego
    {
        instrucciones.SetActive(true);
    }

    public void cerrarInstrucciones()//Cerrar instrucciones del juego
    {
        instrucciones.SetActive(false);
    }

    int index = 0;
    public void anterior()//Retroceder un paso en las instrucciones de ayuda para resolver el juego
    {
        index--;
        if (index >= 0 && index < instruccionesAyuda.Count)
        {
            ayudaTxt.text = instruccionesAyuda[index];
        }
        else
        {
            index++;
        }
    }

    public void siguiente()//Avanzar un paso en las instrucciones de ayuda para resolver el juego
    {
        index++;
        if (index >= 0 && index < instruccionesAyuda.Count)
        {
            ayudaTxt.text = instruccionesAyuda[index];
        }
        else
        {
            index--;
        }
    }

    public void cambiarAModoDebug()//Cambia de modo de juego a modo debug y viceversa
    {
        if (toggle.isOn)
        {
            ayudaBtn.interactable = false;
            ayudaGO.SetActive(false);
            iniciarJuegoBtn.interactable = false;
            colorMovimientos.color = colorOriginal;
        }
        else
        {
            iniciarJuegoBtn.interactable = true;
            ayudaBtn.interactable = true;
        }
        debugUI.SetActive(toggle.isOn);
        discosAnimados.SetActive(toggle.isOn);
    }


}

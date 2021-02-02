using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    public Toggle modoDebug;    //Checkbox que activa o desactiva el modo Debug
    public Text tiempoLabel;    //Texto que indica el tiempo establecido
    public Text velocidadLabel; //Texto que indica la velocidad establecida
    public TextMeshProUGUI siguiemeLabel;
    public Slider tiempo;   //Slider para controlar el tiempo de análisis
    public Slider velocidad;    //Slider para controlar la velocidad
    public LineRenderer laser;  //Controla si el laser se prende o apaga
    public GameObject codigoUI; //Objeto que contiene la UI del código
    public GameObject ui;   //Objeto que contiene toda la UI
    public GameObject debugUI;  //Objeto que contiene la UI del modo Debug
    public GameObject opcionesUI;   //Objeto que contiene la UI de las opciones
    public GameObject instrucciones;    ////Objeto que contiene la UI de las instrucciones
    public PlayerIA bot;    //Objeto para encender o apagar el bot


    // Start is called before the first frame update
    void Start()
    {
        siguiemeLabel.enabled = false;
        bot.enabled = false;
        debugUI.SetActive(false);
        opcionesUI.SetActive(false);
        instrucciones.SetActive(true);
        tiempo.onValueChanged.AddListener(delegate { cambiarTiempo(); });
        velocidad.onValueChanged.AddListener(delegate { cambiarVelocidad(); });
        modoDebug.onValueChanged.AddListener(delegate { ToggleValueChanged(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick))
        {
            
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick))
        {
            UI();
        }

        tiempoLabel.text = tiempo.value.ToString("F");
        velocidadLabel.text = velocidad.value.ToString("F");
    }
    public void desplegarInstrucciones()    //Método que despliega la ventana de las instrucciones
    {
        instrucciones.SetActive(true);
        opcionesUI.SetActive(false);

    }
    public void cerrarInstrucciones()   //Método que cierra la ventana de las instrucciones
    {
        instrucciones.SetActive(false);
        opcionesUI.SetActive(true);

    }

    void ToggleValueChanged()   //Metodo que controla cuando se esciende el modo Debug
    {
        bot.enabled = modoDebug.isOn;
        siguiemeLabel.enabled = modoDebug.isOn;
        debugUI.SetActive(modoDebug.isOn);
    }

    void UI()   //Controla cuando se esciende la UI
    {
        laser.enabled = !ui.activeSelf;
        ui.SetActive(!ui.activeSelf);
    }

    void cambiarVelocidad() //Método que permite cambiar la velocidad del bot
    {
        bot.speed = velocidad.value;
    }
    void cambiarTiempo()    //Método que permite cambiar el tiempo de análisis del bot
    {
        bot.startAnalyzeTime = tiempo.value;
    }

}
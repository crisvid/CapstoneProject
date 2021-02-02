using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public GameObject salir; //Ventana para salir del juego

    void Start()// Start is called before the first frame update
    {

    }

    void Update()// Update is called once per frame
    {

    }

    public void cambiarEscenaTriangulo() //Cambiar de escena a la del triángulo
    {
        SceneManager.LoadScene("Triangulo");
    }

    public void cambiarEscenaLaberinto() //Cambiar de escena a la del laberinto
    {
        SceneManager.LoadScene("Laberinto");
    }

    public void cambiarEscenaHanoi() //Cambiar de escena a la de Hanoi
    {
        SceneManager.LoadScene("Hanoi");
    }

    public void cambiarEscenaMenu() //Cambiar de escena a la del menú principal
    {
        SceneManager.LoadScene("Menu");
    }

    public void desplegarVentanaSalir() //Desplegar ventana para salir del juego
    {
        salir.SetActive(true);
    }

    public void cerrarVentanaSalir() //Cerrar ventana para salir del juego
    {
        salir.SetActive(false);
    }

    public void salirDelJuego() //Cerrar la aplicación 
    {
        Application.Quit();
    }

}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugHanoi : MonoBehaviour
{
    public Animator animator; //Interfaz para controlar el sistema de animación Mecanim
    public float frame; //Frame actual de la animación
    public int paso; //Paso actual en la ejecución del código
    public HighlightLine highlightLine; //Script que resalta las líneas de código
    public Controller controller; //Script que controla todo el juego
    public int numPasos; //Número de pasos en la ejecución del código
    public float normalizedTime; //El tiempo normalizado de la animación. Un valor de 1 es el final de la animación. Un valor de 0,5 es la mitad de la animación
    public float totalFrames; //Total de frames que tiene la animación 
    private int numMovimientosAnimacion; //Número de movimientos del disco que hace la animación en el modo debug
    public Text movimientosAnimacionTxt; //Caja de texto para mostrar numMovimientosAnimacion
    //Cajas de texto para mostrar los parámetros del algoritmo recursivo
    public TextMeshProUGUI numDisco;
    public TextMeshProUGUI posteOrigen;
    public TextMeshProUGUI posteAuxiliar;
    public TextMeshProUGUI posteDestino;

    void Start() // Start is called before the first frame update
    {
        paso = 0;
        numMovimientosAnimacion = 0;
        numPasos = controller.lineas.Length - 1;
        frame = 15; //La animación empieza en el frame 15
        animator.speed = 0; //La velocidad de reproducción del Animator se asigna a 0, 1 es la velocidad de reproducción normal, para que la animación no se reproduzca automáticamente
        totalFrames = 3720f;
    }

    public void Siguiente() //Avanza un paso en el debug
    {
        if (paso < numPasos)
        {
            paso++;
            if (controller.lineas[paso] == 7 || controller.lineas[paso] == 15)
            {
                frame += 120;
                numMovimientosAnimacion++;
            }
            normalizedTime = (1 / totalFrames) * frame; //Regla de 3 para obtener el tiempo normalizado de la animación
            animator.Play("ResolverDiscos", 0, normalizedTime); //Reproduce la animación de un estado según normalizedTime
        }
    }

    public void Anterior() //Retrocede un paso en el debug
    {
        if (paso > 0)
        {

            if (controller.lineas[paso] == 7 || controller.lineas[paso] == 15)
            {
                frame -= 120;
                numMovimientosAnimacion--;

            }
            paso--;
            normalizedTime = (1 / totalFrames) * frame; //Regla de 3 para obtener el tiempo normalizado de la animación
            animator.Play("ResolverDiscos", 0, normalizedTime); //Reproduce la animación de un estado según normalizedTime
        }
    }

    void Update() // Update is called once per frame
    {
        highlightLine.line = controller.lineas[paso];//Resalta una línea en el código
        movimientosAnimacionTxt.text = numMovimientosAnimacion.ToString(); //Actualiza el número de movimientos
        //Actualiza los parámetros del codigo recursivo
        numDisco.text = controller.parametros[paso, 0];
        posteOrigen.text = controller.parametros[paso, 1];
        posteAuxiliar.text = controller.parametros[paso, 2];
        posteDestino.text = controller.parametros[paso, 3];
    }

}

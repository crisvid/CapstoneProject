using TMPro;
using UnityEngine;

public class DebugTriangulo : MonoBehaviour
{

    public Animator animator; //Interfaz para controlar el sistema de animación Mecanim
    public float frame; //Frame actual de la animación
    public float startFrame; //Frame inicial de la animación
    public float endFrame; //Frame final de la animación 
    public float normalizedTime; //El tiempo normalizado de la animación. Un valor de 1 es el final de la animación. Un valor de 0,5 es la mitad de la animación
    public float totalFrames; //Total de frames que tiene la animación 
    public int paso; //Paso actual en la ejecución del código
    public SierpinskiTriangleScript script; //Script que genera el fractal
    public HighlightLine highlight; //Script que resalta las líneas de código
    //Cajas de texto para mostrar los parámetros del algoritmo recursivo
    public TextMeshProUGUI grado;
    public TextMeshProUGUI escala;
    public TextMeshProUGUI posicion;

    void Start() // Start is called before the first frame update
    {
        paso = 0;
        frame = 30; //La animación empieza en el frame 30
        animator = GetComponent<Animator>();
        animator.speed = 0; //La velocidad de reproducción del Animator se asigna a 0, 1 es la velocidad de reproducción normal, para que la animación no se reproduzca automáticamente
        totalFrames = 4680f;
        startFrame = 30;
        endFrame = 30;
    }

    void Update() // Update is called once per frame
    {
        if (frame < 30) //Controla que no se pueda pasar de los límites de la animación
        {
            frame = 30;
        }
        if (frame > totalFrames)
        {
            frame = totalFrames + 30;
        }
        highlight.line = script.lineas[paso]; //Resalta una línea en el código
        //Actualiza los parámetros del codigo recursivo
        posicion.text = script.parametros[paso, 0];
        escala.text = script.parametros[paso, 1];
        grado.text = script.parametros[paso, 2];
    }

    public void next() //Avanza un paso en el debug
    {
        if (frame < endFrame)
        {
            paso++;
            frame += 60;
            normalizedTime = (1 / totalFrames) * frame; //Regla de 3 para obtener el tiempo normalizado de la animación
            animator.Play("triangulo", 0, normalizedTime); //Reproduce la animación de un estado según normalizedTime
        }
    }

    public void previous() //Retrocede un paso en el debug
    {
        if (frame > startFrame)
        {
            paso--;
            frame -= 60;
            normalizedTime = (1 / totalFrames) * frame; //Regla de 3 para obtener el tiempo normalizado de la animación
            animator.Play("triangulo", 0, normalizedTime); //Reproduce la animación de un estado según normalizedTime
        }
    }

    public void selectAnimation(int grado) //Selecciona el frame inicial y final de la animación según el grado seleccionado
    {
        paso = 0;
        switch (grado)
        {
            case 0:
                startFrame = 30;
                endFrame = 30;
                break;
            case 1:
                startFrame = 30;
                endFrame = 690;
                break;
            case 2:
                startFrame = 750;
                endFrame = totalFrames;
                break;
            default:
                break;
        }
        frame = startFrame;
        normalizedTime = (1 / totalFrames) * frame;
        animator.Play("triangulo", 0, normalizedTime);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIA : MonoBehaviour
{
    public int distanceRay; //Distancia maxima de los Raycast
    public float speed; //Velocidad del Bot   
    private bool meta;  //Verdadero si se encontro la meta
    public float distancia; //Distancia utilizada para el método Avanzar
    public LayerMask IgnorePlayer;  //Capa que ignora los Raycast
    public Vector3 targetPosition;  //Posición don se desplaza al avanzar
    List<Vector3> rays = new List<Vector3>();   //Lista de Raycast (Arriba, Abajo, Izquierda, Derecha)
    List<RaycastHit> hitRays = new List<RaycastHit>();  //Lista que guarda con que objeto colisiona cada Raycast
    public float analyzeTime;   //Tiempo de análisis 
    public float startAnalyzeTime;  //Establece el tiempo de análisis
    public int caso;    //Variable que identifica que caso esta analizando
    public bool decide; //Variable que cambia cuando el bot decide a que dirección ir
    public Flechas flechas; //Objeto para controlar el estado de las flechas
    public Animator unityChan;  //Objeto para controlar las animaciones de unity Chan
    public HighlightLine highlight; //Objeto para controlar la linea resaltada en el modo Debug

    void Start()
    {
        
        meta = false;

        rays.Add(Vector3.zero);
        rays.Add(Vector3.zero);
        rays.Add(Vector3.zero);
        rays.Add(Vector3.zero);

        hitRays.Add(new RaycastHit());
        hitRays.Add(new RaycastHit());
        hitRays.Add(new RaycastHit());
        hitRays.Add(new RaycastHit());

        analyzeTime = startAnalyzeTime;
        caso = 1;
    }
    private void Update()
    {      
        StartBot();
    }
    void FixedUpdate()
    {
        
        DrawRay();        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Meta")){
            meta = true;
            unityChan.SetBool("Win", true);
        }
    }
    
    private void Avanzar()  //Método para avanzar
    {
        highlight.line = 26;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);   //Avanaza hacia la posición del Target

        unityChan.SetFloat("Speed", 0.3f);  //Ejecuta la animcaión de caminar

        if (transform.position == targetPosition)
        {
            unityChan.SetFloat("Speed", 0f);    //Ejecuta la animcaión de parado
            decide = false;
            flechas.reiniciar();
        }
        
    }   
    private void StartBot() //Código recursivo que permite al bot resolver el laberinto
    {
        if (meta)   //Detecta si llego a la meta
        {
            highlight.line = 6;
        }
        else
        {            
            if (!decide)    //Si no decide sigue analizando
            {
                if (analizar())
                {
                    switch (caso)   //Selecciona la dirección para avanzar
                    {
                        case 1:

                            flechas.AnalizarDerecha();
                            if (LibreDerecha())
                            {
                                highlight.line = 12;
                                GirarDerecha();
                                decide = true;
                            }
                            break;
                        case 2:
                            if (!ObstaculoFrente()) 
                            {
                                highlight.line = 16;
                                Adelante();
                                decide = true;
                            }
                            break;
                        case 3:
                            if (LibreIzquierda())
                            {
                                highlight.line = 20;
                                GirarIzquierda();
                                decide = true;
                            }
                            break;
                        default:
                            highlight.line = 24;
                            DarVuelta();
                            decide = true;
                            break;
                    }
                    caso++;
                    analyzeTime = startAnalyzeTime; //Reinicia el tiempo de análisis
                }
            }
            else
            {                
                Avanzar();  //Avanza
                caso = 1;
            }
        }
    }
    private bool LibreDerecha() //Método que identifica a la derecha del bot existe o no una pared
    {
        if (hitRays[2].collider == null)
        {
            return true;
        }
        else
        {
            if (hitRays[2].collider.tag.Equals("Wall"))
            {
                return false;
            }
            return true;
        }
    }
    private bool LibreIzquierda()   //Método que identifica a la izquierda del bot existe o no una pared
    {        
        if (hitRays[3].collider == null)
        {
            return true;
        }
        else
        {
            if (hitRays[3].collider.tag.Equals("Wall"))
            {
                return false;
            }
            return true;
        }
    }
    private bool ObstaculoFrente()  //Método que identifica al frente del bot existe o no una pared
    {
        if (hitRays[0].collider == null)
        {
            return false;
        }
        else
        {
            if (hitRays[0].collider.tag.Equals("Wall"))
            {
                return true;
            }
            return false;
        }
    }
    private void DrawRay()  //Método que dibuja los Raycast en la escena
    {
        //RAYCAST
        rays[0] = transform.TransformDirection(Vector3.forward);    //FRENTE
        rays[1] = transform.TransformDirection(Vector3.back);   //ATRAS
        rays[2] = transform.TransformDirection(Vector3.right);  //DERECHA
        rays[3] = transform.TransformDirection(Vector3.left);   //IZQUIERDA
        int index = 0;

        foreach (Vector3 ray in rays)
        {
            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, ray, out hit, distanceRay, ~IgnorePlayer))
            {
                Debug.DrawRay(transform.position, ray * hit.distance, Color.red);   //Si el rayo colisina con una pared se pinta de rojo
            }
            else
            {
                Debug.DrawRay(transform.position, ray * distanceRay, Color.green);//Si el rayo no colisina se pinta de verde
            }
            hitRays[index] = hit;
            index++;                       
        }
    }
    private void Adelante() //Método que mantiene al bot con la vista hacia el frente
    {
        nuevaPoiscion();
        flechas.Adelante(); //Enciende la flecha hacia el frente
    }
    private void GirarDerecha() //Método que gira al bot hacia la derecha
    {
        transform.Rotate(0.0f, 90.0f, 0.0f);
        nuevaPoiscion();
        flechas.Derecha();  //Enciende la flecha hacia la derecha
    }
    private void GirarIzquierda()   //Método que gira al bot hacia la izquierda
    {
        transform.Rotate(0.0f, -90.0f, 0.0f);
        nuevaPoiscion();
        flechas.Izquierda();    //Enciende la flecha hacia la izquierda
    }
    private void DarVuelta()    //Método que hace girar media vuelta al bot  
    {
        transform.Rotate(0.0f, 180.0f, 0.0f);   
        nuevaPoiscion();
        flechas.Atras();    //Enciende la flecha hacia la atras
    }
    bool analizar() //Método que devuelve falso cuando termino de analizar cada caso
    {
        if (analyzeTime > 0)    //Cuando es menor que 0 seleciona el caso,
        {
            analyzeTime -= Time.deltaTime;
            flechas.reiniciar();
            switch (caso)
            {
                case 1:
                    highlight.line = 10;
                    flechas.AnalizarDerecha();
                    break;
                case 2:
                    highlight.line = 14;
                    flechas.AnalizarAdelante();
                    break;
                case 3:
                    highlight.line = 18;
                    flechas.AnalizarIzquierda();
                    break;
                case 4:
                    highlight.line = 22;
                    flechas.AnalizarAtras();

                    break;
            }
            return false;
        }

        return true;

    }
    void nuevaPoiscion()    //Establece la siguiente posición donde se va ha mover el bot
    {
        targetPosition = transform.position;
        targetPosition += this.transform.forward * distancia;
    }

}

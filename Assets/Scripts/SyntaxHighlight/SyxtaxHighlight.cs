using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System;

public class SyxtaxHighlight : MonoBehaviour
{
    public TextMeshProUGUI inputText;   //Guarda el código que se desea aplicar los colores
    public TextMeshProUGUI highlightText;   //Guarda el código con los colores aplicados
    public SyntaxColors syntaxColors;   //Obtiene la información de los colres y pababras
    public string nombreDocumentoDescarga;  //Nombre del archivo de descarga
    string changeInput; //Variable temporal que guarda la información de inputText
    string finalCode;   //Variable que guarda el código final antes de mostrarlo en highlightText

    List<string> symbols = new List<string>();  //Obtiene los simbolos reservados
    List<string> keywords = new List<string>(); //Obtiene las palabras reservadas
    List<string> unityKeywords = new List<string>();    //Obtiene las palabras reservadas de Unity
    List<string> variables = new List<string>();    //Obtiene las variables personalizadas del usuario
    List<string> functions = new List<string>();    //Obtiene las funciones personalizadas del usuario
    List<string> comments = new List<string>(); 
    List<string> listSyntax = new List<string>();
    List<string> inputCode = new List<string>();    //Guada el código ingresado linea por linea
    List<string> highlightCode = new List<string>();    //Guada el código ingresado linea por linea con colres

    //COLORES
    string symbolsColor;
    string keywordsColor;
    string unityKeywordsColor;
    string variablesColor;
    string functionsColor;

    //List<string> symbols = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        changeInput = inputText.text;
        getHexColor();
        createSyntaxList();        
        buildHighlightCode();
    }

    // Update is called once per frame
    void Update()
    {
        if(changeInput != inputText.text)
        {
            buildHighlightCode();
            changeInput = inputText.text;
        }
    }
    void buildHighlightCode()   //Construye el código con Colores
    {
        getLines();
        highlightline(unityKeywords, unityKeywordsColor);   //Colorea las palabras clave de Unity
        highlightline(keywords, keywordsColor); //Colorea las palabras clave de C#
        highlightline(variables, variablesColor);   //Colorea las varibles personalizadas del usuario
        highlightline(functions, functionsColor);   //Colorea las funciones personalizadas del usuario    
        highlightText.text = finalCode; //Mustra en pantalla el código final
    }

    void getLines() //Método que guarda el código linea por linea en una lista
    {
        inputCode = inputText.text.Split('\n').ToList<string>();    
    }

    void getHexColor()  //Método para convertir el color seleccionado en Hexadecimal
    {
        symbolsColor = ToHex(syntaxColors.keywordsColor);
        keywordsColor = ToHex(syntaxColors.keywordsColor);
        unityKeywordsColor = ToHex(syntaxColors.unityKeywordsColor);
        variablesColor = ToHex(syntaxColors.variablesColor);
        functionsColor = ToHex(syntaxColors.functionsColor);
    }

    void createSyntaxList() //Método que guarda los simbolos y palabras en sus respectivas listas
    {
        symbols = syntaxColors.symbols.Split(' ').ToList<string>();
        keywords = syntaxColors.keywords.Split(' ').ToList<string>();
        unityKeywords = syntaxColors.unityKeywords.Split(' ').ToList<string>();
        variables = syntaxColors.variables.Split(' ').ToList<string>();
        functions = syntaxColors.functions.Split(' ').ToList<string>();
    }

    void highlightline(List<string> syntax, string color)   //Envía linea por linea para colorear
    {
        finalCode = "";
        foreach (string line in inputCode)
        {
            highlightSyntax(line, syntax, color);
        }
        inputCode = finalCode.Split('\n').ToList<string>();
    }

    void highlightSyntax(string line, List<string> matches, string color)   //Aplica las etiquetas de color linea por linea en cada palabra
    {
        for (int i = 0; i < matches.Count; i++)
        {
            if (Regex.IsMatch(line, $@"\b{matches[i]}\b"))  //Identifica si existe una palabra o simbolo especial
            {
                line = line.Replace(matches[i], $"<color={color}>" + matches[i].ToString() + "</color>");   //Colorea
            }

        }
        highlightCode.Add(line);
        finalCode = finalCode + line + "\n";    //Guarda linea por linea aplicando el color

    }

    public static string ToHex(Color c) => "#"+ColorUtility.ToHtmlStringRGB(c);

    public void DescargarCodigo()   //Descarga el código en un archivo con extensión .cs
    {
        List<string> descargarCodigo = new List<string>();
        descargarCodigo = inputText.text.Split('\n').ToList<string>();


        string docPath =
          Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //Directorio para guardar el código

        // Write the string array to a new file named "WriteLines.txt".
        using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, nombreDocumentoDescarga+".cs")))
        {
            for (int i = 0; i < descargarCodigo.Count; i++)
            {
                outputFile.WriteLine(descargarCodigo[i]);   //Escribe linea por linea el archivo
            }
        }
    }

}

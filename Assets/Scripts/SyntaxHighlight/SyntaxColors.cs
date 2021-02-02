using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class SyntaxColors : ScriptableObject
{
    //TexArea para ingresar los símbolos y sintaxis
    [TextArea]
    public string delimiterSymbols;
    [TextArea]
    public string keywords;
    [TextArea]
    public string unityKeywords;
    [TextArea]
    public string symbols;
    [TextArea]
    public string variables;
    [TextArea]
    public string functions;
    [TextArea]
    public string comments;

    //Selección de colores para pintar los simbolos y palabras de la sintaxis
    public Color delimiterColor = Color.black;
    public Color keywordsColor = Color.black;
    public Color unityKeywordsColor = Color.black;
    public Color variablesColor = Color.black;
    public Color functionsColor = Color.black;
    public Color symbolsColor = Color.black;
    public Color numbersColor = Color.black;
    public Color commentsColor = Color.black;
    public Color normalTextColor = Color.black;

}

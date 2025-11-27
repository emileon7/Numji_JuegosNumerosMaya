using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class QuizNumji : MonoBehaviour
{
    [Header("PREGUNTAS")]
    
    public List<PreguntaNumji> preguntas;
    private List<PreguntaNumji> preguntasSeleccionadas;
    private int indexActual = 0;

    [Header("UI")]
    public Image imagenPregunta;
    public Button boton1;
    public Button boton2;
    public Button boton3;

    public TextMeshProUGUI texto1;
    public TextMeshProUGUI texto2;
    public TextMeshProUGUI texto3;

    [Header("Resultados")]
    public GameObject panelResultado;
    public TextMeshProUGUI textoAciertos;
    public TextMeshProUGUI textoTiempo;


    private int aciertos = 0;
    private float tiempo;

    void Start()
    {
        tiempo = 0;
        panelResultado.SetActive(false);

        SeleccionarPreguntas();
        MostrarPregunta();

    }


    void Update()
    {
        tiempo += Time.deltaTime;
    }

    void SeleccionarPreguntas()
    {
        // Copia
        List<PreguntaNumji> temp = new List<PreguntaNumji>(preguntas);

        // Mezcla
        for (int i = 0; i < temp.Count; i++)
        {
            int r = Random.Range(i, temp.Count);
            var aux = temp[i];
            temp[i] = temp[r];
            temp[r] = aux;
        }

        // Selecciona las primeras 6
        preguntasSeleccionadas = temp.GetRange(0, 6);
    }

    public void MostrarPregunta()
    {
        PreguntaNumji p = preguntasSeleccionadas[indexActual];

        imagenPregunta.sprite = p.imagen;

        List<string> opciones = new List<string>();
        opciones.Add(p.respuestaCorrecta);
        opciones.Add(p.respuestasIncorrectas[0]);
        opciones.Add(p.respuestasIncorrectas[1]);

        // Mezclar respuestas
        for (int i = 0; i < 3; i++)
        {
            int r = Random.Range(i, 3);
            var aux = opciones[i];
            opciones[i] = opciones[r];
            opciones[r] = aux;
        }

        texto1.text = opciones[0];
        texto2.text = opciones[1];
        texto3.text = opciones[2];

        // Limpiar colores
        boton1.image.color = Color.white;
        boton2.image.color = Color.white;
        boton3.image.color = Color.white;

        boton1.onClick.RemoveAllListeners();
        boton2.onClick.RemoveAllListeners();
        boton3.onClick.RemoveAllListeners();

        boton1.onClick.AddListener(() => Elegir(opciones[0], boton1));
        boton2.onClick.AddListener(() => Elegir(opciones[1], boton2));
        boton3.onClick.AddListener(() => Elegir(opciones[2], boton3));
    }

    void Elegir(string respuesta, Button boton)
    {
        bool correcta = respuesta == preguntasSeleccionadas[indexActual].respuestaCorrecta;

        if (correcta)
        {
            boton.image.color = Color.green;
            aciertos++;
        }
        else
        {
            boton.image.color = Color.red;
        }

        StartCoroutine(SiguientePregunta());
    }

    IEnumerator SiguientePregunta()
    {
        yield return new WaitForSeconds(1f);

        indexActual++;

        if (indexActual >= 6)
        {
            Finalizar();
        }
        else
        {
            MostrarPregunta();
        }
    }

    void Finalizar()
    {
        panelResultado.SetActive(true);

        textoAciertos.text = aciertos + " / 6";
        textoTiempo.text = "Tiempo" + tiempo.ToString("F1") + "s";
    }

}

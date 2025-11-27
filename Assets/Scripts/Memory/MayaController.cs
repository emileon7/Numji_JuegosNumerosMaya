using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MayaController : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject cartaPrefab;
    public Transform contenedorCartas;
    public TextMeshProUGUI textoScore;
    public GameObject panelVictoria;
    public TextMeshProUGUI textoTiempo;
    public TextMeshProUGUI textoTiempoFinal;
    public Button Btn_regresar_1;
    public Button Btn_regresar_2;

    [Header("Contenido del Juego")]
    public List<Sprite> imagenesMayas = new List<Sprite>();
    public List<int> numerosDecimales = new List<int>() { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

    [Header("Audio (Opcional)")]
    public AudioClip sonidoVoltear;
    public AudioClip sonidoAcierto;
    public AudioClip sonidoError;
    public AudioClip sonidoWin;

    private List<Card> todasLasCartas = new List<Card>();
    private Card primeraCarta = null;
    private Card segundaCarta = null;
    private bool estaBloqueado = false;
    private int puntos = 0;
    private int paresEncontrados = 0;
    private AudioSource audioSource;

    private float tiempoTranscurrido = 0f;
    private bool cronometroActivo = false;
    private string tiempoFinalTexto = "";

    void Start()
    {
        // Configurar audio
        audioSource = gameObject.AddComponent<AudioSource>();

        // Validar que tengamos 10 imágenes
        if (imagenesMayas.Count != 10)
        {
            Debug.LogError("¡Necesitas exactamente 10 imágenes mayas!");
            return;
        }

        // Ocultar panel de victoria
        if (panelVictoria != null)
            panelVictoria.SetActive(false);

        // Generar el juego
        GenerarCartas();
        IniciarCronometro();
    }

    void Update()
    {
        if (cronometroActivo)
        {
            tiempoTranscurrido += Time.deltaTime;
            ActualizarCronometro(); // ← SE LLAMA AQUÍ CONSTANTEMENTE
        }
    }
    void GenerarCartas()
    {
        // Limpiar cartas anteriores si existen
        foreach (Transform child in contenedorCartas)
        {
            Destroy(child.gameObject);
        }
        todasLasCartas.Clear();

        // Crear 10 pares (20 cartas)
        for (int i = 0; i < 10; i++)
        {
            // Carta con imagen maya
            CrearCarta(i, imagenesMayas[i], 0, true);

            // Carta con número decimal
            CrearCarta(i, null, numerosDecimales[i], false);
        }

        // Mezclar las cartas
        MezclarCartas();
    }

    void CrearCarta(int id, Sprite sprite, int numeroDecimal, bool esMaya)
    {
        GameObject nuevaCarta = Instantiate(cartaPrefab, contenedorCartas);
        Card cardScript = nuevaCarta.GetComponent<Card>();

        if (cardScript == null)
        {
            Debug.LogError("El prefab de carta no tiene el componente Card!");
            return;
        }

        cardScript.Configurar(id, sprite, numeroDecimal, this);
        todasLasCartas.Add(cardScript);
    }

    void MezclarCartas()
    {
        // Algoritmo Fisher-Yates para mezclar
        for (int i = todasLasCartas.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            todasLasCartas[j].transform.SetSiblingIndex(i);
        }
    }

    public void CartaSeleccionada(Card carta)
    {
        // Ignorar si el juego está bloqueado
        if (estaBloqueado)
            return;

        // Reproducir sonido de voltear
        ReproducirSonido(sonidoVoltear);

        // Primera carta seleccionada
        if (primeraCarta == null)
        {
            primeraCarta = carta;
            return;
        }

        // Segunda carta seleccionada
        if (segundaCarta == null && carta != primeraCarta)
        {
            segundaCarta = carta;
            StartCoroutine(VerificarPareja());
        }
    }

    IEnumerator VerificarPareja()
    {
        estaBloqueado = true;

        // Esperar un momento para que el jugador vea las cartas
        yield return new WaitForSeconds(0.8f);

        // Verificar si las cartas coinciden
        if (primeraCarta.id == segundaCarta.id)
        {
            // ¡Pareja encontrada!
            ReproducirSonido(sonidoAcierto);
            puntos += 10;
            paresEncontrados++;

            primeraCarta.MarcarComoEmparejada();
            segundaCarta.MarcarComoEmparejada();

            

            // Verificar si ganó
            if (paresEncontrados >= 10)
            {
                yield return new WaitForSeconds(1f);
                MostrarVictoria();
            }
        }
        else
        {
            // No coinciden, voltear de nuevo
            ReproducirSonido(sonidoError);
            StartCoroutine(primeraCarta.Voltear(false));
            StartCoroutine(segundaCarta.Voltear(false));
        }

        // Resetear selección
        primeraCarta = null;
        segundaCarta = null;
        estaBloqueado = false;
    }

    /*void ActualizarPuntos()
    {
        if (textoPuntos != null)
        {
            textoPuntos.text = $"Puntos: {puntos}";
        }
    }*/

    void MostrarVictoria()
    {
        cronometroActivo = false;

        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
            Btn_regresar_1.gameObject.SetActive(false);
            Btn_regresar_2.gameObject.SetActive(false);

            if (textoTiempoFinal != null)
            {
                if(tiempoTranscurrido < 60f) {
                textoTiempoFinal.text = "Tiempo " + tiempoFinalTexto;
                    textoScore.text = "¡Excelente!";
                    textoScore.color = Color.green;
                }else if (tiempoTranscurrido < 120f)
                {
                    textoTiempoFinal.text = "Tiempo " + tiempoFinalTexto;
                    textoScore.text = "¡Muy bien!";
                    textoScore.color = Color.yellow;
                }
                else
                {
                    textoTiempoFinal.text = "Tiempo " + tiempoFinalTexto;
                    textoScore.color = Color.red;
                }
            }
            
        }
        Debug.Log("Ganaste con " + puntos + " puntos en " + tiempoFinalTexto);
    }


    public void ReiniciarJuego()
    {
        puntos = 0;
        paresEncontrados = 0;
        primeraCarta = null;
        segundaCarta = null;
        estaBloqueado = false;
        tiempoTranscurrido = 0f;

        if (panelVictoria != null)
            panelVictoria.SetActive(false);

        GenerarCartas();
        IniciarCronometro();
    }

    void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

   

    void IniciarCronometro()
    {
        tiempoTranscurrido = 0f;
        cronometroActivo = true;
        ActualizarCronometro();
    }

    void ActualizarCronometro()
    {
        if (textoTiempo == null) return;

        int minutos = Mathf.FloorToInt(tiempoTranscurrido / 60f);
        int segundos = Mathf.FloorToInt(tiempoTranscurrido % 60f);

        tiempoFinalTexto = string.Format("{0:00}:{1:00}", minutos, segundos);
        textoTiempo.text = tiempoFinalTexto;

        // Cambiar color según tiempo (opcional)
        if (tiempoTranscurrido < 60f)
            textoTiempo.color = Color.green; // Menos de 1 min
        else if (tiempoTranscurrido < 120f)
            textoTiempo.color = Color.yellow; // Entre 1-2 min
        else
            textoTiempo.color = Color.red; // Más de 2 min
    }
}
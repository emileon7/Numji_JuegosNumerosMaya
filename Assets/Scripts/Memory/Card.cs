using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Card : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject dorso;
    public GameObject frente;
    public Image imagenMaya;
    public TextMeshProUGUI textoDecimal;

    [Header("Configuración")]
    public int id;
    private bool estaVolteada = false;
    private bool estaEmparejada = false;
    private MayaController controller;
    private CanvasGroup canvasGroup;
    private Button boton;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        boton = GetComponent<Button>();

        // Asegurar que el CanvasGroup esté visible
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
        }
    }

    public void Configurar(int _id, Sprite sprite, int numeroDecimal, MayaController ctrl)
    {
        id = _id;
        controller = ctrl;

        // Resetear estado
        estaVolteada = false;
        estaEmparejada = false;

        // Configurar visibilidad inicial
        dorso.SetActive(true);
        frente.SetActive(false);

        // Configurar contenido
        if (sprite != null)
        {
            // Es una carta con imagen maya
            imagenMaya.sprite = sprite;
            imagenMaya.gameObject.SetActive(true);
            textoDecimal.gameObject.SetActive(false);
        }
        else
        {
            // Es una carta con número decimal
            textoDecimal.text = numeroDecimal.ToString();
            textoDecimal.gameObject.SetActive(true);
            imagenMaya.gameObject.SetActive(false);
        }

        // Asegurar alpha en 1
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }

        // Configurar el botón
        boton.onClick.RemoveAllListeners();
        boton.onClick.AddListener(AlHacerClick);
    }

    public void AlHacerClick()
    {
        // No hacer nada si ya está volteada o emparejada
        if (estaVolteada || estaEmparejada)
            return;

        StartCoroutine(Voltear(true));
        controller.CartaSeleccionada(this);
    }

    public IEnumerator Voltear(bool mostrarFrente)
    {
        estaVolteada = mostrarFrente;

        float duracion = 0.25f;
        float tiempo = 0f;
        float escalaInicio = 1f;
        float escalaMedio = 0f;

        // Reducir escala en X (efecto de volteo)
        while (tiempo < duracion / 2)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempo / (duracion / 2);
            float escala = Mathf.Lerp(escalaInicio, escalaMedio, progreso);
            transform.localScale = new Vector3(escala, 1, 1);
            yield return null;
        }

        // Cambiar cara
        dorso.SetActive(!mostrarFrente);
        frente.SetActive(mostrarFrente);

        // Aumentar escala de vuelta
        tiempo = 0f;
        while (tiempo < duracion / 2)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempo / (duracion / 2);
            float escala = Mathf.Lerp(escalaMedio, escalaInicio, progreso);
            transform.localScale = new Vector3(escala, 1, 1);
            yield return null;
        }

        transform.localScale = Vector3.one;
    }

    public void MarcarComoEmparejada()
    {
        estaEmparejada = true;
        StartCoroutine(Desaparecer());
    }

    IEnumerator Desaparecer()
    {
        yield return new WaitForSeconds(0.5f);

        float duracion = 0.5f;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, tiempo / duracion);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public bool EstaVolteada()
    {
        return estaVolteada;
    }

    public bool EstaEmparejada()
    {
        return estaEmparejada;
    }
}
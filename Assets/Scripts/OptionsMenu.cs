using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [Header("Referencias UI")]
    public Slider sliderVolumen;
    public TextMeshProUGUI textoVolumen;

    public Button botonCerrar;

    void Start()
    {
        // Cargar valores actuales
        CargarValores();

        // Configurar listeners
        if (sliderVolumen != null)
        {
            sliderVolumen.onValueChanged.AddListener(OnVolumenCambiado);
        }



        if (botonCerrar != null)
        {
            botonCerrar.onClick.AddListener(CerrarMenu);
        }
    }

    void CargarValores()
    {
        if (MusicManager.Instance == null) return;

        // Cargar volumen actual
        float volumenActual = MusicManager.Instance.GetVolumen();

        if (sliderVolumen != null)
        {
            sliderVolumen.value = volumenActual;
        }


        ActualizarTexto(volumenActual);
    }

    void OnVolumenCambiado(float valor)
    {
        // Cambiar volumen en tiempo real
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetVolumen(valor);
        }

        ActualizarTexto(valor);
    }


    void ActualizarTexto(float volumen)
    {
        if (textoVolumen != null)
        {
            int porcentaje = Mathf.RoundToInt(volumen * 100);
            textoVolumen.text = $"Volumen: {porcentaje}%";
        }
    }

    void CerrarMenu()
    {
        gameObject.SetActive(false);
    }

    // Método público para abrir el menú
    public void AbrirMenu()
    {
        gameObject.SetActive(true);
        CargarValores();
    }
}
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Música de Fondo")]
    public AudioClip musicaFondo;

    [Header("Volumen Inicial")]
    [Range(0f, 1f)]
    public float volumen = 0.5f;

    private AudioSource audioSource;

    void Awake()
    {
        // Singleton - Solo una instancia
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // No destruir al cambiar de escena

        // Crear AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        // Cargar volumen guardado
        CargarVolumen();
    }

    void Start()
    {
        // Reproducir música si hay un clip asignado
        if (musicaFondo != null && !audioSource.isPlaying)
        {
            audioSource.clip = musicaFondo;
            audioSource.volume = volumen;
            audioSource.Play();
        }
    }

    /// <summary>
    /// Cambiar el volumen de la música
    /// </summary>
    public void SetVolumen(float nuevoVolumen)
    {
        volumen = Mathf.Clamp01(nuevoVolumen);
        audioSource.volume = volumen;
        GuardarVolumen();
    }

    /// <summary>
    /// Obtener el volumen actual
    /// </summary>
    public float GetVolumen()
    {
        return volumen;
    }

    /// <summary>
    /// Pausar la música
    /// </summary>
    public void Pausar()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    /// <summary>
    /// Reanudar la música
    /// </summary>
    public void Reanudar()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }

    /// <summary>
    /// Silenciar/Desilenciar
    /// </summary>
    public void ToggleMute()
    {
        audioSource.mute = !audioSource.mute;
    }

    /// <summary>
    /// Verificar si está silenciado
    /// </summary>
    public bool EstaSilenciado()
    {
        return audioSource.mute;
    }

    /// <summary>
    /// Guardar volumen en PlayerPrefs
    /// </summary>
    void GuardarVolumen()
    {
        PlayerPrefs.SetFloat("VolumenMusica", volumen);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Cargar volumen desde PlayerPrefs
    /// </summary>
    void CargarVolumen()
    {
        if (PlayerPrefs.HasKey("VolumenMusica"))
        {
            volumen = PlayerPrefs.GetFloat("VolumenMusica");
        }
    }
}
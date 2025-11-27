using UnityEngine;
using TMPro; // para usar TextMeshPro

public class NumeroAleatorioJaguar : MonoBehaviour
{
    public TMP_Text numeroText; // aquí arrastraremos el TextMeshPro desde Unity
    private int numeroAleatorio;

    void Start()
    {
        GenerarNumero(); // genera un número al iniciar el juego
    }

    public void GenerarNumero()
    {
        numeroAleatorio = Random.Range(0, 20); // número del 1 al 20
        numeroText.text = numeroAleatorio.ToString(); // lo muestra en el TMP
        Debug.Log("Número aleatorio: " + numeroAleatorio);
    }

    public int ObtenerNumero()
    {
        return numeroAleatorio; // para verificar después
    }
}

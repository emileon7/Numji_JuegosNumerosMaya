using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  
using TMPro;

public class Verificador_Nivel1 : MonoBehaviour
{
    public NumeroAleatorioJaguar numeroAleatorioJaguar; // Referencia al script que genera el número aleatorio

    public TextMeshProUGUI resultadoText; // Texto para mostrar el resultado de ejercicios
    private int totalEjercicios = 5;
    private int totalActual = 1;
    private int puntajeTotal = 0;
    private int puntajePorEjercicio = 20;

    public Image Jaguar; // Imagen del jaguar
    public Sprite JaguarFeliz;
    public Sprite JaguarTriste;
    public Sprite JaguarNeutro;

    //Botones de interacción
    public Button Btn_verificar;
    public Button Btn_siguiente;
    public Button Btn_Finalizar;

    public Transform Tablilla; // Contenedor de los objetos arrastrables

    void Start()
    {

        Btn_verificar.onClick.AddListener(VerificarRespuesta);
        Btn_siguiente.onClick.AddListener(SiguienteEjercicio);
        


        Btn_verificar.gameObject.SetActive(true);

        Btn_siguiente.gameObject.SetActive(false);//ocultar
        Btn_Finalizar.gameObject.SetActive(false);

        Btn_verificar.interactable = true; // Habilitar el botón de verificar al inicio

        resultadoText.text = totalActual + "/" + totalEjercicios;

    }

    int obtenerValorTablilla()
    {
        int total = 0;
        foreach(Transform child in Tablilla)
        {
            Draggable item = child.GetComponent<Draggable>();
            if (item != null)
            {
                total+= item.valor;

            }

        }
        return total;
    }

  
  

    public void VerificarRespuesta()
    {
        int obtenerValor = obtenerValorTablilla();

        int numeroCorrecto = numeroAleatorioJaguar.ObtenerNumero();
        Debug.Log("Numero correcto: "+ numeroCorrecto);
        Debug.Log("Valor obtenido: "+ obtenerValor);

        if (obtenerValor == numeroCorrecto)
        {
           
            Jaguar.sprite = JaguarFeliz;
            numeroAleatorioJaguar.gameObject.SetActive(false);
            puntajeTotal += puntajePorEjercicio;
        }
        else
        {
            Jaguar.sprite = JaguarTriste;
            numeroAleatorioJaguar.gameObject.SetActive(false);
        }

        Btn_verificar.interactable = false;
        Btn_siguiente.gameObject.SetActive(true);

        if (totalActual == 5)
        {
            Btn_siguiente.gameObject.SetActive(false);
            Btn_verificar.gameObject.SetActive(false);
            //SceneManager.LoadScene("NivelCompletado");
            Btn_Finalizar.onClick.AddListener(() => SceneManager.LoadScene("Nivel1_Fin"));
            Debug.Log("Nivel Completado");

            Btn_Finalizar.gameObject.SetActive(true);

            PlayerPrefs.SetInt("PuntajeFinal", puntajeTotal);
            PlayerPrefs.Save();

            return;
        }
    }
   
    public void SiguienteEjercicio()
    {
        totalActual++;
        resultadoText.text = totalActual + "/" + totalEjercicios;
        Debug.Log("Ejercicio actual: " + totalActual);

       

        Jaguar.sprite = JaguarNeutro;
        numeroAleatorioJaguar.gameObject.SetActive(true);
        numeroAleatorioJaguar.GenerarNumero();

        Btn_verificar.interactable = true;
        Btn_siguiente.gameObject.SetActive(false);

        

        foreach (Transform child in Tablilla) 
        {

            Draggable item = child.GetComponent<Draggable>();
            if (item != null)
            {
                Destroy(item.gameObject);

            }

        }

    }

    

}
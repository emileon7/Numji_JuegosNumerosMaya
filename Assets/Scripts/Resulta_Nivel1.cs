using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Resulta_Nivel1 : MonoBehaviour
{
    
    public TextMeshProUGUI resultado_final;
    public TextMeshProUGUI texto_resultado;
    public Image Score;
    public Sprite one_start;
    public Sprite two_start;
    public Sprite tree_start;

    void Start()
    {
        MostrarResultado();

    }

    void MostrarResultado()
    {
        int puntaje = PlayerPrefs.GetInt("PuntajeFinal", 0);
        
        resultado_final.text = puntaje.ToString() + "%";

        if(puntaje == 0)
        {
            Score.sprite = null;
            texto_resultado.color = Color.red;
            texto_resultado.text = "¡Inténtalo de nuevo!";

        }
        else if (puntaje <= 60)
        {
            Score.sprite = one_start;
            texto_resultado.color = Color.yellow;
            texto_resultado.text = "¡Puedes mejorar!";

        }
        else if(puntaje <= 80)
        {
            Score.sprite = two_start;
            texto_resultado.color = Color.blue;
            texto_resultado.text = "¡Muy bien!";
        }
        else if(puntaje == 100)
        {
            Score.sprite = tree_start;
            texto_resultado.color = Color.green;
            texto_resultado.text = "¡Excelente!, felicidades";
        }
    }

}

using UnityEngine;
using UnityEngine.SceneManagement;


public class BTN : MonoBehaviour
{

    public void Regresar_SelectedLvel()
    {
        string escena_actual = "SelectedLevel";
        SceneManager.LoadScene(escena_actual);
    }

    public void Regresar()
    {
        int escena_actual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(escena_actual - 1);
    }


    public void Seleccionar_Nivel()
    {
        SceneManager.LoadScene("SelectedLevel");
    }

    public void Salir()
    {
        Debug.Log("Salir");
        Application.Quit();
    }

    public void Nivel_1()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void Nivel_2()
    {
        SceneManager.LoadScene("Nivel2");

    }

    public void Nivel_3()
    {
        SceneManager.LoadScene("Nivel3");

    }

    public void Menu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void CargarEscenaPlataformas()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("Acto2");
    }
}

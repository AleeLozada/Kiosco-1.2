using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    public TextMeshProUGUI textoMotivo;
    void Start()
    {
        Time.timeScale = 1f;
        if (GestorGanancias.Instancia != null && textoMotivo != null)
        {
            textoMotivo.text = GestorGanancias.Instancia.motivoFinJuego;
        }
    }
    public void VolverAlInicio()
    {
        if (GestorGanancias.Instancia != null) GestorGanancias.Instancia.ReiniciarJuegoCompleto();
        else SceneManager.LoadScene("Acto1");
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGanancias : MonoBehaviour
{
    [Header("Textos")]
    public TextMeshProUGUI textoGananciasDia;
    public TextMeshProUGUI textoPendiente;
    public TextMeshProUGUI textoAhorroTotal;
    public TextMeshProUGUI textoDia;
    [Header("Solo completar en el Acto 1 (dejar vacío en el Acto 2)")]
    public GeneradorClientes generadorClientes;

    public Slider barraConfianza;
    public TextMeshProUGUI textoPorcentaje;
    private void OnEnable()
    {
        GestorGanancias.OnValoresActualizados += ActualizarUI;
        ActualizarUI();
    }
    private void OnDisable()
    {
        GestorGanancias.OnValoresActualizados -= ActualizarUI;
    }
    private void Update()
    {
        // La fila cambia todo el tiempo, así que esta sí se refresca cada frame
        if (generadorClientes != null && textoPendiente != null && GestorGanancias.Instancia != null)
        {
            int pendienteJugador = Mathf.RoundToInt(generadorClientes.ObtenerGananciaPendienteEnFila() * GestorGanancias.Instancia.porcentajeActual);
            textoPendiente.text = "Por cobrar: $" + pendienteJugador;
        }
    }
    void ActualizarUI()
    {
        if (textoPorcentaje != null) textoPorcentaje.text = "Tu parte: " + Mathf.RoundToInt(GestorGanancias.Instancia.porcentajeActual * 100f) + "%";
        if (GestorGanancias.Instancia == null) return;
        if (textoGananciasDia!=null) textoGananciasDia.text = "Hoy: $" + GestorGanancias.Instancia.gananciasDelDia;
        if (textoAhorroTotal != null) textoAhorroTotal.text = "Ahorro total: $" + GestorGanancias.Instancia.ahorroTotal;
        if (textoDia != null) textoDia.text = "Día " + GestorGanancias.Instancia.diaActual + " - " + GestorGanancias.Instancia.ObtenerFechaFormateada();
        if (barraConfianza != null) barraConfianza.value = GestorGanancias.Instancia.confianza / 100f;
    }
}

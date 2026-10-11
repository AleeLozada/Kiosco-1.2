using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class ControlTurno : MonoBehaviour
{
    [Header("UI del Reloj")]
    public TextMeshProUGUI textoReloj;

    [Header("UI de Cierre")]
    public GameObject botonSiguienteNivel;

    [Tooltip("Duración real en segundos de una hora de juego")]
    public float segundosPorHora = 90f;

    [Header("Referencias")]
    public GeneradorClientes generador;

    private float tiempoTranscurrido = 0f;
    public int horaActual = 12;
    private int minutoActual = 0;
    private bool turnoActivo = true;
    private bool esperandoUltimoCliente = false;

    private void Start()
    {
        ActualizarTextoReloj();

        if (generador == null) generador = GameObject.FindAnyObjectByType<GeneradorClientes>();

        if (botonSiguienteNivel != null) botonSiguienteNivel.SetActive(false);
    }
    private void Update()
    {
        if (!turnoActivo) return;


        if (esperandoUltimoCliente)
        {
            if (generador != null && generador.ObtenerCantidadEnFila() == 0)
            {
                TerminarJuegoDefinitivo();
            }
            return;
        }

        tiempoTranscurrido += Time.deltaTime;

        float fraccionDeHora = tiempoTranscurrido / segundosPorHora;
        int horasPasadas = Mathf.FloorToInt(fraccionDeHora);

        int horaSimulada = 12 + horasPasadas;
        int minutoSimulado = Mathf.FloorToInt((fraccionDeHora - horasPasadas) * 60f);

        if (horaSimulada != horaActual || minutoSimulado != minutoActual)
        {
            horaActual = horaSimulada;
            minutoActual = minutoSimulado;
            ActualizarTextoReloj();

            if (horaActual >= 22)
            {
                CerrarPuertasDelKiosco();
            }
        }
    }
    void ActualizarTextoReloj()
    {
        if (textoReloj != null)
        {
            textoReloj.text = $"{horaActual:D2}:{minutoActual:D2} {(horaActual < 12 ? "AM":"PM")}";
        }
    }
    void CerrarPuertasDelKiosco()
    {
        esperandoUltimoCliente = true;
        horaActual = 22;
        minutoActual = 0;
        ActualizarTextoReloj();
        if (generador != null)
        {
            generador.ApagarSpawn();
        }
    }

    void TerminarJuegoDefinitivo()
    {
        turnoActivo = false;
        Debug.Log("¡¡Día de trabajo completado!!");
        if (botonSiguienteNivel != null)
        {
            botonSiguienteNivel.SetActive(true);
        }
    }
}

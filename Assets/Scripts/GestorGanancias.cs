using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class GestorGanancias : MonoBehaviour
{
    public static GestorGanancias Instancia;
    [Header("Ganancias del día")]
    public int gananciasDelDia = 0;
    [Header("Ahorro total(entre días)")]
    public int ahorroTotal = 0;
    [Header("Confianza del dueño")]
    [Range(0, 100)]
    public int confianza = 50;
    public int confianzaMinimaDespido = 0;
    public int confianzaPorDiaCorrecto = 5;
    public int confianzaPorAtencionRapida = 2;
    public int confianzaPorAtencionLenta = 3;
    public int confianzaPorErrorPedido = 10;
    public static event Action OnDespedido;
    [Header("Dinero generado de la venta")]
    [Range(0f, 1f)]
    public float porcentajeJugador = 0.5f; // % de lo ganado hoy que pasa al ahorro
    public float penalizacionPorError = 0.02f;
    public float porcentajeMinimo = 0f;
    public float porcentajeActual = 0.5f;
    [Header("Robos")]
    public int vecesRobado = 0;
    public int maxRobados = 2;
    [Header("Días")]
    public int diaActual = 1;
    public int diaInicial = 1;
    public int mesInicial = 3;
    public int anioInicial = 2000;

    [Header("Mensaje de día perdido")]
    public float tiempoMensajePerdida = 2.2f;
    // Otros scripts (la UI) se suscriben a esto en vez de consultar cada frame
    public static event Action OnValoresActualizados;
    public static event Action OnDiaPerdido;
    [Header("Alquiler")]
    public int diasPorPago = 5;
    public int montoAlquiler = 100;
    private int diasDesdeUltimoPago = 0;
    private bool juegoTerminado = false;
    public string motivoFinJuego = "";
    public static event Action<int> OnAlquilerCobrado;
    public static event Action OnNoPudoPagarAlquiler;
    private void Awake()
    {
        porcentajeActual = porcentajeJugador;
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AgregarGanancia(int montoVenta)
    {
        int parteJugador = Mathf.RoundToInt(montoVenta * porcentajeActual);
        gananciasDelDia += montoVenta;
        OnValoresActualizados?.Invoke();
    }
    public void RegistrarError()
    {
        porcentajeActual = Mathf.Max(porcentajeMinimo, porcentajeActual - penalizacionPorError);
        Debug.Log($"Error de pedido. Tu parte baja a {porcentajeActual * 100f:0}%");
        OnValoresActualizados?.Invoke();
    }
    public void AumentarConfianza(int monto)
    {
        confianza = Mathf.Min(100, confianza + monto);
        OnValoresActualizados?.Invoke();
    }
    public void DisminuirConfianza(int monto)
    {
        confianza = Mathf.Max(0, confianza - monto);
        OnValoresActualizados?.Invoke();
        if (confianza <= confianzaMinimaDespido)
        {
            Despedido();
        }
    }
    
    public void Robar()
    {
        vecesRobado++;
        Debug.Log("¡Te robaron! Robos: " + vecesRobado + "/" + maxRobados);
        OnValoresActualizados?.Invoke();
        if (vecesRobado >= maxRobados)
        {
            PerderDia();
        }
    }
    void PerderDia()
    {
        Debug.Log("Perdiste el día. No se agrega nada al ahorro.");
        gananciasDelDia = 0;
        vecesRobado = 0;
        AvanzarDia();
        OnDiaPerdido?.Invoke();
        StartCoroutine(CargarEscenaConDemora(tiempoMensajePerdida));
    }
    
    IEnumerator CargarEscenaConDemora(float demora)
    {
        if (demora > 0f) yield return new WaitForSeconds(demora);
        SceneManager.LoadScene(juegoTerminado ? "GameOver" : "Acto1");
    }
    public void TerinarDia()
    {
        ahorroTotal += gananciasDelDia;

        Debug.Log($"Día terminado. Te pagaron: {gananciasDelDia}, Ahorro total: {ahorroTotal}");

        gananciasDelDia = 0;
        vecesRobado = 0;
        porcentajeActual = porcentajeJugador;
        AumentarConfianza(confianzaPorDiaCorrecto);
        bool huboCobro = AvanzarDia();
        StartCoroutine(CargarEscenaConDemora(huboCobro ? tiempoMensajePerdida : 0f));
    }
    bool AvanzarDia()
    {
        diaActual++;
        diasDesdeUltimoPago++;
        bool huboCobro = false;

        if (diasDesdeUltimoPago >= diasPorPago)
        {
            diasDesdeUltimoPago = 0;
            huboCobro = true;
            CobrarAlquiler();
        }

        OnValoresActualizados?.Invoke();
        return huboCobro;
    }
    void CobrarAlquiler()
    {
        if (ahorroTotal >= montoAlquiler)
        {
            ahorroTotal -= montoAlquiler;
            Debug.Log($"Pagaste alquiler : -${montoAlquiler}. Ahorro restante: {ahorroTotal}");
            OnAlquilerCobrado?.Invoke(montoAlquiler);
        }
        else
        {
            Debug.Log("No pudiste pagar el alquiler este mes. Fin del Juego.");
            juegoTerminado = true;
            motivoFinJuego = "No pudiste pagar el alquiler. \nTe echaron de donde alquilavas";
            OnNoPudoPagarAlquiler?.Invoke();
            
        }
    }
    public int ObtenerDiasHastaProximoPago()
    {
        return diasPorPago - diasDesdeUltimoPago;
    }
    public string ObtenerFechaFormateada()
    {
        DateTime fecha = new DateTime(anioInicial, mesInicial, diaInicial).AddDays(diaActual - 1);
        return fecha.ToString("dd/MM/yyyy");
    }
    void Despedido()
    {
        Debug.Log("Te despidieron. Fin del juego.");
        OnDespedido?.Invoke();
        motivoFinJuego = "Te despidieron del kiosco.";
        SceneManager.LoadScene("GameOver"); // escena NUEVA
    }
    public void ReiniciarJuegoCompleto()
    {
        ahorroTotal = 0;
        diaActual = 1;
        confianza = 50;
        gananciasDelDia = 0;
        vecesRobado = 0;
        SceneManager.LoadScene("Acto0"); //Recordar dejar en Acto0
        juegoTerminado = false;
        diasDesdeUltimoPago = 0;
        motivoFinJuego = "";
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Caja : MonoBehaviour
{
    [Header("UI Componentes")]
    public Slider barraCobro;
    public GameObject canvasCobro;

    [Header("Configuración")]
    public float tiempoCobranza = 2.0f;
    public int montoPorVenta = 50;
    private bool estaCobrando = false;

    private Cliente clienteActual; // Guarda al cliente que esta en el mostrador

    public PortaPapel portaPapel;
    [Header("Confianza")]
    public float tiempoLimiteAtencionRapida = 5f;
    public int maxIntentosFallidos = 2;

    private float tiempoLlegadaCliente;
    private int intentosFallidos = 0;

    private void Start()
    {
        // Validar para evitar errores
        if (canvasCobro != null) canvasCobro.SetActive(false);
        if (barraCobro != null) barraCobro.value = 0;
    }
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (clienteActual != null)
            {
                if (portaPapel != null)
                {
                    portaPapel.Abrir(this);
                }
                else
                {
                    Debug.LogError("Falta asignar el PortaPapel en el inspector de la Caja.");
                }
            }
            else
            {
                Debug.LogWarning("Intentaste cobrar, pero no hay ningún cliente");
            }
        }
    }
    
    public void RecibirTotalDelPortaPapel(int totalSeleccionado)
    {
        if (clienteActual == null) return;
        int totalCorrecto = clienteActual.ObtenerTotalPedido();
        if (totalSeleccionado == totalCorrecto)
        {
            montoPorVenta = totalSeleccionado;
            float tiempoTardado = Time.time - tiempoLlegadaCliente;
            if (GestorGanancias.Instancia != null)
            {
                if (tiempoTardado <= tiempoLimiteAtencionRapida) GestorGanancias.Instancia.AumentarConfianza(GestorGanancias.Instancia.confianzaPorAtencionRapida);
                else GestorGanancias.Instancia.DisminuirConfianza(GestorGanancias.Instancia.confianzaPorAtencionLenta);
            }

            IniciarCobranza();
        }
        else
        {
            intentosFallidos++;
            GestorGanancias.Instancia?.RegistrarError();
            Debug.LogWarning($"Selección incorrecta. Pediste ${totalSeleccionado}, el pedido era ${totalCorrecto}");
            if (intentosFallidos >= maxIntentosFallidos)
            {
                ClienteSeVaEnojado();
            }
        }
    }
    void ClienteSeVaEnojado()
    {
        Debug.Log("El cliente se fue sin pagar por un error en el pedido.");
        GestorGanancias.Instancia?.DisminuirConfianza(GestorGanancias.Instancia.confianzaPorErrorPedido);
        if (clienteActual != null)
        {
            clienteActual.IrseSinPagar();
            clienteActual = null;
        }
    }
    // Detecta de forma fisica cuando el cliente toca la caja
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (clienteActual == null && collision.CompareTag("Cliente"))
        {
            clienteActual = collision.GetComponent<Cliente>();
            tiempoLlegadaCliente = Time.time;
            intentosFallidos = 0;
        }
    }
    public void IniciarCobranza()
    {
        if (barraCobro == null || canvasCobro == null)
        {
            Debug.LogError("Por favor, asignar la barra de cobro y el canvas en el inspector de la caja.");
            return;
        }
        if (!estaCobrando)
        {
            StartCoroutine(ProcesoCobroCo());
        }
    }
    private IEnumerator ProcesoCobroCo()
    {
        estaCobrando = true;
        canvasCobro.SetActive(true);
        barraCobro.value = 0;

        //Guarda una copia segura del cliente antes de que empiece a cargar la barra
        Cliente clienteAFrenarYborrar = clienteActual;

        float tiempoTranscurrido = 0;
        while (tiempoTranscurrido < tiempoCobranza)
        {
            tiempoTranscurrido += Time.deltaTime;
            barraCobro.value = tiempoTranscurrido / tiempoCobranza;
            yield return null;
        }

        //finaliza la carga visual
        estaCobrando = false;
        canvasCobro.SetActive(false);
        Debug.Log("Barra de cobro llena");

        // Destrucción: Dar la orden de desaparecer al cliente guardado
        if(clienteAFrenarYborrar != null)
        {
            if (GestorGanancias.Instancia != null)
            {
                GestorGanancias.Instancia.AgregarGanancia(montoPorVenta);
            }
            clienteAFrenarYborrar.DesaparecerCliente();
            clienteActual = null; // Vacia la caja para el sieguiente cliente!
        }
        else
        {
            Debug.LogError("Error: ka barra terminó pero (ClienteActual) se volvió NULL durnte el proceso.");
        }
    }
}
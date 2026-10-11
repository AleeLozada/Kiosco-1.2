using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using TMPro;
public class Cliente : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 3.0f;
    private Transform destinoActual;
    [Header("Pedido")]
    public List<Producto> pedido = new List<Producto>();
    public TextMeshProUGUI textoPedido; // Texto flotante sobre el cliente (World Space Canvas)
    [Header("Salida")]
    public Transform puntoSalida;
    private bool saliendoDeEscena = false;
    void Update()
    {
        //El generador le da un punto de fila, se movera hacia el
        if(destinoActual != null)
        {
            float nuevoX = Mathf.MoveTowards(
                transform.position.x,
                destinoActual.position.x,
                velocidad * Time.deltaTime
            );
            transform.position = new Vector3(
                nuevoX,
                transform.position.y,
                transform.position.z
            );
            if (saliendoDeEscena && Mathf.Abs(transform.position.x - destinoActual.position.x) < 0.05f)
            {
                Destroy(gameObject);
            }
        }
    }

    //Metodo para detener al cliente cuando choque con el mostrador
    public void AsignarNuevoDestino(Transform nuevoPunto)
    {
        destinoActual = nuevoPunto;
    }

    // Genera un pedido aleatorio a partir del catálogo completo del kiosco
    public void GenerarPedido(List<Producto> catalogoCompleto, int minProductos = 1, int maxProductos = 4)
    {
        pedido.Clear();
        List<Producto> disponibles = new List<Producto>(catalogoCompleto);

        int cantidad = UnityEngine.Random.Range(minProductos, maxProductos + 1);
        cantidad = Mathf.Min(cantidad, disponibles.Count);

        for (int i = 0; i < cantidad; i++)
        {
            int indice = UnityEngine.Random.Range(0, disponibles.Count);
            pedido.Add(disponibles[indice]);
            disponibles.RemoveAt(indice);
        }
        MostrarPedidoTexto();
    }

    void MostrarPedidoTexto()
    {
        if (textoPedido == null) return;

        string texto = "Hola! Quisiera comprar: ";
        for (int i = 0; i < pedido.Count; i++)
        {
            texto += pedido[i].nombre;
            if (i < pedido.Count - 1) texto += ", ";
        }
        textoPedido.text = texto;
    }
    public int ObtenerTotalPedido()
    {
        int total = 0;
        foreach (Producto p in pedido) total += p.precio;
        return total;

    }
    public void IrseSinPagar()
    {
        Debug.Log("Cliente se va enojado sin pagar.");
        if (puntoSalida != null)
        {
            saliendoDeEscena = true;
            destinoActual = puntoSalida;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Este metodo lo llamara la caja registradora al terminar el cobro
    public void DesaparecerCliente()
    {
        // Acá podrian sumar puntos antes de destruirlo
        Debug.Log("Cliente satisfecho se retira.");
        if (puntoSalida != null)
        {
            saliendoDeEscena = true;
            destinoActual = puntoSalida;
        }
        else
        {
            Debug.LogWarning("No se asigó un Punto de salida al cliente. Se destruye directo.");
            Destroy(gameObject);
        }
    }
}

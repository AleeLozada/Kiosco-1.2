using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PortaPapel : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelPortaPapel;
    public TextMeshProUGUI textoTotral;
    public TextMeshProUGUI textoSeleccion;

    [Header("Botones de productos (uno por cada producto del catálogo)")]
    public List<BotonProducto> botones;
    private List<Producto> seleccionados = new List<Producto>();
    private Caja cajaAsociada;

    [System.Serializable]
    public class BotonProducto
    {
        public Producto producto;
        public Button boton;
    }
    private void Start()
    {
        if (panelPortaPapel != null) panelPortaPapel.SetActive(false);

        //Conecta cada botón con su producto correspondiente
        foreach (BotonProducto bp in botones)
        {
            Producto p = bp.producto; // Copia local para el closure del lambda
            bp.boton.onClick.AddListener(() => AgregarProducto(p));
        }
    }
    public void Abrir(Caja caja)
    {
        cajaAsociada = caja;
        seleccionados.Clear();
        ActualizarUI();
        if(panelPortaPapel != null) panelPortaPapel.SetActive(true);
    }
    public void Cerrar()
    {
        if (panelPortaPapel != null) panelPortaPapel.SetActive(false);
    }
    void AgregarProducto(Producto producto)
    {
        seleccionados.Add(producto);
        ActualizarUI();
    }
    public void QuitarUltimo()
    {
        if (seleccionados.Count > 0)
        {
            seleccionados.RemoveAt(seleccionados.Count - 1);
            ActualizarUI();
        }
    }
    void ActualizarUI()
    {
        int total = 0;
        string listado = "";
        foreach (Producto p in seleccionados)
        {
            total += p.precio;
            listado += p.nombre + "\n";
        }
        if (textoTotral != null) textoTotral.text = "Total: $" + total;
        if (textoTotral != null) textoSeleccion.text = listado;
    }
    public void ConfirmarSeleccion()
    {
        if (cajaAsociada == null) return;
        int totalSeleccionado = 0;
        foreach (Producto p in seleccionados) totalSeleccionado += p.precio;
        cajaAsociada.RecibirTotalDelPortaPapel(totalSeleccionado);
        Cerrar();
    }
}

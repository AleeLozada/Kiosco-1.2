using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIFinDia : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelPerdida;
    public TextMeshProUGUI textoPerdida;
    private void OnEnable()
    {
        GestorGanancias.OnDiaPerdido += MostrarMensajePerdida;
        GestorGanancias.OnAlquilerCobrado += MostrarAlquilerPagado;
        GestorGanancias.OnNoPudoPagarAlquiler += MostrarAlquilerImpago;
    }
    private void OnDisable()
    {
        GestorGanancias.OnDiaPerdido -= MostrarMensajePerdida;
    }
    void MostrarMensajePerdida()
    {
        if (panelPerdida != null) panelPerdida.SetActive(true);
        if (textoPerdida != null) textoPerdida.text = "¡Te robaron todo! \nPerdiste el dia.";
    }
    void MostrarAlquilerPagado(int monto)
    {
        if (panelPerdida != null) panelPerdida.SetActive(true);
        if (textoPerdida != null) textoPerdida.text = $"Pagaste el alquiler y los servicios: -${monto}";
    }
    void MostrarAlquilerImpago ()
    {
        if (panelPerdida != null) panelPerdida.SetActive(true);
        if (textoPerdida != null) textoPerdida.text = $"No pudiste pagar el alquiler esta semana. \nTe echaron de la vivienda.";
    }
}

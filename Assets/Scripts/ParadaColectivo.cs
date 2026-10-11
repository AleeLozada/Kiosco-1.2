using UnityEngine;

public class ParadaColectivo : MonoBehaviour
{
    private bool yaActivado = false;
    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (!yaActivado && otro.CompareTag("Player"))
        {
            yaActivado = true;
            Debug.Log("¡Llegaste a la parada! Fin del día.");
            GestorGanancias.Instancia?.TerinarDia();
        }
    }
}

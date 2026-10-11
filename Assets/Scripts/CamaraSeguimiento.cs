using UnityEngine;

public class CamaraSeguimiento : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Objetivo")]
    public Transform jugador;
    [Header("Suavizado")]
    public float velocidadSeguimiento = 5f;
    public Vector3 offset = new Vector3(0, 0, -10f);

    [Header("Límites del escenario")]
    public float limiteIzquierdo;
    public float limiteDerecho;
    public bool usarLimites = true;

    private void LateUpdate()
    {
        if (jugador == null) return;
        Vector3 posicionDesada = jugador.position + offset;
        if (usarLimites)
        {
            posicionDesada.x = Mathf.Clamp(posicionDesada.x, limiteIzquierdo, limiteDerecho);
        }
        transform.position = Vector3.Lerp(transform.position, posicionDesada, velocidadSeguimiento * Time.deltaTime);
    }
}

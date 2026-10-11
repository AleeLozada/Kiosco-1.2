using UnityEngine;

public class Ladron : MonoBehaviour
{
    public float velocidad = 3f;
    public Transform jugador;
    public float distanciaDeteccion = 5f;
    public float tiempoVidaSinDetectar = 6f;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        if (jugador == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) jugador = playerObj.transform;
        }
        Destroy(gameObject, tiempoVidaSinDetectar);
    }
    private void FixedUpdate()
    {
        if (jugador == null) return;
        float distancia = Vector2.Distance(transform.position, jugador.position);
        if (distancia <= distanciaDeteccion)
        {
            float direccionX = jugador.position.x > transform.position.x ? 1f : -1f;
            rb.linearVelocity = new Vector2(direccionX * velocidad, rb.linearVelocity.y);
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Abs(escala.x) * direccionX;
            transform.localScale = escala;
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }
    public void AtraparPlayer()
    {
        GestorGanancias.Instancia?.Robar();
        Destroy(gameObject);
    }
}

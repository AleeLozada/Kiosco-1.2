using UnityEngine;
using UnityEngine.InputSystem;
public class MovimientoJugador2D : MonoBehaviour
{
    [Header("Parámetros de Movimiento")]
    public float velocidad = 6.0f;
    public float fuerzaSalto = 10.0f;

    [Header("Detección de Suelo")]
    public Transform verificadorSuelo; // un objeto vacio en los pies del personaje
    //public float radioVerificacion = 0.25f;
    public Vector2 VerificacionCuadrado = new Vector2(0.25f, 0.25f);
    public LayerMask capaSuelo; // Selecciona la capa del suelo en el Inspector

    private Rigidbody2D rb;
    private float movimientoHorizontal;
    private bool estaEnElSuelo;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Vuelta();
        // Leer el INPUT
        if (Keyboard.current != null)
        {
            // Camina con flechas o A/D
            float izquierda = Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? -1f : 0f ;
            float derecha = Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1f : 0f;

            movimientoHorizontal = izquierda + derecha;

            // Saltar con espacio, W o flecha arriba
            if ((Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame) && estaEnElSuelo)
            {
                Saltar();
            }
        }
        // Verifica si toca el suelo
        // Usando un pequeña esfera invisible en los pies para saber si está en el suelo
        estaEnElSuelo = Physics2D.OverlapBox(verificadorSuelo.position, VerificacionCuadrado,0, capaSuelo);
    }
    void FixedUpdate()
    {
        // aplicamos la velocidad en el eje X manteniendo la velocidad del eje Y (gravedad)
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidad, rb.linearVelocity.y);
    }

    void Saltar()
    {
       rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
    }

    // Dibuja un circculo rojo en el editor para que pueda ver el sensor de suelo
    private void OnDrawGizmosSelected()
    {
        if (verificadorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(verificadorSuelo.position, VerificacionCuadrado);
        }
    }
    void Vuelta()
    {
        if (movimientoHorizontal == 0) return;
        bool movimientoDerecha = movimientoHorizontal > 0;
        bool frenteDerecha = transform.localScale.x > 0;
        if (movimientoDerecha != frenteDerecha)
        {
            Vector3 escala = transform.localScale;
            escala.x *= -1;
            transform.localScale = escala;
        }
    }
}

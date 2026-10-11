using UnityEngine;

public class DetectorLadron : MonoBehaviour
{
    private Ladron ladronPadre;
    private void Awake()
    {
        ladronPadre = GetComponentInParent<Ladron>();
    }
    void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Player"))
        {
            ladronPadre?.AtraparPlayer();
        }
    }
}

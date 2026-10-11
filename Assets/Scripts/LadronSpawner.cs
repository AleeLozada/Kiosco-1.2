using UnityEngine;

public class LadronSpawner : MonoBehaviour
{
    public GameObject prefabLadron;
    public Transform jugador;
    public float timeEntreSpawns = 4f;
    public float distanciaSpawnDelPlayer = 8f;
    public float alturaSpawn = 0f;

    private void Start()
    {
        InvokeRepeating(nameof(GeneradorLadron), 2f, timeEntreSpawns);
    }
    void GeneradorLadron()
    {
        if (jugador == null) return;
        float direccion = Random.value > 0.5f ? 1f : -1f;
        Vector3 posicionSpawn = jugador.position + new Vector3(direccion * distanciaSpawnDelPlayer, alturaSpawn, 0);
        Instantiate(prefabLadron, posicionSpawn, Quaternion.identity);
    }
}

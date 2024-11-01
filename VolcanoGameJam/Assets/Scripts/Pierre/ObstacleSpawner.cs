// ObstacleSpawner.cs
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject doorPrefab;        // Préfabriqué de la porte
    public GameObject enemyPrefab;       // Préfabriqué de l'ennemi volant
    public float spawnInterval = 3f;     // Intervalle de spawn initial
    private float nextSpawnTime = 0f;
    public float minSpawnInterval = 1f;  // Intervalle minimum entre les spawns
    public float spawnIntervalDecrease = 0.05f; // Taux de diminution de l'intervalle de spawn
    public float speedIncreaseRate = 0.1f;      // Taux d'augmentation de la vitesse des obstacles
    public float spawnOffsetX = 10f;
    // Références aux joueurs
    public Transform playerTop;
    public Transform playerBottom;

    // Variable pour équilibrer les spawns
    private bool spawnTopNext = true; // Alternance entre haut et bas

    void Update()
    {
        // Vérifier si c'est le moment de spawn un nouvel obstacle
        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            nextSpawnTime = Time.time + spawnInterval;

            // Diminuer l'intervalle de spawn pour augmenter la difficulté
            spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - spawnIntervalDecrease * Time.deltaTime);
        }
    }

    void SpawnObstacle()
    {
        // Distance from the player along the X-axis

        if (spawnTopNext)
        {
            // Spawn an enemy at a position offset from PlayerTop
            Vector3 spawnPosition = new Vector3(playerTop.position.x + spawnOffsetX, playerTop.position.y, 0f);
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.GetComponent<ObstacleMover>().speed = 5f;

            // Set the sequence type
            SequenceHandler sequenceHandler = enemy.GetComponent<SequenceHandler>();
            if (sequenceHandler != null)
            {
                sequenceHandler.sequenceType = SequenceHandler.SequenceType.Arrows;
            }

            spawnTopNext = false;
        }
        else
        {
            // Spawn a door at a position offset from PlayerBottom
            Vector3 spawnPosition = new Vector3(playerBottom.position.x + spawnOffsetX, playerBottom.position.y, 0f);
            GameObject door = Instantiate(doorPrefab, spawnPosition, Quaternion.identity);
            door.GetComponent<ObstacleMover>().speed = 5f;

            // Set the sequence type
            SequenceHandler sequenceHandler = door.GetComponent<SequenceHandler>();
            if (sequenceHandler != null)
            {
                sequenceHandler.sequenceType = SequenceHandler.SequenceType.ZQSD;
            }

            spawnTopNext = true;
        }
    }

}

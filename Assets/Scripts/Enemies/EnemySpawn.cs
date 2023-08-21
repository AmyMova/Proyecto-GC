using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    // Creamos una clase la cual se encargara de manejar las
    // oleadas que se presentaran a lo largo de la partida
    [System.Serializable]
    public class Wave {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public int waveQuota;
        public float spawnInterval;
        public int spawnCount;
    }

    // Con esta clase tenemos un control del grupo de enemigos
    // que aparecera en cada oleada.
    [System.Serializable]
    public class EnemyGroup {
        public string enemyName;
        public int enemyCount;
        public int spawnCount;
        public GameObject enemyPrefab;
    }

    public List<Wave> waves;
    public int currentWaveCount;
    Transform player;

    // Creamos una serie de variables con las que tendremos un control
    // a tiempo real del estado de las oleadas y de cuantos enemigos
    // deberia de haber en pantalla.
    [Header("Spawner Attributes")]
    float spawnTimer;
    public int enemiesAlive;
    public int maxEnemiesAllowed;
    public bool maxEnemiesReached = false;
    public float waveInternal;

    // Creamos una lista la cual se encargara de tener un control de
    // la posicion de los puntos posibles alrededor del jugador en
    // que un enemigo puede aparecer
    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints;

    // Start is called before the first frame update
    void Start() {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
        firstWave();
    }

    

    void firstWave() {
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0) {
            StartCoroutine(BeginNextWave());
        }
        spawnTimer = 0f;
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update() {
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0) {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= waves[currentWaveCount].spawnInterval) {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    // Este es el metodo que se encarga de iniciar la siguiente
    // oleada de enemigos.
    IEnumerator BeginNextWave() {
        yield return new WaitForSeconds(waveInternal);

        if (currentWaveCount < waves.Count - 1) {
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    // Este metodo se encarga de tener un control de la cantidad de
    // enemigos en total que deben aparecer en la oleada. Esto tomando
    // en consideracion la cantidad que debe aparecer para cada grupo de enemigos.
    void CalculateWaveQuota() {
        int currentWaveQuota = 0;

        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups) {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
    }

    // Este es el metodo que se encarga de aparecer a los enemigos
    void SpawnEnemies() {
        // Verifica que el limite de enemigos no haya sido alcanzado y aun este dentro de
        // la cuota de enemigos que deben aparecer en la oleada.
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached) {
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups) {
                if (enemyGroup.spawnCount < enemyGroup.enemyCount) {
                    if (enemiesAlive >= maxEnemiesAllowed) {
                        maxEnemiesReached = true;
                        return;
                    }

                    Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                }
            }
        }

        if (enemiesAlive < maxEnemiesAllowed) {
            maxEnemiesReached = false;
        }
    }

    // Un metodo que se dispara cada que un enemigo es eliminado,
    // siendo que de esta manera aunque se haya alcanzado el limite
    // de enemigos, pueda aparecer uno tras haber eliminado a uno del conjunto.
    public void OnEnemyKilled() {
        enemiesAlive--;
    }
}

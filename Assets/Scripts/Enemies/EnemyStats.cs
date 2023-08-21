using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    public float despawnDistance = 20f;
    Transform player;

    SessionManager _sessionManager;

    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        _sessionManager = SessionManager.Instance;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) >= despawnDistance)
        {
            ReturnEnemy();
        }
    }

    private void ReturnEnemy()
    {
        EnemySpawn es = FindObjectOfType<EnemySpawn>();
        transform.position = player.position + es.relativeSpawnPoints[Random.Range(0, es.relativeSpawnPoints.Count)].position;
    }

    public void TakeDamage(float dmg)
    {

        AudioManager.Instance.PlaySFX("Hit");

        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    private void Kill()
    {
        AudioManager.Instance.PlaySFX("Kill");
        Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            return;
        }

        EnemySpawn es = FindAnyObjectByType<EnemySpawn>();
        _sessionManager.contador.Incrementar();
        es.OnEnemyKilled();
    }
}

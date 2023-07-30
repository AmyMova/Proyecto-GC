using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Creamos unas variables las cuales almacenaran los parametros
    // del enemigo y la posicion del jugador
    EnemyStats enemy;
    Transform player;

    // Start is called before the first frame update
    void Start() {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // De esta manera es que logramos que los enemigos persigan al jugador
    // en todo momento.
    void Update() {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime);
    }
}

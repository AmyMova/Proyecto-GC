using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour {
    PlayerStats player;
    CircleCollider2D playerCollector;
    public float pullSpeed;

    void Start() {
        player = FindObjectOfType<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
    }

    // De esta manera es que establecemos el rango desde
    // donde los objetos se empezaran acercar al jugador
    void Update() {
        playerCollector.radius = player.currentMagnet;
    }

    // Este metodo se encarga de revisar que el objeto que entre
    // dentro del rango del "iman" sea uno del tipo coleccionable.
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.TryGetComponent(out ICollectible collectible)) {
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();

            // De esta manera es que hacemos que el objeto vaya a la posicion del
            // jugador. Siendo la pullspeed la velocidad en que se mueve el objeto
            Vector2 force = (transform.position - col.transform.position).normalized;
            rb.AddForce(force * pullSpeed);
            collectible.Collect();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Creamos unas variables las cuales almacenaran la direccion
    // en que se movio el jugador.
    [HideInInspector]
    public float lastHorizontal;
    [HideInInspector]
    public float lastVertical;
    [HideInInspector]
    public Vector2 moveDir;
    [HideInInspector]
    public Vector2 lastMovedVector;
    PlayerStats player;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        lastMovedVector = new Vector2(1, 0f);
    }

    void Update() {
        InputManagement();
    }

    void FixedUpdate() {
        Move();
    }
    
    // Este metodo es el encargado de tener un control de los
    // controles del juego.
    void InputManagement() {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        if (moveDir.x != 0) {
            lastHorizontal = moveDir.x;
            lastMovedVector = new Vector2(lastHorizontal, 0f);
        }

        if (moveDir.y != 0) {
            lastVertical = moveDir.y;
            lastMovedVector = new Vector2(0f, lastVertical);
        }

        if (moveDir.x != 0 && moveDir.y != 0) {
            lastMovedVector = new Vector2(lastHorizontal, lastVertical);
        }
    }

    void Move() {
        rb.velocity = new Vector2(moveDir.x * player.currentMoveSpeed, moveDir.y * player.currentMoveSpeed);
    }
}

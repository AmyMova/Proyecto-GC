using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Creamos una variable la cual almacenara la posicion
    // del jugador
    public Transform jugador;
    public Vector3 offset;

    // De esta manera es que se maneja que la camara persiga al
    // jugador en todo momento.
    void Update() {
        transform.position = jugador.position + offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI puntaje;

    SessionManager _sessionManager;

    void Awake()
    {
        _sessionManager = SessionManager.Instance;
    }

    void Start()
    {
        int puntosActual = _sessionManager.contador.Puntaje;

        puntaje.text = puntosActual.ToString();
    }

}

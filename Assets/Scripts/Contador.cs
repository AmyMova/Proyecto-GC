using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Contador
{
    public int Puntaje { get; private set; }

    public void Incrementar() {
        Puntaje += 5;
    }

    public void Resetear() {
        Puntaje = 0;
    }
}

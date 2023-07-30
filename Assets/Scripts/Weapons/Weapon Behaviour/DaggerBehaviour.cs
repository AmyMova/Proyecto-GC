using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerBehaviour : ProjectileWeaponBehaviour {
    protected override void Start() {
        base.Start();
    }

    // Este metodo se encarga de movilizar el projectil en una direccion 
    // hasta que choque con algo o haya expirado
    void Update() {
        transform.position += Time.deltaTime * currentSpeed * direccion;
    }
}

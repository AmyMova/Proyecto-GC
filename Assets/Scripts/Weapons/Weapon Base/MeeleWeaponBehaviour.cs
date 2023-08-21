using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleWeaponBehaviour : MonoBehaviour
{
    public float duracion;
    public WeaponScriptableObject weaponData;

    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    // Inicializamos las variables con los datos del
    // objeto scripteado.
    void Awake() {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentPierce = weaponData.Pierce;
        currentCooldownDuration = weaponData.CooldownDuration;
    }

    public float GetCurrentDamage() {
        return currentDamage *= FindObjectOfType<PlayerStats>().currentMight;
    }

    // Start is called before the first frame update
    protected virtual void Start() {
        Destroy(gameObject, duracion);
    }

    // Este metodo se ddispara en el momento en que el projectil
    // choca contra un enemigo para asi bajarle la vida.
    protected virtual void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Enemy")) {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage());
        } 
    }
}

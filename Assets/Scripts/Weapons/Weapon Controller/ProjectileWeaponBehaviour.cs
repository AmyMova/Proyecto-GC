using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject datosArma;
    protected Vector3 direccion;
    public float duracion;

    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    // Inicializamos las variables con los datos del
    // objeto scripteado.
    void Awake() {
        currentDamage = datosArma.Damage;
        currentSpeed = datosArma.Speed;
        currentCooldownDuration = datosArma.CooldownDuration;
        currentPierce = datosArma.Pierce;
    }

    // Start is called before the first frame update
    protected virtual void Start() {
        Destroy(gameObject, duracion);
    }

    // Este metodo es el encargado de revisar la direccion
    // a la que se ha movido el jugador para asi poder
    // cambiar el sentido en que el projectil sale disparado.
    public void DirectionChecker(Vector3 dir) {
        direccion = dir;

        float dirx = direccion.x;
        float diry = direccion.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if (dirx < 0 && diry == 0) {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        } else if (dirx == 0 && diry < 0) {
            scale.y = scale.y * -1;
        } else if (dirx == 0 && diry > 0) {
            scale.x = scale.x * -1;
        } else if (dir.x > 0 && dir.y > 0) {
            rotation.z = 0f;
        } else if (dir.x > 0 && dir.y < 0) {
            rotation.z = -90f;
        } else if (dir.x < 0 && dir.y > 0) {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = -90f;
        } else if (dir.x < 0 && dir.y < 0) {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = 0f;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
    }

    public float GetCurrentDamage() {
        return currentDamage *= FindObjectOfType<PlayerStats>().currentMight;
    }

    // Este metodo se ddispara en el momento en que el projectil
    // choca contra un enemigo para asi bajarle la vida.
    protected virtual void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Enemy")) {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage());
            ReducePierce();
        }
    }

    // Este metodo se encarga de destruir el projectil cuando este
    // choca contra un enemigo. El pierce es en caso de que queramos
    // que el projectil atraviese mas de un enemigo a traves de un
    // objeto.
    void ReducePierce() {
        currentPierce--;
        if (currentPierce <= 0) {
            Destroy(gameObject);
        }
    }
}

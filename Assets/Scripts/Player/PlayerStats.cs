using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour {

    // Creamos una variable la cual contiene los parametros
    // del personaje.
    [SerializeField]
    CharacterScriptableObject characterData;

    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentRecovery;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentMight;
    [HideInInspector]
    public float currentProjectileSpeed;
    [HideInInspector]
    public float currentMagnet;
    [HideInInspector]
    public int puntos;



    void Awake() {

        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnet = characterData.Magnet;
        
        SpawnWeapon(characterData.StartingWeapon);
    }


    // Creamos las variables que utilizaremos para manejar
    // la experiencia y los niveles del personaje
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    // Creamos una lista la cual contendra las armas que consigue
    // el jugador y que se iran utilizando cada cierto tiempo.
    public List<GameObject> spawnedWeapons;

    // Aqui se encuentran las variables que maneja el tiempo de
    // invencibilidad cada vez que el personaje es golpeado.
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    // Creamos una clase con la que manejaremos los requerimientos
    // de experiencia para cada rango de niveles.
    [System.Serializable]
    public class LevelRange {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    public List<LevelRange> levelRanges;

    
    // Este metodo es el encargado de añadir a la lista las nuevas armas que
    // el jugador vaya consiguiendo en su partida.
    public void SpawnWeapon(GameObject weapon) {
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        spawnedWeapons.Add(spawnedWeapon);
    }

    void Start() {
        experienceCap = levelRanges[0].experienceCapIncrease;
    }

    // Aqui manejamos el tiempo de invencibilidad, siendo que
    // mediante el boolean, se indica si es invencible o no.
    void Update() {
        if (invincibilityTimer > 0) {
            invincibilityTimer -= Time.deltaTime;
        } else {
            isInvincible = false;
        }

        Recover();
    }

    public void IncreaseExperience(int amount) {
        experience += amount;
        LevelUpChecker();
    }

    // Este metodo es el utilizado para asi tener un control
    // de como aumenta la experiencia con cada nivel.
    void LevelUpChecker() {
        if (experience >= experienceCap) {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges) {
                if (level >= range.startLevel && level <= range.endLevel) {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }

            experienceCap += experienceCapIncrease;
        }
    }

    // Este metodo es el que se encarga de que el jugador reciba
    // daño cuando no es invencible.
    public void TakeDamage(float dmg) {
        if (!isInvincible) {
            currentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (currentHealth <= 0) {
                Kill();
            }
        }
    }

    // Este es el metodo que se dispara cuando el jugador muere
    // y es llevado a la pantalla de Game Over
    void Kill() {
        SceneManager.LoadScene(1);
    }

    // Este metodo es el encargado de curar al jugador cuando
    // este toma un objeto de curacion
    internal void RestoreHealth(float amount) {
        currentHealth = (currentHealth + amount) >= characterData.MaxHealth ? characterData.MaxHealth : currentHealth + amount;
    }

    // Este es el metodo que se encarga de manejar la recuperacion
    // pasiva que tiene el jugador.
    void Recover() {
        if (currentHealth < characterData.MaxHealth) {
            currentHealth += currentRecovery * Time.deltaTime;

            if (currentHealth > characterData.MaxHealth) {
                currentHealth = characterData.MaxHealth;
            }
        }
    }
}

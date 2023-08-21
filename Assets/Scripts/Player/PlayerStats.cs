using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour {
    
    [HideInInspector]
    public UnityEvent damage;
    [SerializeField]
    public Animator animator;
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

        inventory = GetComponent<InventoryManager>();
        
        SpawnWeapon(characterData.StartingWeapon);
    }


    // Creamos las variables que utilizaremos para manejar
    // la experiencia y los niveles del personaje
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;

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
        if(weaponIndex >= inventory.weaponSlots.Count - 1) {
            return;
        }

        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());

        weaponIndex++;
    }

    public void SpawnPassiveItem(GameObject weapon) {
        if (passiveItemIndex >= inventory.passiveItemSlots.Count - 1) {
            return;
        }

        GameObject spawnedPassiveItem = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>());

        passiveItemIndex++;
    }

    void Start() {
        experienceCap = levelRanges[0].experienceCapIncrease;
    }

    // Aqui manejamos el tiempo de invencibilidad, siendo que
    // mediante el boolean, se indica si es invencible o no.
    void Update() {
        if (invincibilityTimer > 0) {
            invincibilityTimer -= Time.deltaTime;
            
        }
        else
        {
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
            GameManager.instance.StartLevelUp();
        }
    }

    // Este metodo es el que se encarga de que el jugador reciba
    // daño cuando no es invencible.
    public void TakeDamage(float dmg) {
        if (!isInvincible) {
            currentHealth -= dmg;
            animator.SetTrigger("getHurt");

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

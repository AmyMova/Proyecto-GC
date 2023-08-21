using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour
{
    readonly object _lock = new object();

    static WeaponController _instance;

    public static WeaponController Instance
    {
        get
        {
            return _instance;
        }
    }
    [HideInInspector]
    public UnityEvent attack;
    [SerializeField]
    public Animator animator;


    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData;
    float currentCooldown;
    protected PlayerMovement pm;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        currentCooldown = weaponData.CooldownDuration;
    }
    void Awake()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = this;
                }
            }
        }
        attack.AddListener(Attack);
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        
        if (currentCooldown <= 0f)
        {
            int index = Random.Range(0, 2);
            Attack();
            
            switch (index)
            {
                case 0:
                    animator.SetTrigger("attack1");
                    break;

                case 1:animator.SetTrigger("attack2");
                    break;
            }
            

        }
    }

    protected virtual void Attack()
    {
        currentCooldown = weaponData.CooldownDuration;

    }
}

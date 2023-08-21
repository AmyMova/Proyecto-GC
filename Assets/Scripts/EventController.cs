using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventController : MonoBehaviour
{
   
    public void onAttack()
    {
        WeaponController.Instance.attack.Invoke();
    }
    
}

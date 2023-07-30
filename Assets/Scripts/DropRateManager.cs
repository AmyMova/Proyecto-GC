using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    // Creamos una clase con la que podamos almacenar los datos del objeto
    // como su nombre, sprite y posibilidad de aparicion.
    [System.Serializable]
    public class Drops {
        public string name;
        public GameObject item;
        public float dropRate;
    }

    // Creamos una lista para asi almacenar una serie de posibilidades
    // en caso de que queramos que el enemigo pueda dejar caer diferentes
    // objetos
    public List<Drops> drops;

    // Este metodo es el que se verificara los posibles drops
    // cuando el enemigo es eliminado
    void OnDestroy() {

        // Esto es para evitar que se genere un error a la hora de dejar la
        // escena o quitar el juego.
        if (!gameObject.scene.isLoaded) {
            return;
        }

        float random = Random.Range(0f, 100f);
        List<Drops> dropsConseguidos = new(); // Creamos esta lista para almacenar los drops conseguidos

        // Repasamos la lista de posibles objetos para verificar si nuestro
        // numero aleatorio es menor o igual a la posibilidad de aparicion
        foreach (Drops rate in drops) {
            if (random <= rate.dropRate) {
                dropsConseguidos.Add(rate);
            }
        }

        if (dropsConseguidos.Count > 0) {
            // De la lista de drops conseguidos, unicamente se elige uno al azar
            // ya que no deseamos que haga spawn mas de un objeto.
            Drops drops = dropsConseguidos[Random.Range(0, dropsConseguidos.Count)];
            Instantiate(drops.item, transform.position, Quaternion.identity);
        }
    }
}

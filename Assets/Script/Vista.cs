using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vista : MonoBehaviour
{
    public List<Player> list_objetivos;
    // Start is called before the first frame update
    void Start()
    {
        list_objetivos = new List<Player>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var obj = other.gameObject.GetComponent<Player>();
            bool validar = false;

            foreach (var entidad in list_objetivos)
            {
                if (obj == entidad)
                {
                    validar = true;
                }
            }

            if (!validar)
            {
                list_objetivos.Add(obj);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var obj = other.gameObject.GetComponent<Player>();
            list_objetivos.Remove(obj);
        }
    }
}

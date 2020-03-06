using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golpe : MonoBehaviour
{
    public string objetivo;
    public int daño;

    public List<Vida> list_objetivos;

    // Start is called before the first frame update
    void Start()
    {
        list_objetivos = new List<Vida>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(objetivo))
        {
            var obj = other.gameObject.GetComponent<Vida>();
            bool validar = false;

            foreach(var entidad in list_objetivos)
            {
                if(obj == entidad)
                {
                    validar = true;
                }
            }

            if(!validar)
            {
                list_objetivos.Add(obj);
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(objetivo))
        {
            var obj = other.gameObject.GetComponent<Vida>();
            list_objetivos.Remove(obj);
        }
    }

    public void dañar()
    {
        lock (list_objetivos)
        {
            foreach (var entidad in list_objetivos)
            {
                entidad.hp -= daño;
            }
        }
    }
}

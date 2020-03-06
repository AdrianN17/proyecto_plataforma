using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;
    public GameObject modelo;
    public Collider coll;

    public Vista vista;

    public float velocidad;

    public Golpe golpe;

    public Vector3 punto_inicio;

    public Vida vida;

    // Start is called before the first frame update
    void Start()
    {
        rb.freezeRotation = true;

        punto_inicio = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dt = Time.deltaTime;

        var target = buscarTarget();

        if (target != null)
        {
            anim.SetBool("Correr", true);
            direccionarAngulo(target.coll.bounds.center, dt);
        }
        else
        {
            anim.SetBool("Correr", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Zombie Running"))
        {
            rb.AddForce(transform.forward * velocidad * rb.mass * dt);
        }

        if(transform.position.y < -20)
        {
            vida.hp = 0;
        }

        if (vida.hp < 0.1 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Zombie Dying"))
        {
            anim.SetTrigger("Morir");
        }
    }

    private float calcularDistancia(Vector3 mi_centro, Vector3 centro_objetivo)
    {
        var v1 = new Vector3(mi_centro.x,0, mi_centro.z);
        var v2 = new Vector3(centro_objetivo.x, 0, centro_objetivo.z);

        return Vector3.Distance(v1, v2);
    }

    public void direccionarAngulo(Vector3 vector, float dt)
    {
        Vector3 direction = (vector - coll.bounds.center).normalized;
        Quaternion rotate = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, dt * 5f);
        rb.MoveRotation(transform.rotation);
    }

    public Player buscarTarget()
    {
        Player target = null;

        if (vista.list_objetivos.Count > 0)
        {
            float min_distance = 99999;
            

            foreach (var player in vista.list_objetivos)
            {
                var distancia = calcularDistancia(coll.bounds.center, player.coll.bounds.center);

                if (distancia < min_distance)
                {
                    min_distance = distancia;
                    target = player;
                }
            }
        }

        return target;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Zombie Dying") || !anim.GetCurrentAnimatorStateInfo(0).IsName("Zombie Attack"))
            {
                anim.SetTrigger("Atacar");
            }
        }
    }

    public void atacar()
    {
        golpe.dañar();
    }

    public void morir()
    {
        vida.hp = vida.max_hp;

        transform.position = punto_inicio;

        anim.ResetTrigger("Atacar");
    }
}

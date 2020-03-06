using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;
    public GameObject modelo;

    public float velocidad;
    public float salto;

    private bool mover_vertical;
    private bool mover_horizontal;

    private Vector3 vec_v;
    private Vector3 vec_h;

    private bool suelo;

    public Vida vida;

    public Golpe golpe;

    public Collider coll;

    public Vector3 punto_inicio;

    public Text txtMonedas;
    private ushort monedas;

    private bool finalizar;

    // Start is called before the first frame update
    void Start()
    {
        rb.freezeRotation = true;

        mover_vertical = false;
        mover_horizontal = false;

        suelo = false;

        punto_inicio = transform.position;

        finalizar = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!finalizar)
        { 
            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                vec_h = new Vector3(-1, 0, 0);

                mover_horizontal = true;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                vec_h = new Vector3(1, 0, 0);

                mover_horizontal = true;
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                vec_v = new Vector3(0, 0, 1);

                mover_vertical = true;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                vec_v = new Vector3(0, 0, -1);

                mover_vertical = true;
            }


            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                vec_h = new Vector3();
                mover_horizontal = false;
            }

            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                vec_h = new Vector3();
                mover_horizontal = false;
            }

            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                vec_v = new Vector3();
                mover_vertical = false;
            }

            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                vec_v = new Vector3();
                mover_vertical = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) && suelo)
            {
                anim.ResetTrigger("Atacar");
                saltar();
            }

            anim.SetFloat("Salto", rb.velocity.y);

            if (Input.GetKeyDown(KeyCode.X) && suelo && (anim.GetCurrentAnimatorStateInfo(0).IsName("Thriller Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Run")))
            {
                anim.SetTrigger("Atacar");
            }

        }

        anim.SetBool("Suelo", suelo);

        if (mover_vertical || mover_horizontal)
            anim.SetBool("Correr", true);
        else
            anim.SetBool("Correr", false);


        if (rb.velocity.y < -4.5 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Fall A Loop") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Dying"))
        {
            anim.SetTrigger("Caida");
        }

        if (transform.position.y < -20)
        {
            vida.hp = 0;
        }

        if (vida.hp < 0.1 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Dying"))
        {
            anim.SetTrigger("Morir");
        }

        
   
    }

    void FixedUpdate()
    {
        float dt = Time.deltaTime; 

        if ((mover_vertical || mover_horizontal) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Boxing") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Dying"))
        { 
            var vec_n = new Vector3(vec_h.x, 0, vec_v.z);

            var radio = (Mathf.Atan2(rb.velocity.z , rb.velocity.x * -1) * Mathf.Rad2Deg) -90 ;

            modelo.transform.eulerAngles = new Vector3(0,radio, 0);

            rb.AddForce(vec_n * dt * velocidad * rb.mass);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Bloque"))
        {
            if (collision.contacts[0].normal.y> 0.1)
                suelo = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Bloque"))
        {
            suelo = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Obstaculo"))
        {
            vida.hp = 0;
        }
        else if(other.transform.CompareTag("Moneda"))
        {
            monedas++;

            txtMonedas.text = "Monedas : " + monedas;

            Destroy(other.gameObject);
        }
        else if (other.transform.CompareTag("Estrella"))
        {
            monedas++;

            finalizar = true;

            rb.velocity = new Vector3(0, 0, 0);
            mover_vertical = false;
            mover_horizontal = false;

            anim.SetBool("Bailar", true);


            Destroy(other.gameObject);
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
        anim.ResetTrigger("Saltar");
    }

    public void errorSalto()
    {
        anim.ResetTrigger("Saltar");
    }

    public void saltar()
    {
        rb.AddForce(salto * Vector3.up * rb.mass);
    }
}

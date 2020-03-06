using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girar : MonoBehaviour
{
    public float velocidad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dt = Time.deltaTime;
        var radio = velocidad * dt;
        transform.Rotate(0, 0, radio);
    }
}

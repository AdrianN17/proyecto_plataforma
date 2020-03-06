using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vida : MonoBehaviour
{
    public int hp;
    public int max_hp;

    private void Start()
    {
        max_hp = hp;
    }
}

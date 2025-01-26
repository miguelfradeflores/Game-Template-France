using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_activation : MonoBehaviour
{

    public Transform player;
    Animator anim;
    public float distancia = 4f;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Mathf.Sqrt(Mathf.Pow((player.position.x - transform.localPosition.x), 2) + Mathf.Pow(player.position.y - transform.localPosition.y, 2)) < distancia)
        {
            anim.SetBool("lights_on", true);

        }
    }
}
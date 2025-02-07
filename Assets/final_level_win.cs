using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class final_level_win : MonoBehaviour
{
    Animator [] anims ;
    public Transform player;
    public float distancia = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Mathf.Sqrt(Mathf.Pow((player.position.x - transform.localPosition.x), 2) + Mathf.Pow(player.position.y - transform.localPosition.y, 2)) < distancia)
        {
            foreach (Animator anim in anims)
            {
                anim.SetBool("lights_on", true);
            }
        }
    }
}

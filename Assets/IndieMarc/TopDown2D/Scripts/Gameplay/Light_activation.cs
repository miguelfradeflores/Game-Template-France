using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.TopDown
{
    public class Light_activation : MonoBehaviour
    {

        private Player m_Player;
        private Transform player_p;
        Animator anim;
        public float distancia = 4f;
        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            m_Player = FindFirstObjectByType<Player>();
            player_p = m_Player.GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {

            if (Mathf.Sqrt(Mathf.Pow((player_p.position.x - transform.localPosition.x), 2) + Mathf.Pow(player_p.position.y - transform.localPosition.y, 2)) < distancia)
            {
                anim.SetBool("lights_on", true);

            }
        }
    }
};
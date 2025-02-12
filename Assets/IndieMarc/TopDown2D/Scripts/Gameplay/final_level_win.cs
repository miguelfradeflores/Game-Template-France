using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IndieMarc.TopDown
{
    public class final_level_win : MonoBehaviour
    {
        Transform [] bulbs;
        private Player m_Player;
        private Transform player_p;
        public float distancia = 2.0f;
        // Start is called before the first frame update
        void Start()
        {
            m_Player = FindFirstObjectByType<Player>();
            player_p = m_Player.GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_Player) {
                if (Mathf.Sqrt(Mathf.Pow((player_p.position.x - transform.localPosition.x), 2) + Mathf.Pow(player_p.position.y - transform.localPosition.y, 2)) < distancia)
                {
                    for (int i=0; i < transform.childCount;i++ ){
                        Transform child = transform.GetChild(i); 
                        Animator anim = child.GetComponent<Animator>();
                        anim.SetBool("lights_on", true);
                    }
                }
            } }
    }
}
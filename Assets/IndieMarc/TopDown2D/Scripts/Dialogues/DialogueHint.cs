using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.TopDown {

    public class DialogueHint : MonoBehaviour
    {
        private Player m_Player;
        private Transform player_p;
        public Dialogues dialogue;
        private SpriteRenderer sprite;
        private Animator anim;
        float distancia = 2f;
    
    // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            sprite = GetComponent<SpriteRenderer>();
            m_Player = FindFirstObjectByType<Player>();
            player_p = m_Player.GetComponent<Transform>();
            Debug.Log("Player founded");

        }


        void Update()
        {
            if (m_Player)
            {
                if (Mathf.Sqrt(Mathf.Pow((player_p.position.x - transform.localPosition.x), 2) + Mathf.Pow(player_p.position.y - transform.localPosition.y, 2)) < distancia)
                {
                    Debug.Log("Player Near");
                    sprite.enabled = true;
                    anim.SetBool("playerNear", true);
                }
                else
                {
                    sprite.enabled = false;
                    anim.SetBool("playerNear", false);

                }
            }
        }
    }
}
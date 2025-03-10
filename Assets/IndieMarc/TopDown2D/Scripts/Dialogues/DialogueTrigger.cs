using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace IndieMarc.TopDown
{
    [System.Serializable]
    public class DialogueTrigger : MonoBehaviour
    {

        public KeyCode talk_button = KeyCode.Return;
        private Player m_Player;
        private Transform player_p;
        public Dialogues dialogue;
        public bool interactive = false;
        [SerializeField] public float distancia = 2.0f;
        private bool triggered;

        public void TriggerDailogue()
        {
            FindObjectOfType<DialogueManager2>().StartDailogue(dialogue);
        }

        void Start()
        {
            m_Player = FindFirstObjectByType<Player>();
            player_p = m_Player.GetComponent<Transform>();
            Debug.Log("Player founded");
            triggered = false;
        }


        void Update()
        {
            if (m_Player)
            {
                if (Mathf.Sqrt(Mathf.Pow((player_p.position.x - transform.localPosition.x), 2) + Mathf.Pow(player_p.position.y - transform.localPosition.y, 2)) < distancia)
                {
                    Debug.Log("Colliding with Player");
                    if (triggered == false) {
                        if (Input.GetKeyDown(talk_button) || interactive)
                        {
                            TriggerDailogue();
                            interactive = false;
                            triggered = true;
                        }
                    }
                }
                else
                {
                    triggered = false;
                }
            }
        }
    }
}
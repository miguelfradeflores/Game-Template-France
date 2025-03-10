using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace IndieMarc.TopDown
{

    public class DialogueManager2 : MonoBehaviour
    {
        private Queue<string> tittles;
        private Queue<string> sentences;

        //  public Animator anim;
        public Text tittleText;
        public Text dialogueText;
        public CanvasGroup dialogueGroup;
        private bool fade_in = false;
        private bool fade_out = false;
        public bool final_level = false;
        private bool started = false;

        // Start is called before the first frame update
        void Start()
        {
            tittles = new Queue<string>();
            sentences = new Queue<string>();
            dialogueGroup.alpha = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (fade_in)
            {
                if (dialogueGroup.alpha < 1)
                {
                    dialogueGroup.alpha += Time.deltaTime;
                    if (dialogueGroup.alpha >= 1)
                    {
                        fade_in = false;
                    }
                }
            }
            if (fade_out)
            {
                if (dialogueGroup.alpha >= 0)
                {
                    dialogueGroup.alpha -= Time.deltaTime;
                    if (dialogueGroup.alpha == 0)
                    {
                        fade_out = false;
                    }
                }
            }


            if (fade_in & started){
                if (Input.GetKeyDown(KeyCode.Escape)) DisplayNextSentence();
            }


        }

        public void ShowCanvas()
        {
            fade_in = true;
        }


        public void HideCanvas()
        {
            fade_out = true;
            started = false;
        }


        public void StartDailogue(Dialogues dialogue)
        {
            Debug.Log("Start Dialogue conversation with " + dialogue.tittle);

            //anim.SetBool("isOpen", true);
            started = false;
            tittles.Clear();
            sentences.Clear();

            foreach (string tittle in dialogue.tittle)
            {
                tittles.Enqueue(tittle);
            }


            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }
            ShowCanvas();
            started = true;
            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {


            if (sentences.Count == 0)
            {
                if (final_level)
                {
                    EndGame();
                    return;
                }
                else
                {
                    EndDialogue();
                    return;
                }
            }
            else
            {
                string tittle = tittles.Dequeue();
                string sentence = sentences.Dequeue();
                tittleText.text = tittle;
                dialogueText.text = sentence;
                Debug.Log("Actor: " + tittle);
                Debug.Log("Sentence: " + sentence);
                StopAllCoroutines();
                StartCoroutine(TypeSentence(sentence));


            }
        }

        IEnumerator TypeSentence(string sentence)
        {
            dialogueText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }

        }

        public void EndDialogue()
        {
            Debug.Log("FINISHING Converdation");
            HideCanvas();
        }

        public void EndGame()
        {
            Debug.Log("FINISHING game");
            HideCanvas();
            Destroy(FindFirstObjectByType<Player>().gameObject);
            SceneNav.GoToLevel("Creditos", 6);
        }

    }
}
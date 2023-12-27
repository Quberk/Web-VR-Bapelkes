using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BapelkesWebVrAnc.DialogueController{
    public class DialogueManager : MonoBehaviour
    {
        private Queue<string> sentences;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text dialogueText;

        [SerializeField] private Animator dialoguePanelAnimator;
        [SerializeField] private string startDialogueAnimatorTrigger;
        [SerializeField] private string endDialogueAnimatorTrigger;

        private bool dialogueFinishedStatus = false;


        void Start()
        {
            sentences = new Queue<string>();
        }

        public void StartDialogue(Dialogue dialogue){

            dialogueFinishedStatus = false;

            dialoguePanelAnimator.SetTrigger(startDialogueAnimatorTrigger);

            nameText.text = dialogue.name;

            sentences.Clear();

            foreach (string sentence in dialogue.sentences){
                sentences.Enqueue(sentence);
            }

            DisplayNextSentece();

        }

        public void DisplayNextSentece(){

            if (sentences.Count == 0){
                
                EndDialogue();
                return;
            }

            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentece(sentence));
        
        }

        IEnumerator TypeSentece (string sentence){

            dialogueText.text = "";

            foreach(char letter in sentence.ToCharArray()){
                
                dialogueText.text += letter;
                yield return null;
            }

        }

        private void EndDialogue(){

            dialoguePanelAnimator.SetTrigger(endDialogueAnimatorTrigger);
            dialogueFinishedStatus = true;

        }

        public bool GetDialogueFinishedStatus(){
            return dialogueFinishedStatus;
        }
    }
}


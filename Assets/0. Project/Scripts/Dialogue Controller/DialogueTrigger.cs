using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using BapelkesWebVrAnc.DeviceControllers;
using WebXR.Interactions;
using BapelkesWebVrAnc.Protocols;

namespace BapelkesWebVrAnc.DialogueController{

    [RequireComponent (typeof(Rigidbody))]
    public class DialogueTrigger : MonoBehaviour, IRetakingHandReference
    {
        [SerializeField] private Dialogue dialogue;
        private DialogueManager dialogueManager;
        private Rigidbody myRigidbody;

        private bool dialogueFinishedStatus = false;

        [Header("Hands Control")]
        [SerializeField] private Rigidbody nextButtonRigidbody;
        private ControllersInteraction[] controllersInteractions;
        private ControllerInteraction[] vrControllerInteractions;
        private bool startDialogueTrigger = false;

        [Header("Delay between Next")]
        [SerializeField] private float delayTime;
        private float delayCounter = 0;

        void Start(){
            myRigidbody = GetComponent<Rigidbody>();
            dialogueManager = FindObjectOfType<DialogueManager>();
        }

        void Update(){

            if (!startDialogueTrigger)
                return;

            dialogueFinishedStatus = dialogueManager.GetDialogueFinishedStatus();
            
            DelayInDetectingHand();
        }

        void DelayInDetectingHand(){

            delayCounter += Time.deltaTime;

            if (delayCounter >= delayTime){

                DetectingHand();
                return;
            }
        }

        void DetectingHand(){
            
            if (controllersInteractions != null){

                foreach(ControllersInteraction controller in controllersInteractions){
                
                    Rigidbody contactedRigidbody = controller.GetCurrentRigidbody();

                    if (contactedRigidbody == null){
                        continue;
                    }
                            
                    if (contactedRigidbody == nextButtonRigidbody){

                        dialogueManager.DisplayNextSentece();
                        delayCounter = 0;
                        return;
                    }  
                }
            }

            else if (vrControllerInteractions != null){

                foreach(ControllerInteraction controller in vrControllerInteractions){
                
                    Rigidbody contactedRigidbody = controller.GetCurrentRigidbody();

                    if (contactedRigidbody == null){
                        continue;
                    }
                                
                    if (contactedRigidbody == nextButtonRigidbody){

                        dialogueManager.DisplayNextSentece();
                        delayCounter = 0;
                        return;
                    }  
                }
            }

        }

        public void TriggerDialogue(){

            dialogueManager.StartDialogue(dialogue);

            if (GameObject.FindGameObjectsWithTag("Hand").Length == 0)
                return;

            GameObject[] hands = GameObject.FindGameObjectsWithTag("Hand");

            if (hands[0].GetComponent<ControllersInteraction>()){
                controllersInteractions = new ControllersInteraction[hands.Length];
                for (int i = 0; i < controllersInteractions.Length; i++){
                    controllersInteractions[i] = hands[i].GetComponent<ControllersInteraction>();
                }
            }
                
            else if(hands[0].GetComponent<ControllerInteraction>()){
                vrControllerInteractions = new ControllerInteraction[hands.Length];
                for (int i = 0; i < vrControllerInteractions.Length; i++){
                    vrControllerInteractions[i] = hands[i].GetComponent<ControllerInteraction>();
                }
            }
        }

        public void StartTheDialogueTrigger(){
            startDialogueTrigger = true;
        }

        public Rigidbody GetRigidbody(){
            return myRigidbody;
        }

        public bool GetDialogueFinishedStatus(){
            if (dialogueFinishedStatus)
                startDialogueTrigger = false;
            return dialogueFinishedStatus;
        }

        public void RetakingHandReference(){
            
            controllersInteractions = null;
            vrControllerInteractions = null;
            
            if (GameObject.FindGameObjectsWithTag("Hand").Length == 0)
                return;

            GameObject[] hands = GameObject.FindGameObjectsWithTag("Hand");

            if (hands[0].GetComponent<ControllersInteraction>()){
                controllersInteractions = new ControllersInteraction[hands.Length];
                for (int i = 0; i < controllersInteractions.Length; i++){
                    controllersInteractions[i] = hands[i].GetComponent<ControllersInteraction>();
                }
            }
                
            else if(hands[0].GetComponent<ControllerInteraction>()){
                vrControllerInteractions = new ControllerInteraction[hands.Length];
                for (int i = 0; i < vrControllerInteractions.Length; i++){
                    vrControllerInteractions[i] = hands[i].GetComponent<ControllerInteraction>();
                }
            }
        }
    
    }
}


using System.Collections;
using System.Collections.Generic;
using BapelkesWebVrAnc.DialogueController;
using UnityEngine;
using BapelkesWebVrAnc.DeviceControllers;
using WebXR.Interactions;


namespace BapelkesWebVrAnc.Protocols{

    /// <summary>
    /// Class ini berfungsi untuk memunculkan Chatting terhadap Target Objek ketika menekan sebuah Trigger
    /// </summary>
    /// 

    public class DialogueWithTriggerProtocol : ProtocolManager, IRetakingHandReference
    {
        
        [SerializeField] private DialogueTrigger dialogueTrigger;
        private Rigidbody dialogueTriggerRb;        
        private ControllersInteraction[] controllersInteractions;
        private ControllerInteraction[] vrControllerInteractions;
        private bool alreadyTriggered = false;

        void Update(){

            if (!protocolStarted || protocolFinished){
                return;
            }

            if (dialogueTrigger.GetDialogueFinishedStatus()){
                StopTheProtocol();
                return;
            }

            if (alreadyTriggered)
                return;

            if (controllersInteractions != null){

                foreach(ControllersInteraction controller in controllersInteractions){
                
                    Rigidbody contactedRigidbody = controller.GetCurrentRigidbody();

                    if (contactedRigidbody == null){
                        return;
                    }
                        
                    if (contactedRigidbody == dialogueTriggerRb){

                        dialogueTrigger.TriggerDialogue();
                        dialogueTrigger.StartTheDialogueTrigger();

                        alreadyTriggered = true;

                        return;
                    }  
                }
            }

            else if (vrControllerInteractions != null){

                foreach(ControllerInteraction controller in vrControllerInteractions){
                
                    Rigidbody contactedRigidbody = controller.GetCurrentRigidbody();

                    if (contactedRigidbody == null){
                        return;
                    }
                        
                    if (contactedRigidbody == dialogueTriggerRb){

                        dialogueTrigger.TriggerDialogue();
                        dialogueTrigger.StartTheDialogueTrigger();

                        alreadyTriggered = true;

                        return;
                    }  
                }
            }
        }

        void TakingReference(){
            
            dialogueTriggerRb = dialogueTrigger.GetRigidbody();

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


        //=====================================OVERRIDE METHODS============================================================
        
        public override void StartTheProtocol()
        {
            protocolStarted = true;

            TakingReference();
        }

        public override void StopTheProtocol()
        {
            protocolFinished = true;
            protocolStarted = false;
        }
    }

}


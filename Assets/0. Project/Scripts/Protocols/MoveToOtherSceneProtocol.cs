using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BapelkesWebVrAnc.DeviceControllers;
using WebXR.Interactions;
using UnityEngine.SceneManagement;


namespace BapelkesWebVrAnc.Protocols{

    public class MoveToOtherSceneProtocol : ProtocolManager
    {
        private ControllersInteraction[] controllersInteractions;
        private ControllerInteraction[] vrControllerInteractions;
        [SerializeField] private GameObject targetObject;
        [SerializeField] private Rigidbody targetRigidbody;
        [SerializeField] private string targetScene;

        private bool alreadyTakingReference = false;

        void Start(){
            targetRigidbody = targetObject.GetComponent<Rigidbody>();
        }


        void Update(){

            if (!protocolStarted || protocolFinished)
                return;

            if (!alreadyTakingReference)
                return;

            if (controllersInteractions != null){

                 foreach(ControllersInteraction controller in controllersInteractions){
                
                    Rigidbody contactedRigidbody = controller.GetCurrentRigidbody();

                    if (contactedRigidbody == null){
                        continue;
                    }
                        
                    if (contactedRigidbody == targetRigidbody){
                        MoveToScene(targetScene);
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
                        
                    if (contactedRigidbody == targetRigidbody){
                        MoveToScene(targetScene);
                        return;
                    }   
                }
            }
        }

        void MoveToScene(string targetScene){

            SceneManager.LoadScene(targetScene);
        }

        void TakingReference(){

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

            alreadyTakingReference = true;

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


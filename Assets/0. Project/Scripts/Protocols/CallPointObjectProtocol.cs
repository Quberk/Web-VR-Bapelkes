using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using BapelkesWebVrAnc.DeviceControllers.VRControllers;
using BapelkesWebVrAnc.DeviceControllers;
using WebXR.Interactions;

namespace BapelkesWebVrAnc.Protocols{

    /// <summary>
    /// Class ini berfungsi untuk memunculkan Identifier pada Objek yang ditunjuk
    /// Dengan mengecek Rigidbody yang disentuh oleh DesertControllerInteraction
    /// </summary>

    public class CallPointObjectProtocol : ProtocolManager, IRetakingHandReference
    {
        
        [SerializeField] private Rigidbody targetObjectRigidbody;

        [SerializeField] private GameObject targetUiIdentifier;
        private ControllersInteraction[] controllersInteractions;
        private ControllerInteraction[] vrControllerInteractions;

        void Start(){
            targetUiIdentifier.SetActive(false);
        }

        void Update(){

            if (!protocolStarted)
                return;

            if (controllersInteractions != null){
                foreach(ControllersInteraction controller in controllersInteractions){
                
                    Rigidbody contactedRigidbody = controller.GetCurrentRigidbody();

                    if (contactedRigidbody == targetObjectRigidbody){
                        targetUiIdentifier.SetActive(true);
                        StopTheProtocol();
                    } 
                }
            }

            else if (vrControllerInteractions != null){
                foreach(ControllerInteraction controller in vrControllerInteractions){
                
                    Rigidbody contactedRigidbody = controller.GetCurrentRigidbody();

                    if (contactedRigidbody == targetObjectRigidbody){
                        targetUiIdentifier.SetActive(true);
                        StopTheProtocol();
                    }
                }
            }

        }

        void ProtocolStarted(){
            
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
            ProtocolStarted();
        }

        public override void StopTheProtocol()
        {
            protocolFinished = true;
            protocolStarted = false;
        }
    }
}


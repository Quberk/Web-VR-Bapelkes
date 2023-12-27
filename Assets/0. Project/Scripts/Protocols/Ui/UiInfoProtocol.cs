using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using BapelkesWebVrAnc.DeviceControllers;
using BapelkesWebVrAnc.Protocols;
using JetBrains.Annotations;
using UnityEngine;
using WebXR.Interactions;

namespace BapelkesWebVrAnc.Protocols.UI{

    /// <summary>
    /// Class ini berfungsi untuk memunculkan UI Info
    /// Lalu hilang Jika di Trigger oleh User
    /// </summary>

    public class UiInfoProtocol : ProtocolManager, IRetakingHandReference
    {
        [SerializeField] private GameObject uiInfo;
        [SerializeField] private Rigidbody button;
        [SerializeField] private Animator uiInfoAnimator;
        [SerializeField] private string uiInfoStartTrigger;
        [SerializeField] private string uiInfoFinishedTrigger;
        private ControllersInteraction[] controllersInteractions;
        private ControllerInteraction[] vrControllerInteractions;

        void Start()
        {
            uiInfo.SetActive(false);
        }

        void Update()
        {
            if (!protocolStarted || protocolFinished)
                return;

            if (controllersInteractions != null){
                foreach(ControllersInteraction controller in controllersInteractions){
                
                    Rigidbody contactedRigidbody = controller.GetCurrentRigidbody();

                    if (contactedRigidbody == null){
                        continue;
                    }
                        
                    if (contactedRigidbody == button){

                        StopTheProtocol();

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
                        
                    if (contactedRigidbody == button){

                        StopTheProtocol();

                        return;
                    }   
                }
            }

    
        }

        void ProtocolStarted(){

            uiInfo.SetActive(true);

            TakingReference();
        }

        void ProtocolFinished(){

            uiInfoAnimator.SetTrigger(uiInfoFinishedTrigger);
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

            ProtocolFinished();
        }
    }

}


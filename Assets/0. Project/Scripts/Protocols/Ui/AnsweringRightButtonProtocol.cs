using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using BapelkesWebVrAnc.DeviceControllers;
using WebXR.Interactions;


namespace BapelkesWebVrAnc.Protocols.UI{

    /// <summary>
    /// Class ini berfungsi untuk menjawab Button yang benar
    /// Jika salah maka Button yang dipilih berubah menjadi mereah
    /// Jika benar maka Button yang dipilih berubah menjadi Hijau dan Protocol Finished
    /// </summary>
    /// 
    public class AnsweringRightButtonProtocol : ProtocolManager, IRetakingHandReference
    {
        [SerializeField] private Image[] buttonsImage;
        [SerializeField] private Rigidbody rightButton;
        [SerializeField] private Rigidbody[] wrongButtons;

        private ControllersInteraction[] controllersInteractions;
        private ControllerInteraction[] vrControllerInteractions;

        void Update(){

            if (!protocolStarted || protocolFinished){
                return;
            }

            if (controllersInteractions != null){

                foreach(ControllersInteraction controller in controllersInteractions){
                
                    Rigidbody contactedRigidbody = controller.GetCurrentRigidbody();

                    if (contactedRigidbody == null){
                        continue;
                    }
        
                    if (contactedRigidbody == rightButton){

                        RightAnswer();

                        return;
                    } 

                    foreach(Rigidbody wrongButton in wrongButtons){

                        if (contactedRigidbody == wrongButton){

                            WrongAnswer();
                            return;
                        }
                    }
                }
            }

            else if (vrControllerInteractions != null){
                
                foreach(ControllerInteraction controller in vrControllerInteractions){
                
                    Rigidbody contactedRigidbody = controller.GetCurrentRigidbody();

                    if (contactedRigidbody == null){
                        continue;
                    }
        
                    if (contactedRigidbody == rightButton){

                        RightAnswer();

                        return;
                    } 

                    foreach(Rigidbody wrongButton in wrongButtons){

                        if (contactedRigidbody == wrongButton){

                            WrongAnswer();
                            return;
                        }
                    }
                }
            }
            
        }

        void RightAnswer(){

            StopTheProtocol();
        }

        void WrongAnswer(){
            
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


        void ProtocolStarted(){
            TakingReference();
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


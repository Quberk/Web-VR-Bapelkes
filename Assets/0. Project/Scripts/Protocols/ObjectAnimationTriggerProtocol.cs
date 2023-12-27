using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BapelkesWebVrAnc.DeviceControllers;
using BapelkesWebVrAnc.AnimationControls;
using WebXR.Interactions;

namespace BapelkesWebVrAnc.Protocols{

    /// <summary>
    /// Class ini berfungsi untuk memainkan Animasi Terteentu
    /// Lalu Protokol selesai ketika Animasi telah selesai
    /// JANGAN LUPA UNTUK MEMANGGIL ANIMATIONFINISHED() PADA FRAME AKHIR ANIMASI YANG DIMAKSUD
    /// </summary>

    public class ObjectAnimationTriggerProtocol : ProtocolManager, IRetakingHandReference
    {
        ///<param name = "targetObject">Sebagai Objek yang ingin dijadikan Trigger</param>
        ///<param name = "targetAnimator">Animator dari Objek yang ingin animasinya diTrigger</param>
        ///<param name = "simpleInteractable">Referensi Komponen SimpleInteractable yg berfungsi sbg penerima Trigger klik dari User</param>
        ///<param name = "objectGlowManager">Referensi Komponen ObjectGlowManager yg berfungsi utk membuat Objek Glow</param>
        ///<param name = "deactivateColliderAfterTrigger">Variable untuk mengetahui apakah setelah animasi terTrigger Collider dinonaktifkan atau tidak</param>

        [SerializeField] private GameObject targetObject;
        private Rigidbody targetRigidbody;
        [SerializeField] private Animator targetAnimator;
        [SerializeField] private string targetTrigger;
        [SerializeField] private bool deactivateColliderAfterTrigger;
        [SerializeField] private AnimationFinishedStatus animationFinishedStatus;
        private ControllersInteraction[] controllersInteractions;
        private ControllerInteraction[] vrControllerInteractions;

        private bool alreadyTriggered = false;

        void Start(){
            targetRigidbody = targetObject.GetComponent<Rigidbody>();
        }

        void Update(){

            if (!protocolStarted || protocolFinished)
                return;

            if (alreadyTriggered){

                if (animationFinishedStatus.GetAnimationFinishedStatus())
                    FinishedAnimation();
                    
                return;
            }
                

            if (controllersInteractions != null){

                foreach(ControllersInteraction controller in controllersInteractions){
                
                    Rigidbody contactedRigidbody = controller.GetCurrentRigidbody();

                    if (contactedRigidbody == null){
                        continue;
                    }
                        
                    if (contactedRigidbody == targetRigidbody){

                        alreadyTriggered = true;
                        BeginAnimation();

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

                        alreadyTriggered = true;
                        BeginAnimation();

                        return;
                    }   
                }
            }
                
        }

        void BeginAnimation(){
            
            targetAnimator.SetTrigger(targetTrigger);

            if (deactivateColliderAfterTrigger)
                targetObject.GetComponent<Collider>().enabled = false;
        }

        public void FinishedAnimation(){
            StopTheProtocol();
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
            TakingReference();
        }

        public override void StopTheProtocol()
        {
            protocolFinished = true;
            protocolStarted = false;
        }
    }

}


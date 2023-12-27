using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BapelkesWebVrAnc.Protocols.ActivateOrDeactivateProtocols{

    /// <summary>
    /// Class ini berfungsi untuk mengaktifkan atau menonaktifkan sebuah Gameobject ketika dijalankan
    /// </summary>
    /// 

    public class ActivateorDeactivateGameobjectProtocol : ProtocolManager
    {
        [SerializeField] private GameobjectActivationStatus gameobjectActivationStatus;
        public GameObject[] targetObjects;
        [SerializeField] private bool backToPreviousSetActive = false;

        [SerializeField] private bool mainProtocol = false;


        public virtual void ActivatingTargetObject(){

            foreach(GameObject targetObject in targetObjects){

                targetObject.SetActive(true);

            }

        }

        public virtual void DeactivatingTargetObject(){

            foreach(GameObject targetObject in targetObjects){

                targetObject.SetActive(false);
                
            }
        }

        private void ProtocolStarted(){

            if (gameobjectActivationStatus == GameobjectActivationStatus.Activate)
                ActivatingTargetObject();
            else
                DeactivatingTargetObject();

            if (mainProtocol)
                StopTheProtocol();
        }

        private void ProtocolFinished(){
            
            if (!backToPreviousSetActive)
                return;
                
            if (gameobjectActivationStatus == GameobjectActivationStatus.Activate)
                DeactivatingTargetObject();
            else
                ActivatingTargetObject();
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

    public enum GameobjectActivationStatus{
        Activate,
        Deactivate
    }
}


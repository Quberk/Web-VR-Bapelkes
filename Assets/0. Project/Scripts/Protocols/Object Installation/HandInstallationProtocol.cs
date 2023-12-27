using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BapelkesWebVrAnc.DeviceControllers;
using System.Linq;

namespace BapelkesWebVrAnc.Protocols.Installation{
    /// <summary>
    /// Class ini berfungsi menginstall Tangan Player pada
    /// Posisi tertentu yang dimana jika tangan Player sudah berada pada Posisi tertentu
    /// Maka tangan VRnya akan nonaktif dan Tangan Installmentnya aktif
    /// </summary>
    public class HandInstallationProtocol : ProtocolManager
    {
        private GameObject[] controllersInteractionsGameobject;
        private GameObject installedControllerInteraction;
        [SerializeField] private GameObject handInstallmentPos;
        [SerializeField] private GameObject temporaryHandInstallment;
        [SerializeField] private float radiusDetection;
        [SerializeField] private float handInstallmentTime;
        private float handInstallmentCounter = 0f;
        private bool startCounting = false;

        void Start(){
            handInstallmentPos.SetActive(false);
            temporaryHandInstallment.SetActive(false);
        }

        void Update(){

            if (!protocolStarted || protocolFinished)
                return;

            StartTheCounting();

            CheckingDistanceFromHands();
        }

        void StartTheCounting(){

            if (!startCounting)
                return;

            handInstallmentCounter += Time.deltaTime;

            if (handInstallmentCounter >= handInstallmentTime){
                ProtocolStopped();
            }
        }

        void CheckingDistanceFromHands(){

            if (startCounting)
                return;

            foreach(GameObject controller in controllersInteractionsGameobject){
                
                float distanceFromHand = Vector3.Distance(handInstallmentPos.transform.position, controller.transform.position);

                if (distanceFromHand <= radiusDetection){

                    startCounting = true;
                    installedControllerInteraction = controller;
                    installedControllerInteraction.SetActive(false);
                    handInstallmentPos.SetActive(false);
                    temporaryHandInstallment.SetActive(true);
                    break;
                }

            }
        }

        void TakingReference(){
            
            GameObject[] hands = GameObject.FindGameObjectsWithTag("Hand");
            controllersInteractionsGameobject = new GameObject[hands.Length];

            for (int i = 0; i < controllersInteractionsGameobject.Length; i++){
                controllersInteractionsGameobject[i] = hands[i];
            }
        }

        void ProtocolStarted(){

            handInstallmentPos.SetActive(true);
            TakingReference();

        }

        void ProtocolStopped(){
            StopTheProtocol();
            temporaryHandInstallment.SetActive(false);
            installedControllerInteraction.SetActive(true);
            
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


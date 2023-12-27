using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BapelkesWebVrAnc.DeviceControllers;
using UnityEngine.AI;
using WebXR.Interactions;

namespace BapelkesWebVrAnc.Protocols.Installation{

    /// <summary>
    /// Class ini berfungsi untuk mengetahui Status Object To INstall
    /// Apakah sudah berada di Posisi yang seharusnya atau belum
    /// </summary>
    public class ObjectToInstallStatus : MonoBehaviour, IRetakingHandReference
    {
        private GameObject contactedGameobject;
        private ControllersInteraction[] controllersInteractions;
        private ControllerInteraction[] vrControllerInteractions;
        private bool alreadyTakingReference = false;

        void Update(){

            if (alreadyTakingReference)
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

        private void OnTriggerEnter(Collider other){

            if (controllersInteractions != null){

                foreach(ControllersInteraction controller in controllersInteractions){
                
                    if (controller.GetGrabbingStatus()){
                        contactedGameobject = other.gameObject;
                        return;
                    }

                    else{
                        contactedGameobject = null;
                    }
                }
            }

            else if (vrControllerInteractions != null){
                foreach(ControllerInteraction controller in vrControllerInteractions){
                
                    if (controller.GetGrabbingStatus()){
                        contactedGameobject = other.gameObject;
                        return;
                    }

                    else{
                        contactedGameobject = null;
                    }
                }
            }
        }

        private void OnTriggerStay(Collider other){
            

        }

        private void OnTriggerExit(Collider other){
            if (contactedGameobject == other)
                contactedGameobject = null;
        }

        public GameObject GetBeingContactedStatus(){
            return contactedGameobject;
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



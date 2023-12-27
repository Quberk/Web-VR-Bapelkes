using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BapelkesWebVrAnc.DeviceControllers;
using WebXR;
using WebXR.Interactions;
using BapelkesWebVrAnc.Protocols;

namespace BapelkesWebVrAnc.ObjectGrabbed{

    public class ObjectGrabbedAnimationControl : MonoBehaviour, IRetakingHandReference
    {
        [SerializeField] private Rigidbody targetRigidbody;
        [SerializeField] private Animator targetAnimator;
        [SerializeField] private string grabbedAnimationName;
        [SerializeField] private string ungrabbedAnimationName;

        private ControllersInteraction[] controllersInteractions;
        private ControllerInteraction[] vrControllerInteractions;
        private ObjectGrabbedState objectGrabbedState;
        private ObjectGrabbedState lastObjectGrabbedState;
        private bool alreadyTakingReference = false;
        private string playCertainAnimation = "";

        void Start()
        {
            lastObjectGrabbedState = ObjectGrabbedState.NotGrabbed;
        }

        // Update is called once per frame
        void Update()
        {
            if (!alreadyTakingReference){

                TakingReference();
            }

            if (playCertainAnimation != ""){
                return;
            }

            CheckingGrabbedStatus();
        }

        public void PlayingCertainAnimation(string animationName){
            playCertainAnimation = animationName;
            targetAnimator.Play(playCertainAnimation);
        }

        public void StopPlayingCertainAnimation(){
            playCertainAnimation = "";
            targetAnimator.Play(ungrabbedAnimationName);
        }

        void CheckingGrabbedStatus(){
            
            if (controllersInteractions != null){
                foreach(ControllersInteraction controller in controllersInteractions){
                
                    Rigidbody contactedRigidbody = controller.GetCurrentRigidbody();

                    if (contactedRigidbody == targetRigidbody){
                        Debug.Log("Animasinya di Open kok");
                        objectGrabbedState = ObjectGrabbedState.Grabbed;
                        PlayGrabbedAnimation(objectGrabbedState);
                        lastObjectGrabbedState = objectGrabbedState;
                        break;
                    }

                    else{
                        objectGrabbedState = ObjectGrabbedState.NotGrabbed;
                        PlayUnGrabbedAnimation(objectGrabbedState);
                        lastObjectGrabbedState = objectGrabbedState;
                        break;
                    }
                }
            }

            else if (vrControllerInteractions != null){
                foreach(ControllerInteraction controller in vrControllerInteractions){
                
                    Rigidbody contactedRigidbody = controller.GetCurrentRigidbody();

                    if (contactedRigidbody == targetRigidbody){
                        Debug.Log("Animasinya di Open kok");
                        objectGrabbedState = ObjectGrabbedState.Grabbed;
                        PlayGrabbedAnimation(objectGrabbedState);
                        lastObjectGrabbedState = objectGrabbedState;
                        break;
                    }

                    else{
                        objectGrabbedState = ObjectGrabbedState.NotGrabbed;
                        PlayUnGrabbedAnimation(objectGrabbedState);
                        lastObjectGrabbedState = objectGrabbedState;
                        break;
                    }
                }
            }
            

        }

        void PlayGrabbedAnimation(ObjectGrabbedState objectGrabbedState){

            if (lastObjectGrabbedState != objectGrabbedState){

                targetAnimator.Play(grabbedAnimationName);
            }
        }

        void PlayUnGrabbedAnimation(ObjectGrabbedState objectGrabbedState){

            if (lastObjectGrabbedState != objectGrabbedState){

                targetAnimator.Play(ungrabbedAnimationName);
            }
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

    }

    public enum ObjectGrabbedState{
        Grabbed,
        NotGrabbed
    }
}

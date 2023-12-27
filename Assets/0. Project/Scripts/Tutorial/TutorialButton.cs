using UnityEngine;
using BapelkesWebVrAnc.DeviceControllers;
using BapelkesWebVrAnc.AnimationControls;
using WebXR.Interactions;
using BapelkesWebVrAnc.Protocols;

namespace BapelkesWebVrAnc.Tutorial{
    public class TutorialButton : ProtocolManager, IRetakingHandReference
    {
        private ControllersInteraction[] controllersInteractions;
        private ControllerInteraction[] vrControllerInteractions;
        [SerializeField] private GameObject targetObject;
        private Rigidbody targetRigidbody;
        [SerializeField] private Animator targetAnimator;
        [SerializeField] private bool deactivateColliderAfterTrigger;

        private bool firstTimeOpenPanel = false;

        void Start(){
            StartTheProtocol();
            targetRigidbody = targetObject.GetComponent<Rigidbody>();
            targetAnimator.gameObject.SetActive(false);
        }

        void Update(){
            
            if (!protocolStarted || protocolFinished)
                return;

            if (controllersInteractions != null){

                foreach(ControllersInteraction controller in controllersInteractions){
                
                    Rigidbody contactedRigidbody = controller.GetCurrentRigidbody();

                    if (contactedRigidbody == null){
                        continue;
                    }
                        
                    if (contactedRigidbody == targetRigidbody){

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

                        BeginAnimation();

                        return;
                    }   
                }
            }
        }

        void BeginAnimation(){

            targetAnimator.gameObject.SetActive(true);

            if (!firstTimeOpenPanel){
                firstTimeOpenPanel = true;
            }
            else
                targetAnimator.SetTrigger("Start");

            if (deactivateColliderAfterTrigger)
                targetObject.GetComponent<Collider>().enabled = false;
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



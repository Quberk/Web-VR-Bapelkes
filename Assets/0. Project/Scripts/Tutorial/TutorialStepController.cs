using UnityEngine;
using BapelkesWebVrAnc.DeviceControllers;
using BapelkesWebVrAnc.AnimationControls;
using WebXR.Interactions;
using BapelkesWebVrAnc.Protocols;
using UnityEditor;


namespace BapelkesWebVrAnc.Tutorial{

    public class TutorialStepController : MonoBehaviour, IRetakingHandReference
    {
        private int step = 1;
        [SerializeField] private int maxStep = 3;
        [SerializeField] private GameObject previousButton;
        [SerializeField] private GameObject nextButton;
        [SerializeField] private GameObject okButton;

        [SerializeField] private GameObject[] stepPanels;
        [SerializeField] private Animator panelAnimator;


        private Rigidbody previousButtonRb;
        private Rigidbody nextButtonRb;
        private Rigidbody okButtonRb;

        private ControllersInteraction[] controllersInteractions;
        private ControllerInteraction[] vrControllerInteractions;

        void Start(){

            TakingReference();
            previousButtonRb = previousButton.GetComponent<Rigidbody>();
            nextButtonRb = nextButton.GetComponent<Rigidbody>();
            okButtonRb = okButton.GetComponent<Rigidbody>();

            HideAllPanels();
            stepPanels[0].SetActive(true);
        }

        void Update(){

            CheckingStep();
            CheckingButtonPressed();
        }

        void CheckingStep(){

            if (step == 1){
                okButton.SetActive(false);
                nextButton.SetActive(true);
                previousButton.SetActive(false);
            }

            else if (step == maxStep){
                okButton.SetActive(true);
                nextButton.SetActive(false);
                previousButton.SetActive(true);
            }

            else{
                okButton.SetActive(false);
                nextButton.SetActive(true);
                previousButton.SetActive(true);
            }
        }

        void CheckingButtonPressed(){

            if (controllersInteractions != null){

                foreach(ControllersInteraction controller in controllersInteractions){
                
                    Rigidbody contactedRigidbody = controller.GetCurrentRigidbody();

                    if (contactedRigidbody == null){
                        continue;
                    }
                    
                    // Next Button Pressed
                    if (contactedRigidbody == nextButtonRb){

                        NextStep();
                        return;
                    }

                    // Previous Button Pressed
                    if (contactedRigidbody == previousButtonRb){

                        PreviousStep();
                        return;
                    }

                    // Ok Button Pressed
                    if (contactedRigidbody == okButtonRb){

                        CloseTutorial();
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

                    // Next Button Pressed
                    if (contactedRigidbody == nextButtonRb){

                        NextStep();
                        return;
                    }

                    // Previous Button Pressed
                    if (contactedRigidbody == previousButtonRb){

                        PreviousStep();
                        return;
                    }

                    // Ok Button Pressed
                    if (contactedRigidbody == okButtonRb){

                        CloseTutorial();
                        return;
                    }
                }
            }
        }

        void NextStep(){

            step++;
            HideAllPanels();
            stepPanels[step - 1].SetActive(true);
        }

        void PreviousStep(){

            step--;
            HideAllPanels();
            stepPanels[step - 1].SetActive(true);
        }

        void CloseTutorial(){

            step = 1;
            HideAllPanels();
            stepPanels[0].SetActive(true);
            panelAnimator.SetTrigger("Finish");
        }

        void HideAllPanels(){

            for (int i = 0; i < maxStep; i++){

                stepPanels[i].SetActive(false);
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
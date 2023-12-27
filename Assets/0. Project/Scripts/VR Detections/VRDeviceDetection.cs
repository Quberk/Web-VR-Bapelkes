using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using UnityEngine.Networking;
using WebXR;
using System.Linq;
using BapelkesWebVrAnc.Protocols;
using System;

namespace BapelkesWebVrAnc.VRDetections{
    public class VRDeviceDetection : MonoBehaviour
    {
        [SerializeField] private bool inEditor;
        [SerializeField] private GameObject vrDeviceController;
        private WebXRManager webXRManager;
        [SerializeField] private GameObject mouseAndKeyboardController;
        [SerializeField] private GameObject vrHandRight, vrHandLeft;
        [SerializeField] private GameObject keyboardHand;
        [SerializeField] private GameObject tutorialWeb;
        [SerializeField] private GameObject tutorialVr;

        private bool usingVRDevice;
        private bool usingKeyboardAndMouseDevice;

        public bool GetVRDeviceStatus(){
            return usingVRDevice;
        }

        private void UsingVrDevice(){

            if (usingVRDevice)
                return;

            usingVRDevice = true;
            usingKeyboardAndMouseDevice = false;
            vrDeviceController.SetActive(true);
            mouseAndKeyboardController.SetActive(false);
            vrHandRight.tag = "Hand";
            vrHandLeft.tag = "Hand";
            RetakingHandReference();

            if (tutorialVr != null){
                tutorialVr.SetActive(true);
                tutorialWeb.SetActive(false);
            }
        }

        private void UsingMouseAndKeyboardDevice(){

            if (usingKeyboardAndMouseDevice)
                return;
            
            usingVRDevice = false;
            usingKeyboardAndMouseDevice = true;
            vrDeviceController.SetActive(false);
            mouseAndKeyboardController.SetActive(true);
            keyboardHand.tag = "Hand";
            RetakingHandReference();

            if (tutorialVr != null){
                tutorialVr.SetActive(false);
                tutorialWeb.SetActive(true);
            }

        }

        private void RetakingHandReference(){
            var ss = FindObjectsOfType<MonoBehaviour>().OfType<IRetakingHandReference>();
                foreach (IRetakingHandReference s in ss) {
                    s.RetakingHandReference();
             }
        }

        void Start()
        {
            if (inEditor){
                if (ExampleUtil.isPresent())
                {
                    
                    Debug.Log("Perangkat VR terhubung di Unity Editor!");
                    UsingVrDevice();
                }
                else
                {

                    Debug.Log("Tidak ada perangkat VR terhubung di Unity Editor.");
                    UsingMouseAndKeyboardDevice();
                }
            }

            else{

                webXRManager = vrDeviceController.GetComponent<WebXRManager>();
            }
        }
        

        void Update(){

            if (inEditor){

                return;
            }

            else{

                if (webXRManager.isSupportedVR == true){

                    UsingVrDevice();
                }
                else{

                    UsingMouseAndKeyboardDevice();
                }
            }

        }
    }
}


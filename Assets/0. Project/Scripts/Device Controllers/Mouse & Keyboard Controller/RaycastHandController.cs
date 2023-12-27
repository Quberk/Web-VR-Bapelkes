using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BapelkesWebVrAnc.DeviceControllers.MouseAndKeyboardController{

    /// <summary>
    /// Class ini berfungsi untuk mengatur Keyboard & Mouse Controller
    /// Mengatur Interaction Hand untuk Mouse & Keyboard
    /// </summary>
    /// 
    public class RaycastHandController : MonoBehaviour
    {

        [SerializeField] private LineRenderer rayEffect;
        [SerializeField] private float rayEffectLength = 12f;
        [SerializeField] private GameObject forwardPoint;
        private Rigidbody contactedRb;

        void Start(){

            DeactivateRayCast();
        }

        public void ActivateRayCast(){

            rayEffect.gameObject.SetActive(true);

            Vector3 direction = forwardPoint.transform.position - transform.position;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, 20f)){

                Debug.DrawRay(transform.position, direction * hitInfo.distance * rayEffectLength, Color.red);

                if (hitInfo.rigidbody != null)
                    contactedRb = hitInfo.rigidbody;
                else
                    contactedRb = null;

                rayEffect.SetPosition(1, forwardPoint.transform.localPosition * hitInfo.distance * rayEffectLength);

            }

            else{
                Debug.DrawRay(transform.position, direction * 12f, Color.green);

                contactedRb = null;

                rayEffect.SetPosition(1, forwardPoint.transform.localPosition * rayEffectLength);
            }     
        }

        public void DeactivateRayCast(){

            rayEffect.gameObject.SetActive(false);
            contactedRb = null;

        }

        public void DetectingInteractableObject(){
            rayEffect.SetColors(Color.red, Color.red);
        }

        public void DetectingNonInteractableObject(){
            rayEffect.SetColors(Color.white, Color.white);
        }

        public Rigidbody GetContactedRaycastRigidbody(){
            return contactedRb;
        }
    }
}


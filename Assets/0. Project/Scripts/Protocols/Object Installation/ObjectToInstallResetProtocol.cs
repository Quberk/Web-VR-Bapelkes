using System.Collections;
using System.Collections.Generic;
using BapelkesWebVrAnc.Protocols;
using UnityEngine;

namespace BapelkesWebVrAnc.Protocols.Installation{
    /// <summary>
    /// Class ini berfungsi untuk mereset Object To Install
    /// Posisi Awalnya / Animasi
    /// </summary>
    /// 
    public class ObjectToInstallResetProtocol : ProtocolManager
    {
        [SerializeField] private GameObject targetGameobject;
        [SerializeField] private Transform targetInitialPos;
        [SerializeField] private Animator targetAnimator;
        [SerializeField] private string targetInitialAnimation;

        [SerializeField] private bool resetRotation;
        private Vector3 initialRotation;

        void Start(){

            if (resetRotation)
                initialRotation = new Vector3 (targetGameobject.transform.rotation.x,
                                                targetGameobject.transform.rotation.y,
                                                targetGameobject.transform.rotation.z);
        }

        void StartResetting(){

            if (targetGameobject != null){

                if (targetGameobject.GetComponent<Rigidbody>()){
                    targetGameobject.GetComponent<Rigidbody>().isKinematic = false;
                    targetGameobject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }

            if (targetInitialPos != null)
                targetGameobject.transform.position = targetInitialPos.position;


            if (targetAnimator != null){
                targetAnimator.Play(targetInitialAnimation);
            }

            if (resetRotation)
                targetGameobject.transform.rotation = Quaternion.Euler(initialRotation);

            StopTheProtocol();
        }

        //=====================================OVERRIDE METHODS============================================================
        
        public override void StartTheProtocol()
        {
            StartResetting();
            protocolStarted = true;
        }

        public override void StopTheProtocol()
        {
            protocolFinished = true;
            protocolStarted = false;
        }
    }
}


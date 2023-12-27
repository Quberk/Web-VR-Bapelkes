using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BapelkesWebVrAnc.DeviceControllers{
    public class ControllersInteraction : MonoBehaviour
    {
        protected Rigidbody currentRigidBody;
        protected bool grabbing = false;
        protected bool pointing = false;

        public Rigidbody GetCurrentRigidbody(){
            
            return currentRigidBody;
        }

        public bool GetGrabbingStatus(){
            return grabbing;
        }

        public bool GetPointingStatus(){
            return pointing;
        }
    }
}


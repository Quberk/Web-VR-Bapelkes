using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BapelkesWebVrAnc.DeviceControllers.MouseAndKeyboardController{

    [RequireComponent(typeof(FixedJoint))]
    [RequireComponent(typeof(RaycastHandController))]

    /// <summary>
    /// Class ini berfungsi untuk mengatur Keyboard & Mouse Controller
    /// Mengatur Interaction Hand untuk Mouse & Keyboard
    /// </summary>
    /// 
    public class HandControlInteraction : ControllersInteraction
    {
        private RaycastHandController raycastHandController;
        private FixedJoint attachJoint;

        private List<Rigidbody> contactRigidBodies = new List<Rigidbody> ();
        
        private Transform t;
        private Vector3 lastPosition;
        private Quaternion lastRotation;

        [SerializeField] private Animator anim;

        void Awake()
        {
            t = transform;
            attachJoint = GetComponent<FixedJoint> ();
            raycastHandController = GetComponent<RaycastHandController>();
        }

        void Update()
        {
            Pointing();

            if (!grabbing && !pointing)
                currentRigidBody = null;

            if (Input.GetKeyDown(KeyCode.E))
                Pickup();

            if (Input.GetKeyUp(KeyCode.E))
                Drop();

            if (Input.GetKeyDown(KeyCode.Q))
                Point();

            if (Input.GetKeyUp(KeyCode.Q))
                StopPoint();
        }

        void FixedUpdate()
        {
            if (!currentRigidBody) return;
            
            lastPosition = currentRigidBody.position;
            lastRotation = currentRigidBody.rotation;
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Interactable"))
                return;

            contactRigidBodies.Add(other.attachedRigidbody);
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Interactable"))
                return;

            contactRigidBodies.Remove(other.attachedRigidbody);
        }

        public void Point(){

            anim.Play("Take1");
            
            pointing = true;

        }

        void Pointing(){

            if (!pointing)
                return;

            currentRigidBody = null;

            raycastHandController.ActivateRayCast();

            if (raycastHandController.GetContactedRaycastRigidbody() == null){
                raycastHandController.DetectingNonInteractableObject();
                return;
            }

            if (raycastHandController.GetContactedRaycastRigidbody().gameObject.CompareTag("Interactable")){
                raycastHandController.DetectingInteractableObject();
            }
            else{
                raycastHandController.DetectingNonInteractableObject();
            }
        }

        public void StopPoint(){

            anim.Play("Idle");

            pointing = false;


            if (raycastHandController.GetContactedRaycastRigidbody() == null){

                currentRigidBody = null;
                
            }
            else if (raycastHandController.GetContactedRaycastRigidbody().gameObject.CompareTag("Interactable")){

                currentRigidBody = raycastHandController.GetContactedRaycastRigidbody();
            }
            else{

                currentRigidBody = null;
            }

            raycastHandController.DeactivateRayCast();
        }

        public void Pickup() {

            anim.Play("Grab1");

            grabbing = true;

            currentRigidBody = GetNearestRigidBody();

            if (!currentRigidBody)
                return;

            currentRigidBody.MovePosition(t.position);
            attachJoint.connectedBody = currentRigidBody;
            
            lastPosition = currentRigidBody.position;
            lastRotation = currentRigidBody.rotation;
        }

        public void Drop() {

            anim.Play("Idle");

            grabbing = false;

            if (!currentRigidBody)
                return;

            attachJoint.connectedBody = null;
            
            currentRigidBody.velocity = Vector3.zero;
            
            var deltaRotation = currentRigidBody.rotation * Quaternion.Inverse(lastRotation);
            float angle;
            Vector3 axis;
            deltaRotation.ToAngleAxis(out angle, out axis);
            angle *= Mathf.Deg2Rad;
            currentRigidBody.angularVelocity = axis * angle / Time.deltaTime;
            
            currentRigidBody = null;
        }

        private Rigidbody GetNearestRigidBody() {
            Rigidbody nearestRigidBody = null;
            float minDistance = float.MaxValue;
            float distance;

            foreach (Rigidbody contactBody in contactRigidBodies) {
                distance = (contactBody.transform.position - t.position).sqrMagnitude;

                if (distance < minDistance) {
                    minDistance = distance;
                    nearestRigidBody = contactBody;
                }
            }

            return nearestRigidBody;
        }
    }
}


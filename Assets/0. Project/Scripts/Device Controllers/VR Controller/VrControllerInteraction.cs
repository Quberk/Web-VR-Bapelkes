using UnityEngine;
using System.Collections.Generic;
using WebXR;
using BapelkesWebVrAnc.DeviceControllers.MouseAndKeyboardController;


namespace BapelkesWebVrAnc.DeviceControllers.VRControllers{

    /// <summary>
    /// Class ini berfungsi untuk mengatur VR Controller
    /// Baik Kamera dan Tangan Controller
    /// </summary>

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(FixedJoint))]
    //[RequireComponent(typeof(WebXRController))]
    public class VrControllerInteraction : ControllersInteraction
    {
        [SerializeField] private GameObject mainGameobject;
        [SerializeField] private GameObject cameraReference;
        [SerializeField] private GameObject cameraMain;
        private Rigidbody mainGameobjectRb;
        [SerializeField] private float straffeSpeed = 5f;
        
        private FixedJoint attachJoint;
        private WebXRController controller;
        private Transform t;
        private Vector3 lastPosition;
        private Quaternion lastRotation;

        private List<Rigidbody> contactRigidBodies = new List<Rigidbody> ();

        private Animator anim;

        private RaycastHandController raycastHandController;

        private bool rightHandHorizontal = false;

        private float direction = 0f;
        
        void Awake()
        {
            t = transform;
            attachJoint = GetComponent<FixedJoint> ();
            anim = GetComponent<Animator>();
            controller = GetComponent<WebXRController>();

            mainGameobjectRb = mainGameobject.GetComponent<Rigidbody>();
            raycastHandController = GetComponent<RaycastHandController>();
        }

        void Update()
        {

            if (controller.GetButtonDown(WebXRController.ButtonTypes.Trigger))
                Point();

            if (controller.GetButtonUp(WebXRController.ButtonTypes.Trigger))
                StopPoint();

            if (controller.GetButtonDown(WebXRController.ButtonTypes.Grip))
                Pickup();

            if (controller.GetButtonUp(WebXRController.ButtonTypes.Grip))
                Drop();

            Movement();
            Pointing();
            CameraMovement();

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
            Debug.Log("Ada yang terkena kontak dengan tangan");
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Interactable"))
                return;

            contactRigidBodies.Remove(other.attachedRigidbody);
        }

        void Movement(){

            float x = Input.GetAxis("Horizontal") * Time.deltaTime * straffeSpeed;
            float z = Input.GetAxis("Vertical") * Time.deltaTime * straffeSpeed;

            if (x == 0 && z == 0){
                mainGameobjectRb.velocity = Vector3.zero;
                mainGameobjectRb.angularVelocity = Vector3.zero;
            }

            Vector3 forward = cameraReference.transform.forward * z;
            Vector3 right = cameraReference.transform.right * x;

            Debug.DrawRay(cameraReference.transform.position, cameraReference.transform.forward);

            mainGameobject.transform.Translate(forward + right, Space.World);
            
        }

        void CameraMovement(){

            float x = Input.GetAxis("HorizontalR");

            if (x > 0){
                
                rightHandHorizontal = true;
                direction = 1f;
            }

            else if (x < 0){
                rightHandHorizontal = true;
                direction = -1f;
            }

            if (rightHandHorizontal && x == 0){
                rightHandHorizontal = false;
                Vector3 newRotation = new Vector3(0, direction * 20f, 0);
                mainGameobject.gameObject.transform.Rotate(newRotation, Space.World);
                //cameraMain.gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                //cameraReference.gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
                
        }

        public void Point(){
            Debug.Log("Saya menunjuk");

            anim.Play("Take1");
            
            pointing = true;

            if (!currentRigidBody)
                return;
        }

        void Pointing(){

            if (!pointing)
                return;

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
            Debug.Log("Saya Grab");

            anim.Play("Grab1");

            currentRigidBody = GetNearestRigidBody ();

            if (!currentRigidBody)
                return;

            currentRigidBody.MovePosition(t.position);
            attachJoint.connectedBody = currentRigidBody;
            
            lastPosition = currentRigidBody.position;
            lastRotation = currentRigidBody.rotation;
        }

        public void Drop() {

            anim.Play("Idle");

            if (!currentRigidBody)
                return;

            attachJoint.connectedBody = null;
            
            currentRigidBody.velocity = (currentRigidBody.position - lastPosition) / Time.deltaTime;
            
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
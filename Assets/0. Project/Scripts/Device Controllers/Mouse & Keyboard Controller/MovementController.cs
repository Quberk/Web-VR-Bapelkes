﻿using UnityEngine;
//using WebXR;

namespace BapelkesWebVrAnc.DeviceControllers.MouseAndKeyboardController{

    /// <summary>
    /// Class ini berfungsi untuk mengatur Keyboard & Mouse
    /// Berfungsi untuk mengatur pergerakan Player
    /// </summary>
    /// 
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private GameObject mainGameobject;
        private Rigidbody mainGameobjectRb;

        [Tooltip("Enable/disable rotation control. For use in Unity editor only.")]
        public bool rotationEnabled = true;

        [Tooltip("Enable/disable translation control. For use in Unity editor only.")]
        public bool translationEnabled = true;

        //private WebXRDisplayCapabilities capabilities;

        [Tooltip("Mouse sensitivity")]
        public float mouseSensitivity = 1f;

        [Tooltip("Straffe Speed")]
        public float straffeSpeed = 5f;

        private float minimumX = -360f;
        private float maximumX = 360f;

        private float minimumY = -90f;
        private float maximumY = 90f;

        private float rotationX = 0f;
        private float rotationY = 0f;

        Quaternion originalRotation;

        void Start()
        {
            //WebXRManager.Instance.OnXRChange += onXRChange;
            //WebXRManager.Instance.OnXRCapabilitiesUpdate += onXRCapabilitiesUpdate;
            originalRotation = transform.localRotation;

            mainGameobjectRb = mainGameobject.GetComponent<Rigidbody>();
        }

        void Update() {

            if (translationEnabled)
            {
                float x = Input.GetAxis("Horizontal") * Time.deltaTime * straffeSpeed;
                float z = Input.GetAxis("Vertical") * Time.deltaTime * straffeSpeed;

                if (x == 0 && z == 0){
                    mainGameobjectRb.velocity = Vector3.zero;
                    mainGameobjectRb.angularVelocity = Vector3.zero;
                }

                mainGameobject.transform.Translate((mainGameobject.transform.forward * z) + (mainGameobject.transform.right * x));

            }

            if (rotationEnabled && Input.GetMouseButton(0))
            {

                rotationX += Input.GetAxis ("Mouse X") * mouseSensitivity;
                rotationY += Input.GetAxis ("Mouse Y") * mouseSensitivity;

                rotationX = ClampAngle (rotationX, minimumX, maximumX);
                rotationY = ClampAngle (rotationY, minimumY, maximumY);

                Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
                Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, Vector3.left);

                transform.localRotation = originalRotation * xQuaternion * yQuaternion;
                mainGameobject.transform.rotation = originalRotation * xQuaternion;
            }
        }

        void DisableEverything()
        {
            translationEnabled = false;
            rotationEnabled = false;
        }

        /// Enables rotation and translation control for desktop environments.
        /// For mobile environments, it enables rotation or translation according to
        /// the device capabilities.
        /*void EnableAccordingToPlatform()
        {
            rotationEnabled = translationEnabled = !capabilities.supportsImmersiveVR;
        }*/

        public static float ClampAngle (float angle, float min, float max)
        {
            if (angle < -360f)
                angle += 360f;
            if (angle > 360f)
                angle -= 360f;
            return Mathf.Clamp (angle, min, max);
        }

    }
}


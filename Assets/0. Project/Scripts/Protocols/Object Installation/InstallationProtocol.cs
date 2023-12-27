using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BapelkesWebVrAnc.DeviceControllers.VRControllers;
using BapelkesWebVrAnc.DeviceControllers;
using WebXR.Interactions;

namespace BapelkesWebVrAnc.Protocols.Installation{

    /// <summary>
    /// Class ini berfungsi untuk memainkan Animasi Terteentu
    /// Lalu Protokol selesai ketika Animasi telah selesai
    /// JANGAN LUPA UNTUK MEMANGGIL ANIMATIONFINISHED() PADA FRAME AKHIR ANIMASI YANG DIMAKSUD
    /// </summary>

    public class InstallationProtocol : ProtocolManager, IRetakingHandReference
    {
        ///<param name = "objectToInstallStatus">Objek yang ingin di pasang</param>
        ///<param name = "installedObjectPosition">Trigger Posisi dari Objek yang ingin dipasang</param>
        ///<param name = "objectInstalledTo">Objek yang ingin dipasangkan nantinya ObjectToInstall dijadikan Child dari Objek ini</param>
        ///<param name = "protocolStarted">Variabel untuk mengetahui apakah Simulasi Instalasi sudah berkalan atau belum</param>
        ///<param name = "grabInteractable">XRGrabInteractable Component pada objek yang ingin dipasang</param>
        ///<param name = "objectIsGrabbed">Info apakah objek yang ingin dipasang sedang di Grab atau tidak</param>
        
        [SerializeField] private ObjectToInstallStatus objectToInstallStatus;
        private Rigidbody objectToInstallRigidbody;
        private GameObject objectToInstallGameobject;
        [SerializeField] private GameObject installedObjectPosition;

        [SerializeField] private bool installedObjectPositionPosReference;
        [SerializeField] private bool installedObjectPositionRotReference;
        [SerializeField] private bool installedObjectPositionLocalScaleReference;

        private ControllersInteraction[] controllersInteractions;
        private ControllerInteraction[] vrControllerInteractions;
        
        private bool objectIsGrabbed = false; //Variable untuk mengetahui apakah ObjectToInstall sedang di grab atau tidak


        /// <summary>
        /// Pada awal permainan maka Posisi objek yang ingin dipasang akan di nonaktifkan
        /// Juga mengambil referensi dari XRGrabInteractable pada objek yang ingin dipasang
        /// Memasukkan juga Method yang berfungsi mengetahui apakah objek yang ingin dipasang sedang di Grab atau tidak
        /// </summary>
        void Start(){

            installedObjectPosition.SetActive(false);

        }

        void Update(){

            if (!protocolStarted || protocolFinished){
                return;
            }

            ObjectToInstallGrabbedStatus();
            ObjectInstalledStatus();

        }

        void ObjectToInstallGrabbedStatus(){

            if (controllersInteractions != null){

                foreach(ControllersInteraction controller in controllersInteractions){
                
                    Rigidbody contactedRigidbody = controller.GetCurrentRigidbody();

                    objectIsGrabbed = false;

                    if (contactedRigidbody == objectToInstallRigidbody){
                        objectIsGrabbed = true;
                        return;
                    }
                }
            }

            else if (vrControllerInteractions != null){
                foreach(ControllerInteraction controller in vrControllerInteractions){
                
                    Rigidbody contactedRigidbody = controller.GetCurrentRigidbody();

                    objectIsGrabbed = false;

                    if (contactedRigidbody == objectToInstallRigidbody){
                        objectIsGrabbed = true;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Method yang berfungsi untuk mengaktifkan Installation Protocol tandanya Objek akan Glow dan Pemasangan dapat dilakukan
        /// Menyalakan Glow Effect pada Objek yang ingin dipasang
        /// </summary>
        private void StartTheInstallationProtocol(){

            installedObjectPosition.SetActive(true);

            TakingReference();
        }

        /// <summary>
        /// Mengecek apakah Objek yang ingin diinstall sudah berada pada Trigger Posisinya
        /// Dicek juga apakah Protocol Instalasi sudah dimulai
        /// Dicek juga apakah Objek sudah berhenti diGrab
        /// Setelah semua kondisi benar barulah Objek dianggap terpasang
        /// Objek yang ingin diinstall ditaruh pada Posisi yang seharusnya dan dimasukkan sbg Child dari Objek yang ingin dipasangkan
        /// Objek yang dipasang juga dibuat agar tdk dapat di Grab lagi
        /// Objek yang dijadikan referensi posisi juga akan dinonaktifkan
        /// Rigidbody dari Objek yang dipasang akan dinonaktfikan
        /// Posisi & Rotasi juga disesuaikan
        /// Mematikan glow effect
        /// Menyelesaikan Protokol
        /// </summary>
        private void ObjectInstalledStatus(){

            GameObject contactedObjectToInstallWith = objectToInstallStatus.GetBeingContactedStatus();
            
            if (protocolStarted && !objectIsGrabbed && contactedObjectToInstallWith == installedObjectPosition.gameObject){

                if (installedObjectPositionPosReference)
                    objectToInstallStatus.gameObject.transform.position = installedObjectPosition.transform.position;
                if (installedObjectPositionRotReference)
                    objectToInstallStatus.gameObject.transform.rotation = installedObjectPosition.transform.rotation;
                if (installedObjectPositionLocalScaleReference)
                    objectToInstallStatus.gameObject.transform.localScale = installedObjectPosition.transform.localScale;

                installedObjectPosition.SetActive(false);

                objectToInstallRigidbody.isKinematic = true;

                //Finish The Protocol
                StopTheProtocol();

            }
        }

        void TakingReference(){

            objectToInstallGameobject = objectToInstallStatus.gameObject;
            objectToInstallRigidbody = objectToInstallGameobject.GetComponent<Rigidbody>();
            
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


        //=====================================OVERRIDE METHODS============================================================
        
        public override void StartTheProtocol()
        {
            StartTheInstallationProtocol();
            protocolStarted = true;
        }

        public override void StopTheProtocol()
        {
            protocolFinished = true;
            protocolStarted = false;
        }
    }
}


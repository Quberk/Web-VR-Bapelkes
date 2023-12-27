using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BapelkesWebVrAnc.Protocols{

    /// <summary>
    /// Class ini berfungsi untuk memberikan sebuah titik pada Player
    /// Ketika Player berada dekat dengan titik maka titiknya menghilang
    /// </summary>
    /// 

    public class GuidePointProtocol : ProtocolManager
    {
        [SerializeField] private GameObject guidePoint;
        [SerializeField] private string playerTag;

        void Start(){

            guidePoint.SetActive(false);
        }

        private void OnTriggerEnter(Collider other){
            if (other.transform.CompareTag(playerTag) && protocolStarted){
                StopTheProtocol();
            }
        }

        void ProtocolStarted(){
            Debug.Log("Berjalanja pak Eko");
            guidePoint.SetActive(true);
        }

        void ProtocolFinished(){
            Destroy(guidePoint);
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
            ProtocolFinished();
        }
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BapelkesWebVrAnc.Protocols{

    /// <summary>
    /// Class ini berfungsi untuk membuat Petunjuk bagi Player kemudian hilang ketika Player berada pada Area yang ditentukan
    /// </summary>

    public class DirectionProtocol : ProtocolManager
    {
        [SerializeField] private GameObject directionGameobject;
        [SerializeField] private string playerTag;

        void Start(){
            directionGameobject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other){
            if (other.transform.CompareTag(playerTag)){
                StopTheProtocol();
            }
        }

        //=====================================OVERRIDE METHODS============================================================
        
        public override void StartTheProtocol()
        {
            protocolStarted = true;
        }

        public override void StopTheProtocol()
        {
            protocolFinished = true;
            protocolStarted = false;
        }
    }

}

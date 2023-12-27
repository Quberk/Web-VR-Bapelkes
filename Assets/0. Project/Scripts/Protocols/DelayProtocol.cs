using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BapelkesWebVrAnc.Protocols{

    /// <summary>
    /// Class ini berfungsi untuk memberikan Delay
    /// Ketika waktu delay selesai maka Protocol Selesai
    /// </summary>
    /// 
    public class DelayProtocol : ProtocolManager
    {
        [SerializeField] private float delayTime;
        private float delayCounter = 0f;


        void Update()
        {
            if (!protocolStarted || protocolFinished)
                return;

            delayCounter += Time.deltaTime;

            if (delayCounter >= delayTime){
                StopTheProtocol();
            }   
        }

        //=====================================OVERRIDE METHODS============================================================
        
        public override void StartTheProtocol()
        {
            protocolStarted = true;
            protocolFinished = false;
        }

        public override void StopTheProtocol()
        {
            protocolFinished = true;
            protocolStarted = false;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BapelkesWebVrAnc.Protocols{

    ///<SUMMARY>
    /// Class ini sebagai Class Parent dari semua Protocol
    /// Class ini memuat 2 Komponen utama dari sebuah Protokol
    /// Yaitu Ketika Protokol Mulai dan Ketika Protokol Berakhir
    ///</SUMMARY>
    ///
    public abstract class ProtocolManager : MonoBehaviour
    {
        [HideInInspector] public bool protocolFinished = false;
        [HideInInspector] public bool protocolStarted = false;
        public abstract void StartTheProtocol();
        public abstract void StopTheProtocol();

        public bool IsProtocolFinished(){
            return protocolFinished;
        }

        public bool IsProtocolStarted(){
            return protocolStarted;
        }
    }

    [System.Serializable]
    public class Protocol{
        public string title;
        public ProtocolManager protocolManager;
        public ProtocolManager[] additionalProtocolManagers;
    }

    public interface IRetakingHandReference{
        void RetakingHandReference();
    }
}


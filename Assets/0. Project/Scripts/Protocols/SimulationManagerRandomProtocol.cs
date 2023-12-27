using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BapelkesWebVrAnc.Protocols{

    ///<SUMMARY>
    /// Class ini sebagai Manager utama dalam projek yang berfungsi untuk mengatur berjalannya Protokol-protokol
    /// MUlai dari Protokol pertama hingga terakhir
    ///</SUMMARY>
    ///
    public class SimulationManagerRandomProtocol : ProtocolManager
    {
        [SerializeField] private bool mainSimulationManager;

        private int stepsAmount;
        private int step = 1;

        [Header("Semua Protocol Manager adalah Class Installation yang merupakan turunan Protocol Manager")]
        [SerializeField] private List<Protocol> protocolManagers;
        private bool protocolInAction;

        void Start(){

            stepsAmount = protocolManagers.Count;

            if (mainSimulationManager)
                StartTheProtocol();
        }

        void Update(){

            //Simulasi berakhir jika sudah tidak ada lagi Step
            if (protocolManagers.Count == 0){
                StopTheProtocol();
                return;
            }
            
            if (!protocolStarted)
                return;

            //Jika ada Protocol sedang berjalan maka jangan diganggu
            if (protocolInAction){

                foreach(Protocol protocolManager in protocolManagers){
                    
                    //Mengecek apakah Protokol telah selesai jika ya maka kita bisa lanjut ke Protokol selanjutnya
                    if (protocolManager.protocolManager.IsProtocolFinished()){

                        //Mematikan juga semua Additional ProtocolManager ketika Protocol Utama telah selesai
                        for(int i = 0; i < protocolManager.additionalProtocolManagers.Length; i++){
                            protocolManager.additionalProtocolManagers[i].StopTheProtocol();
                        }

                        step++;

                        protocolManagers.Remove(protocolManager);

                        return;
                    }
                }

            }

        }

        private void ActivateAllProtocols(){

            foreach(Protocol protocol in protocolManagers){

                protocol.protocolManager.StartTheProtocol();

                //Mengaktifkan semua Additional Protocol sesuai dengan Step
                for(int i = 0; i < protocol.additionalProtocolManagers.Length; i++){
                    protocol.additionalProtocolManagers[i].StartTheProtocol();
                }

                protocolInAction = true;
            }

        }

        //=======================================GETTER METHOD==========================================================
        public int GetStep(){
            return step;
        }
        

        //=====================================OVERRIDE METHODS============================================================
        public override void StartTheProtocol()
        {
            ActivateAllProtocols();
            protocolStarted = true;
            Debug.Log("Simulation Manager Random Aktif.");
        }

        public override void StopTheProtocol()
        {
            protocolFinished = true;
            protocolStarted = false;
        }
}
}


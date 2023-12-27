using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BapelkesWebVrAnc.Protocols{

    ///<SUMMARY>
    /// Class ini sebagai Manager utama dalam projek yang berfungsi untuk mengatur berjalannya Protokol-protokol
    /// MUlai dari Protokol pertama hingga terakhir
    ///</SUMMARY>
    public class SimulationManagerProtocol : ProtocolManager
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
            
            if (!protocolStarted)
                return;

            //Simulasi berakhir jika sudah tidak ada lagi Step
            if (step > stepsAmount){
                StopTheProtocol();
                return;
            }

            //Jika ada Protocol sedang berjalan maka jangan diganggu
            if (protocolInAction){

                //Mengecek apakah Protokol telah selesai jika ya maka kita bisa lanjut ke Protokol selanjutnya
                if (protocolManagers[step - 1].protocolManager.IsProtocolFinished()){

                    //Mematikan juga semua Additional ProtocolManager ketika Protocol Utama telah selesai
                    for(int i = 0; i < protocolManagers[step - 1].additionalProtocolManagers.Length; i++){
                        protocolManagers[step - 1].additionalProtocolManagers[i].StopTheProtocol();
                    }

                    protocolInAction = false;
                    step++;
                }
                
                return;
            }

            //Mengaktifkan Protocol sesuai dengan step
            protocolManagers[step - 1].protocolManager.StartTheProtocol();

            //Mengaktifkan semua Additional Protocol sesuai dengan Step
            for(int i = 0; i < protocolManagers[step - 1].additionalProtocolManagers.Length; i++){
                protocolManagers[step - 1].additionalProtocolManagers[i].StartTheProtocol();
            }

            protocolInAction = true;

        }

        //=======================================GETTER METHOD==========================================================
        public int GetStep(){
            return step;
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


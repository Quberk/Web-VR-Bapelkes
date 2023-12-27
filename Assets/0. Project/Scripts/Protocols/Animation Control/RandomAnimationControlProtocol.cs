using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BapelkesWebVrAnc.AnimationControls;

namespace BapelkesWebVrAnc.Protocols.AnimationControls{

    /// <summary>
    /// Class ini berfungsi untuk memainkan Animasi Terteentu
    /// Lalu Protokol selesai ketika Animasi telah selesai
    /// JANGAN LUPA UNTUK MEMANGGIL ANIMATIONFINISHED() PADA FRAME AKHIR ANIMASI YANG DIMAKSUD
    /// </summary>
    /// 
    public class RandomAnimationControlProtocol : ProtocolManager
    {

        [SerializeField] private Animator targetAnimator;
        [SerializeField] private AnimationFinishedStatus animationFinishedStatus;
        [SerializeField] private string[] targetAnimationTrigger;

        private bool alreadyTriggered = false;

        void Update()
        {
            if (!protocolStarted || protocolFinished)
                return;

            if (animationFinishedStatus.GetAnimationFinishedStatus()){
                animationFinishedStatus.AnimationStarted();
                StopTheProtocol();
            }
            
            TriggeringAnimation();
            
        }

        void TriggeringAnimation(){

            if (!alreadyTriggered){

                alreadyTriggered = true;

                float randomNum = Random.Range(0, targetAnimationTrigger.Length * 100f);           

                for (int i = 0; i < targetAnimationTrigger.Length; i++){

                    if (randomNum <= (i + 1) * 100f){
                        targetAnimator.SetTrigger(targetAnimationTrigger[i]);
                        break;
                    }
                }

                return;
            }
        }

        public void AnimationFinished(){
            StopTheProtocol();
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


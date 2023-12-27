using System.Collections;
using System.Collections.Generic;
using BapelkesWebVrAnc.AnimationControls;
using UnityEngine;


namespace BapelkesWebVrAnc.Protocols.AnimationControls{

    /// <summary>
    /// Class ini berfungsi untuk memainkan Animasi Terteentu
    /// Lalu Protokol selesai ketika Animasi telah selesai
    /// JANGAN LUPA UNTUK MEMANGGIL ANIMATIONFINISHED() PADA FRAME AKHIR ANIMASI YANG DIMAKSUD
    /// </summary>
    /// 
    public class AnimationControlProtocol : ProtocolManager
    {

        [SerializeField] private Animator targetAnimator;
        [SerializeField] private AnimationFinishedStatus animationFinishedStatus;
        [SerializeField] private string targetAnimationTrigger;

        private bool alreadyTriggered = false;

        void Update()
        {
            if (!protocolStarted || protocolFinished)
                return;

            if (animationFinishedStatus.GetAnimationFinishedStatus()){
                animationFinishedStatus.AnimationStarted();
                StopTheProtocol();
            }
            
            if (!alreadyTriggered){
                alreadyTriggered = true;
                targetAnimator.SetTrigger(targetAnimationTrigger);
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


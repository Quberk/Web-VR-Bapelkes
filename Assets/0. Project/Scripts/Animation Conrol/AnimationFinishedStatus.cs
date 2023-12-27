using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BapelkesWebVrAnc.AnimationControls{
    public class AnimationFinishedStatus : MonoBehaviour
    {
        private bool animationFinished = false;

        public void AnimationStarted(){
            animationFinished = false;
        }

        public void AnimationFinished(){
            animationFinished = true;
        }
        public bool GetAnimationFinishedStatus(){
            return animationFinished;
        }
    }
}


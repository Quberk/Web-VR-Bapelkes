using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BapelkesWebVrAnc.Protocols.ActivateOrDeactivateProtocols{

    /// <summary>
    /// Class ini berfungsi untuk mengaktifkan atau menonaktifkan sebuah Gameobject secara acak ketika dijalankan
    /// </summary>
    /// 

    public class RandomlyActivateOrDeactivateProtocols : ActivateorDeactivateGameobjectProtocol
    {
        public override void ActivatingTargetObject()
        {
            float randomNum = Random.Range(0, targetObjects.Length * 100f);           

            for (int i = 0; i < targetObjects.Length; i++){

                if (randomNum <= (i + 1) * 100f){
                    targetObjects[i].SetActive(true);
                    break;
                }
            }
        }

        public override void DeactivatingTargetObject()
        {
            float randomNum = Random.Range(0, targetObjects.Length * 100f);           

            for (int i = 0; i < targetObjects.Length; i++){

                if (randomNum <= (i + 1) * 100f){
                    targetObjects[i].SetActive(false);
                    break;
                }
            }
        }
    }
}


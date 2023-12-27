using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BapelkesWebVrAnc.Generals{
    public class RandomActivation : MonoBehaviour
    {
        [SerializeField] private GameObject[] targetObjects;
        
        void Start(){

            for (int i = 0; i < targetObjects.Length; i++){
                targetObjects[i].SetActive(false);
            }
            
            float randomNum = Random.Range(0, targetObjects.Length * 100f);           

            for (int i = 0; i < targetObjects.Length; i++){

                if (randomNum <= (i + 1) * 100f){
                    targetObjects[i].SetActive(true);
                    Debug.Log(targetObjects[i].name + " Activated.");
                    break;
                }
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


namespace BapelkesWebVrAnc.Generals{
    public class FirebaseManager : MonoBehaviour
    {
        [SerializeField] private GameObject unpaidPanel;

        [DllImport("__Internal")]
        public static extern void GetJSON(string path, string objectName, string callback, string fallback);

        void Start(){
            GetJSON("Playable", gameObject.name, "OnRequestSuccess", "OnRequestFailed");
            unpaidPanel.SetActive(true);
        }

        private void OnRequestSuccess(string data){

            if (data == "true"){
                Debug.Log("Can Play...");
                unpaidPanel.SetActive(false);
            }

            else{
                Debug.Log("Cannot Play...");
                unpaidPanel.SetActive(true);
            }
        }

        private void OnRequestFailed(string data){
            Debug.Log("Error Happened : " + data);
            unpaidPanel.SetActive(true);
        }
    }
}


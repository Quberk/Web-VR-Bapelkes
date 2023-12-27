using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


namespace BapelkesWebVrAnc.Generals{
    public class LineToCertainObject : MonoBehaviour
    {
        [SerializeField] private LineRenderer line;
        [SerializeField] private Transform  pos1;
        [SerializeField] private Transform pos2;

        void Start()
        {
            line.positionCount = 2;
        }


        void Update()
        {
            line.SetPosition(0, pos1.position);
            line.SetPosition(1, pos2.position);
            /*
            for ( int i = 0; i < poses.Length; i++){

                line.SetPosition(i, poses[i].position);
   
            }*/
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BapelkesWebVrAnc.Generals{


    public class FollowTransform : MonoBehaviour
    {
        [SerializeField] private GameObject targetObject;
        [SerializeField] private GameObject followedObject;

        [SerializeField] private bool followPosition;
        [SerializeField] private bool followRotation;
        [SerializeField] private bool followScale;

        // Update is called once per frame
        void Update()
        {
            if (followRotation){
                targetObject.transform.localRotation = Quaternion.Euler(0f,
                                                                followedObject.transform.localRotation.eulerAngles.y,
                                                                0f);
            }

            if (followPosition){
                targetObject.transform.localPosition = new Vector3(followedObject.transform.localPosition.x,
                                                                followedObject.transform.localPosition.y,
                                                                followedObject.transform.localPosition.z);
            }
        }
    }
}


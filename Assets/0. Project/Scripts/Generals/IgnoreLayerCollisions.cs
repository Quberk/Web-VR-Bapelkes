using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BapelkesWebVrAnc.Generals{

    public class IgnoreLayerCollisions : MonoBehaviour
    {
        [SerializeField] private LayerCollision[] layerCollisions;

        // Start is called before the first frame update
        void Start()
        {
            foreach(LayerCollision layerCollision in layerCollisions){

                Physics.IgnoreLayerCollision(layerCollision.firstLayerIndex, layerCollision.secondLayerIndex);
            }
        }
    }
    

    [System.Serializable]
    public class LayerCollision{
        public int firstLayerIndex;
        public int secondLayerIndex;
    }

}

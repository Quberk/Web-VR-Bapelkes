using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ExampleUtil : MonoBehaviour
{
    public static bool isPresent()
        {
            var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
            SubsystemManager.GetInstances<XRDisplaySubsystem>(xrDisplaySubsystems);
            foreach (var xrDisplay in xrDisplaySubsystems)
            {
                if (xrDisplay.running)
                {
                    return true;
                }
            }
            return false;
        }
}

public class CheckXRDisplay : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Do we have an Active Display? " + ExampleUtil.isPresent().ToString());
    }
}
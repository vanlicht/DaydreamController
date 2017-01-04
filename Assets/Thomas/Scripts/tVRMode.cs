using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class tVRMode : MonoBehaviour
{
    bool VRState = true;
	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void tVRToggle()
    {
        VRState = !VRState;
        if(VRState == false)
        {
            VRSettings.enabled = false;
        }
        
    }
}

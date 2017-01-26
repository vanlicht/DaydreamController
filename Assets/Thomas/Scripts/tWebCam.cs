using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class tWebCam : MonoBehaviour
{
    public Text textPanel;
    WebCamTexture mCamera = null;

    public GameObject camPlane;
	// Use this for initialization
	void Start ()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        //camPlane = GameObject.FindWithTag("Player");
        
        if (devices.Length > 0)
        {
            mCamera = new WebCamTexture(devices[devices.Length-1].name, 1920, 1920, 30);
            camPlane.GetComponent<Renderer>().material.mainTexture = mCamera;

            mCamera.deviceName = devices[devices.Length - 1].name;
            Debug.Log(devices.Length);
            Debug.Log(mCamera.deviceName);
            mCamera.Play();
            textPanel.text = "# of devices: " + devices.Length.ToString() + "; Device 1: " + mCamera.deviceName;
        }
        else
        {
            textPanel.text = "No device detected...";
        }
            

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

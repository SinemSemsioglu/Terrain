using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit.Examples;

public class CameraSelector : MonoBehaviour {
    public Camera perspectiveCamera;
    public Camera topCamera;
    public Camera staticTopCamera;

    public Camera sideCamera;
    public GameObject vaOverlay;

    public GameObject topRotator;
    public GameObject sideRotator;
    public GameObject perspectiveRotator;

    Camera currentCamera;
	// Use this for initialization
	void Start () {
        currentCamera = perspectiveCamera;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Camera getCurrentCamera() {
        return currentCamera;
    }

    public void deselectAll() {
        currentCamera = null;

        staticTopCamera.enabled = false;
        topCamera.enabled = false;
        perspectiveCamera.enabled = false;
        sideCamera.enabled = false;
        vaOverlay.SetActive(false);

        topRotator.SetActive(false);
        sideRotator.SetActive(false);
        perspectiveRotator.SetActive(false);
    }


    public void selectPerspective()
    {
        currentCamera = perspectiveCamera;

        staticTopCamera.enabled = false;
        topCamera.enabled = false;
        perspectiveCamera.enabled = true;
        sideCamera.enabled = false;
        vaOverlay.SetActive(false);

        topRotator.SetActive(false);
        sideRotator.SetActive(false);
        perspectiveRotator.SetActive(true);
    }

     public void selectSide()
    {
        currentCamera = sideCamera;

        staticTopCamera.enabled = false;
        topCamera.enabled = false;
        perspectiveCamera.enabled = false;
        sideCamera.enabled = true;
        vaOverlay.SetActive(false);

        topRotator.SetActive(false);
        sideRotator.SetActive(true);
        perspectiveRotator.SetActive(false);
    }

    public void selectTop()
    {
        currentCamera = topCamera;

        staticTopCamera.enabled = false;
        topCamera.enabled = true;
        perspectiveCamera.enabled = false;
        sideCamera.enabled = false;
        vaOverlay.SetActive(false);

        topRotator.SetActive(true);
        sideRotator.SetActive(false);
        perspectiveRotator.SetActive(false);
    }

    public void selectVa()
    {
        currentCamera = topCamera;
        
        staticTopCamera.enabled = true;
        topCamera.enabled = false;
        perspectiveCamera.enabled = false;
        sideCamera.enabled = false;
        vaOverlay.SetActive(true);

        topRotator.SetActive(false);
        sideRotator.SetActive(false);
        perspectiveRotator.SetActive(false);
    }
}

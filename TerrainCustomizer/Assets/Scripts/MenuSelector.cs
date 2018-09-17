using System.Collections;
using System.Collections.Generic;
using ProceduralToolkit.Examples;
using UnityEngine;

public class MenuSelector : MonoBehaviour {
	public GameObject terrain;
	public GameObject light;
	public GameObject path;

    public GameObject customizerDefaultMenus;

    public GameObject mainOverlay;
    public GameObject mainSelection;
    public GameObject inputPanel;
    public GameObject loadButton;
    public GameObject uploadButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void customizerHide() {
        customizerDefaultMenus.SetActive(false);

        light.SetActive(false);
        terrain.SetActive(false);
        path.SetActive(false);
    }

    public void selectMain()
    {
        customizerHide();

        mainOverlay.SetActive(true);
        mainSelection.SetActive(true);
        inputPanel.SetActive(false);
    }

    public void selectUpload() {
        customizerHide();

        mainOverlay.SetActive(true);
        mainSelection.SetActive(false);
        inputPanel.SetActive(true);
        uploadButton.SetActive(true);
        loadButton.SetActive(false);
    }

    public void selectLoad() {
        customizerHide();

        mainOverlay.SetActive(true);
        mainSelection.SetActive(false);
        inputPanel.SetActive(true);
        uploadButton.SetActive(false);
        loadButton.SetActive(true);
    }

    public void loadCustomizer() {
        mainOverlay.SetActive(false);
        customizerDefaultMenus.SetActive(true);
        selectTerrain();
    }

	public void selectTerrain()
    {
        light.SetActive(false);
        terrain.SetActive(true);
        path.SetActive(false);
    }

    public void selectLight()
    {
        light.SetActive(true);
        terrain.SetActive(false);
        path.SetActive(false);
    }

    public void selectPath()
    {
        light.SetActive(false);
        terrain.SetActive(false);
        path.SetActive(true);
    }
}

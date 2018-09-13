using System.Collections;
using System.Collections.Generic;
using ProceduralToolkit.Examples;
using UnityEngine;

public class MenuSelector : MonoBehaviour {
	public GameObject terrain;
	public GameObject light;
	public GameObject path;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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

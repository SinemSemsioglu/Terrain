using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using ProceduralToolkit.Examples;
using UnityEngine.Networking;

public class GetConfig : MonoBehaviour {
	public LowPolyTerrainGeneratorConfigurator terrain;
	public LightPanel lights;
	public LightPlacer path;
	
    public InputField nameInput;
    public InputField passInput;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void loadTerrain() {
		Debug.Log(nameInput.text + " " + passInput.text);
        StartCoroutine(getConfig(nameInput.text, passInput.text));
    }

    IEnumerator getConfig(string terrainName, string terrainPass) {
        WWWForm form = new WWWForm();
        form.AddField("terrainName", terrainName);
        form.AddField("terrainPass", terrainPass);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:5000/getConfig", form);
		yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("PUT successful!");

            // Print Body
            Debug.Log(www.downloadHandler.text);
			// TODO check success true
			setConfig(www.downloadHandler.text);
        }
    }

	public void setConfig(string configStr) {
		ConfigurationCollector.ServerConfig config = JsonUtility.FromJson<ConfigurationCollector.ServerConfig>(configStr);
 		
		// Set LowPolyTerrainGenerator - Config
		LowPolyTerrainGenerator.Config terrainConfig = new LowPolyTerrainGenerator.Config();
		
		// terrain: cell size, noise scale
		terrainConfig.cellSize = config.cellSize;
		terrainConfig.noiseScale = config.noiseScale;
		// terrain, va: height vs. valence arousal, noise vs. valence arousal
		terrainConfig.heightValenceDirect = config.heightRelations[0];
		terrainConfig.heightValenceInverse = config.heightRelations[1];
		terrainConfig.heightArousalDirect = config.heightRelations[2];
		terrainConfig.heightArousalInverse = config.heightRelations[3];

		terrainConfig.noiseValenceDirect = config.noiseRelations[0];
		terrainConfig.noiseValenceInverse = config.noiseRelations[1];
		terrainConfig.noiseArousalDirect = config.noiseRelations[2];
		terrainConfig.noiseArousalInverse = config.noiseRelations[3];

		terrain.setConfig(terrainConfig);
		terrain.Generate();

		// Set Light Properties
		// colors: skybox atmosphere thickness, sea material color, light colors
        lights.setAtmosphereThickness(config.atmosphereThickness);
        lights.setSeaColor(config.seaColor);
		lights.setLightColors(config.lightHSV);

		// TODO implement path + interpolation stuff
		// path: object positions (x,y, (z)) coordinates -- z should be fitted somehow
	}

	
}

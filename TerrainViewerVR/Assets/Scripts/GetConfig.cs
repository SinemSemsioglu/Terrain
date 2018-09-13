using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using ProceduralToolkit.Examples;
using UnityEngine.Networking;

public class GetConfig : MonoBehaviour {
	public LowPolyTerrainGeneratorConfigurator terrain;
	public Material skyboxMat;
	public Material seaMat;

	 // to be set as 9 lights in the scene, 0: low arousal neutral valence, going counter clockwise for the next 7 and ending with neutral.
    public Light[] lights = new Light[9];
	// neutral
	//string configStr = "{\"cellSize\":5.0,\"noiseScale\":5.0,\"atmosphereThickness\":0.0,\"seaColor\":{\"x\":0.0,\"y\":0.0,\"z\":0.0},\"stoneLocations\":[],\"lightHSV\":[{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176},{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176},{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176},{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176},{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176},{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176},{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176},{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176},{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176}],\"heightRelations\":[false,false,false,false],\"noiseRelations\":[false,false,false,false]}";
	string configStr = "{\"cellSize\":3.0,\"noiseScale\":7.0,\"atmosphereThickness\":0.9590736627578735,\"seaColor\":{\"x\":0.0,\"y\":0.0,\"z\":0.0},\"stoneLocations\":[],\"lightHSV\":[{\"x\":0.677966296672821,\"y\":0.19999998807907105,\"z\":0.5},{\"x\":0.0,\"y\":0.3999999761581421,\"z\":0.3500000238418579},{\"x\":0.45762714743614199,\"y\":0.6000000238418579,\"z\":0.20000000298023225},{\"x\":0.23728792369365693,\"y\":0.800000011920929,\"z\":0.3500000238418579},{\"x\":0.16949157416820527,\"y\":1.0,\"z\":0.5},{\"x\":0.16949154436588288,\"y\":0.800000011920929,\"z\":0.6499999761581421},{\"x\":0.0,\"y\":0.6000000238418579,\"z\":0.7999999523162842},{\"x\":0.8305084109306336,\"y\":0.3999999463558197,\"z\":0.6499999761581421},{\"x\":0.45762714743614199,\"y\":0.6000000834465027,\"z\":0.5}],\"heightRelations\":[false,false,true,false],\"noiseRelations\":[false,true,false,false]}";
	
	// Use this for initialization
	void Start () {
		getConfig();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void getConfig() {
		//TODO get config via HTTP request
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
		// colors: skybox atmosphere thickness, sea material color
		skyboxMat.SetFloat("_AtmosphereThickness", config.atmosphereThickness);
		seaMat.SetColor("_EmissionColor", Color.HSVToRGB(config.seaColor.x, config.seaColor.y, config.seaColor.z));
		// colors, va: saturation vs. valence arousal, brightness vs. valence arousal
		// colors, regions: hue values for 9 lights (or combine with prev step and get hsv or rgb)
		setLightColors(config.lightHSV);

		// TODO implement path + interpolation stuff
		// path: object positions (x,y, (z)) coordinates -- z should be fitted somehow

	}

	private void setLightColors(Vector3[] lightHSV) {
		if (lights.Length != lightHSV.Length) {
			Debug.Log("light numbers do not match");
		} else {
			for (int i = 0; i <lights.Length; i++) {
				Vector3 col = lightHSV[i];
				lights[i].color = Color.HSVToRGB(col.x, col.y, col.z);
			}
		}
	}
}

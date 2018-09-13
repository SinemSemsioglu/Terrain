using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using ProceduralToolkit.Examples;
using UnityEngine.Networking;

public class ConfigurationCollector : MonoBehaviour {
	  [Serializable]
        public class ServerConfig
        {
            public float cellSize;
			public float noiseScale;
			public float atmosphereThickness;
			public Vector3 seaColor;
			public Vector3[] stoneLocations;
			public Vector3[] lightHSV; // or maybe even rgb
			public bool[] heightRelations; // 0: v direct, v Inverse, a direct, a Inverse
			public bool[] noiseRelations; // 0: v direct, v Inverse, a direct, a Inverse
        }

	public LowPolyTerrainGeneratorConfigurator terrain;
	public LightPanel lights;
	public LightPlacer path;

	ServerConfig config = new ServerConfig();
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void getConfig() {
		// Get from LowPolyTerrainGenerator - Config
		LowPolyTerrainGenerator.Config terrainConfig = terrain.getConfig();
		// terrain: cell size, noise scale
		config.cellSize = terrainConfig.cellSize;
		config.noiseScale = terrainConfig.noiseScale;
		// terrain, va: height vs. valence arousal, noise vs. valence arousal
		config.heightRelations = new bool[4];
		config.noiseRelations = new bool[4];
		config.heightRelations[0] = terrainConfig.heightValenceDirect;
		config.heightRelations[1] = terrainConfig.heightValenceInverse;
		config.heightRelations[2] = terrainConfig.heightArousalDirect;
		config.heightRelations[3] = terrainConfig.heightArousalInverse;

		config.noiseRelations[0] = terrainConfig.noiseValenceDirect;
		config.noiseRelations[1] = terrainConfig.noiseValenceInverse;
		config.noiseRelations[2] = terrainConfig.noiseArousalDirect;
		config.noiseRelations[3] = terrainConfig.noiseArousalInverse;

		// Get from LightPanel
		// colors: skybox atmosphere thickness, sea material color
		config.atmosphereThickness = lights.getAtmosphereThickness();
		config.seaColor = lights.getSeaColor();
		// colors, va: saturation vs. valence arousal, brightness vs. valence arousal
		// colors, regions: hue values for 9 lights (or combine with prev step and get hsv or rgb)
		config.lightHSV = lights.getLightColors();

		// Get from LightPlacer
		// path: object positions (x,y, (z)) coordinates -- z should be fitted somehow

		string json = JsonUtility.ToJson(config);
		Debug.Log(json);
		//upload(json);
		//return config;
	}

	public void upload(string data) {
		UnityWebRequest www = UnityWebRequest.Put("http://localhost:5000/uploadConfig", data);
		www.SetRequestHeader("Content-Type", "application/json");
		www.Send();
	}
}

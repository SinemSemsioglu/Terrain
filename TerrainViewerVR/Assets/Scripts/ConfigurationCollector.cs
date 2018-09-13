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
            public int cellSize;
			public float noiseScale;
			public float atmosphereThickness;
			public Vector3 seaColor;
			public Vector3[] stoneLocations;
			public Vector3[] lightHSV; // or maybe even rgb
			public bool[] heightRelations; // 0: v direct, v Inverse, a direct, a Inverse
			public bool[] noiseRelations; // 0: v direct, v Inverse, a direct, a Inverse
        }
}

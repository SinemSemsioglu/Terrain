using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Valve.VR.InteractionSystem;

using UnityEngine.Assertions;
using ProceduralToolkit.Examples;
using UnityEngine.Networking;

public class GetConfig : MonoBehaviour {

	public GameObject terrainObjects;
	public GameObject selectionObjects;
	public GameObject emotionSelection;

	public class TerrainNames
        {
            public string[] names;
        }

	public LowPolyTerrainGeneratorConfigurator terrain;
	public Material skyboxMat;
	public Material seaMat;

	public GameObject buttonPrefab;
	public GameObject canvas;

	 // to be set as 9 lights in the scene, 0: low arousal neutral valence, going counter clockwise for the next 7 and ending with neutral.
    public Light[] lights = new Light[9];
	public GameObject[] lightObjs = new GameObject[9];


	string configStr;
	// neutral
	//string configStr = "{\"cellSize\":5.0,\"noiseScale\":5.0,\"atmosphereThickness\":0.0,\"seaColor\":{\"x\":0.0,\"y\":0.0,\"z\":0.0},\"stoneLocations\":[],\"lightHSV\":[{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176},{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176},{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176},{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176},{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176},{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176},{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176},{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176},{\"x\":0.5,\"y\":0.5,\"z\":0.501960813999176}],\"heightRelations\":[false,false,false,false],\"noiseRelations\":[false,false,false,false]}";
	//string configStr = "{\"cellSize\":3.0,\"noiseScale\":7.0,\"atmosphereThickness\":0.9590736627578735,\"seaColor\":{\"x\":0.0,\"y\":0.0,\"z\":0.0},\"stoneLocations\":[],\"lightHSV\":[{\"x\":0.677966296672821,\"y\":0.19999998807907105,\"z\":0.5},{\"x\":0.0,\"y\":0.3999999761581421,\"z\":0.3500000238418579},{\"x\":0.45762714743614199,\"y\":0.6000000238418579,\"z\":0.20000000298023225},{\"x\":0.23728792369365693,\"y\":0.800000011920929,\"z\":0.3500000238418579},{\"x\":0.16949157416820527,\"y\":1.0,\"z\":0.5},{\"x\":0.16949154436588288,\"y\":0.800000011920929,\"z\":0.6499999761581421},{\"x\":0.0,\"y\":0.6000000238418579,\"z\":0.7999999523162842},{\"x\":0.8305084109306336,\"y\":0.3999999463558197,\"z\":0.6499999761581421},{\"x\":0.45762714743614199,\"y\":0.6000000834465027,\"z\":0.5}],\"heightRelations\":[false,false,true,false],\"noiseRelations\":[false,true,false,false]}";
	
	Vector3 destinationPoint;
	bool goingDown = false;
	
	public GameObject player;
	/*public Toggle lalv;
	public Toggle hahv;
	public Toggle halv;
	public Toggle lahv;*/
	public QuadrantSelectorGroup selectorGroup;

	// Use this for initialization
	void Start () {
        StartCoroutine(getAllTerrainNames());
	}
	
	// Update is called once per frame
	void Update () {
		if (goingDown) {
			if( player.transform.position.y > destinationPoint.y + 0.2f) {
				Vector3 playerPos = player.transform.position;
				playerPos.y -= 0.05f;

				player.transform.position = playerPos;
			} else {
				goingDown = false;
			}
		}
	}


 	public void loadTerrain(string terrainName) {
		Debug.Log("load terrain called with name " + terrainName);
        StartCoroutine(getConfig(terrainName));
    }

 	IEnumerator getAllTerrainNames() {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/getAllTerrainNames");
		Debug.Log("request sent");
		yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("get successful!");

            // Print Body
            Debug.Log(www.downloadHandler.text);
			// TODO check success true
			TerrainNames tNames = JsonUtility.FromJson<TerrainNames>(www.downloadHandler.text);

			for (int i=0; i<tNames.names.Length; i++) {
				GameObject button = Instantiate(buttonPrefab);
				button.GetComponentInChildren<TextMesh>().text = tNames.names[i];
				int ind = i;

				button.GetComponent<LoadTerrainButton>().configurator = this;

				//Interactable intr = button.GetComponent<Interactable>();
				//intr.onAttachedToHand.AddListener(() => {
				//	Debug.Log("click");
				//});
				/*UnityEvent bEvent = new UnityEvent();
				bEvent.AddListener(() => {
					string bName = tNames.names[ind];
					loadTerrain(bName);
				});

				InteractableButtonEvents evs = button.GetComponent<InteractableButtonEvents>();
				if (evs == null) {
					Debug.Log("couldn't access interactable button events script");
				} else {
					if(evs.onTriggerDown == null) {
						Debug.Log("on trigger down is null");
						evs.onTriggerDown = new UnityEvent();
					}

					evs.onTriggerDown.AddListener(() => {
						string bName = tNames.names[ind];
						loadTerrain(bName);
					});
				} */

				button.transform.parent = canvas.transform;
				button.transform.localPosition = new Vector3(0, i * 0.3f, 0);
			}
			
        }
    }

    IEnumerator getConfig(string terrainName) {
        WWWForm form = new WWWForm();
        form.AddField("terrainName", terrainName);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:5000/getTerrainView", form);
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
			emotionSelection.SetActive(true);
			canvas.SetActive(false);
			configStr = www.downloadHandler.text;
        }
    }

	public void setConfig() {
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
		setLightHeights(config.heightRelations);

		// TODO implement path + interpolation stuff
		// path: object positions (x,y, (z)) coordinates -- z should be fitted somehow

		

	}

	public void enterTerrain() {
		
		Vector3 position = new Vector3(0, 80, 0);
		int q = selectorGroup.currentQuadrant;
		if(q == 3) {
			// low arousal low valence
			position.x = -50;
			position.z = -50;
		} else if (q == 2) {
			position.x = 50;
			position.z = -50;
		} else if (q == 1) {
			position.x = 50;
			position.z = 50;
		} else if (q == 4) {
			position.x =-50;
			position.z = 50; 
		}
		Debug.Log(position);
		player.transform.position = position;


		terrainObjects.SetActive(true);
		setConfig();
		selectionObjects.SetActive(false);

		Ray ray = new Ray(position, Vector3.down);
		RaycastHit hitInfo;

		if (Physics.Raycast(ray, out hitInfo))
		{
			/*if (hitInfo.collider != null && (hitInfo.collider.GetType() == typeof(MeshCollider)))
			{*/
				Debug.Log(hitInfo.point);
				destinationPoint = hitInfo.point;
				goingDown = true;
			/*}*/

		} else {
			Debug.Log("rayacst not hit");
		}

	}

	private void setLightHeights(bool[] heightRelations) {
		//TODO make this function more modular
		if (heightRelations[0]) {
			Debug.Log("h v direct");
			// height valence direct
			lightObjs[4].transform.Translate(0,18,0, Space.World);
			lightObjs[3].transform.Translate(0,14,0, Space.World);
			lightObjs[5].transform.Translate(0,14,0, Space.World);
			lightObjs[6].transform.Translate(0,10,0, Space.World);
			lightObjs[8].transform.Translate(0,10,0, Space.World);
			lightObjs[2].transform.Translate(0,10,0, Space.World);
			lightObjs[7].transform.Translate(0,6,0, Space.World);
			lightObjs[1].transform.Translate(0,6,0, Space.World);
			lightObjs[0].transform.Translate(0,2,0, Space.World);
		} else if( heightRelations[0]) {
			Debug.Log("h v indirect");

			// height valence indirect
			lightObjs[4].transform.Translate(0,2,0, Space.World);
			lightObjs[3].transform.Translate(0,6,0, Space.World);
			lightObjs[5].transform.Translate(0,6,0, Space.World);
			lightObjs[6].transform.Translate(0,10,0, Space.World);
			lightObjs[8].transform.Translate(0,10,0, Space.World);
			lightObjs[2].transform.Translate(0,10,0, Space.World);
			lightObjs[7].transform.Translate(0,14,0, Space.World);
			lightObjs[1].transform.Translate(0,14,0, Space.World);
			lightObjs[0].transform.Translate(0,18,0, Space.World);
		} else if(heightRelations[2]) {
			Debug.Log("h a direct");

			// height arousal direct
			lightObjs[6].transform.Translate(0,18,0, Space.World);
			lightObjs[5].transform.Translate(0,14,0, Space.World);
			lightObjs[7].transform.Translate(0,14,0, Space.World);
			lightObjs[0].transform.Translate(0,10,0, Space.World);
			lightObjs[8].transform.Translate(0,10,0, Space.World);
			lightObjs[4].transform.Translate(0,10,0, Space.World);
			lightObjs[1].transform.Translate(0,6,0, Space.World);
			lightObjs[3].transform.Translate(0,6,0, Space.World);
			lightObjs[2].transform.Translate(0,2,0, Space.World);
		} else if(heightRelations[3]) {
			Debug.Log("h a indirect");

			// height arousal indirect
			lightObjs[6].transform.Translate(0,2,0, Space.World);
			lightObjs[5].transform.Translate(0,6,0, Space.World);
			lightObjs[7].transform.Translate(0,6,0, Space.World);
			lightObjs[0].transform.Translate(0,10,0, Space.World);
			lightObjs[8].transform.Translate(0,10,0, Space.World);
			lightObjs[4].transform.Translate(0,10,0, Space.World);
			lightObjs[1].transform.Translate(0,14,0, Space.World);
			lightObjs[3].transform.Translate(0,14,0, Space.World);
			lightObjs[2].transform.Translate(0,18,0, Space.World);
		}
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

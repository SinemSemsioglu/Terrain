using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit.Examples.Primitives;

public class LightPlacer : MonoBehaviour {
    public GameObject light;
    public CameraSelector camManager;

    Dictionary<string, Type> scriptTypes;
    List<GameObject> t1;
    List<GameObject> t2;
    List<GameObject> t3;
    List<GameObject> t4;

    Type[] scripts;

	// Use this for initialization
	void Start () {
        t1 = new List<GameObject>();
        t2 = new List<GameObject>();
        t3 = new List<GameObject>();
        t4 = new List<GameObject>();
        
        scriptTypes = new Dictionary<string,Type>();
       
        scriptTypes.Add("Dodecahedron", typeof(Dodecahedron));
        scriptTypes.Add("Icosahedron", typeof(Icosahedron));
        scriptTypes.Add("Pyramid", typeof(Pyramid));
        scriptTypes.Add("FlatSphere", typeof(FlatSphere));
        scriptTypes.Add("Sphere", typeof(Sphere));
        scriptTypes.Add("Tetrahedron", typeof(Tetrahedron));
        scriptTypes.Add("Octahedron", typeof(Octahedron));
       
       scripts = new Type[4];
       
       for (int i=0; i <4; i++) {
           scripts[i] = typeof(Dodecahedron);
       }
    }

    public void setQuadrantObject(int quadrant, string objName) {
        Type oldType = scripts[quadrant-1];
        Type newType = scriptTypes[objName];
        scripts[quadrant-1] = newType;

        List<GameObject> toBeRefreshed = t1;
        switch(quadrant) {
            case 1: toBeRefreshed = t1; break;
            case 2: toBeRefreshed = t2; break;
            case 3: toBeRefreshed = t3; break;
            case 4: toBeRefreshed = t4; break;
        }

        refreshObjects(toBeRefreshed, oldType, newType);
    }
	
    private void refreshObjects(List<GameObject> list, Type oldT, Type newT) {
        foreach(GameObject elm in list) {
            Destroy(elm.GetComponent(oldT));
            elm.AddComponent(newT);
        }
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
            //Debug.Log("down");

        if (Input.GetButton("Fire1"))
        {
            //Debug.Log("regular");
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            Camera currentCam = camManager.getCurrentCamera();
            //Debug.Log("up");
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Ray ray = currentCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 1000))
            {
                if (hitInfo.collider != null && (hitInfo.collider.GetType() == typeof(MeshCollider)))
                {
                    GameObject l = Instantiate(light, hitInfo.point, light.transform.rotation);

                    string terrainName = hitInfo.transform.gameObject.name;
                    
                    switch(terrainName) {
                        case "TerrainRendererQ1": l.AddComponent(scripts[0]); t1.Add(l); break;
                        case "TerrainRendererQ2": l.AddComponent(scripts[1]); t2.Add(l); break;
                        case "TerrainRendererQ3": l.AddComponent(scripts[2]); t3.Add(l); break;
                        case "TerrainRendererQ4": l.AddComponent(scripts[3]); t4.Add(l); break;
                    };
                }

            }
            
        }

         if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Debug.Log("down key press detected");
            Camera currentCam = camManager.getCurrentCamera();
            //Debug.Log("up");
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Ray ray = currentCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 1000))
            {
               
                //Debug.Log("collided with sphere collider");
                GameObject hitObj = hitInfo.transform.gameObject;
                //Debug.Log(hitObj.name);
                if (hitObj.GetComponent<Dodecahedron>() != null ) {
                    //Debug.Log("dodecahedron found");
                    DestroyImmediate(hitObj);
                }
            
            }
            
        }
    }
}

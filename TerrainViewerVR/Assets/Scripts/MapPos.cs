using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MapPos : MonoBehaviour {
	public Player player;
	public GameObject camera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 playerPos = player.transform.position;
		playerPos.y = 0;
		transform.localPosition = playerPos * -0.03f;
		transform.localEulerAngles = new Vector3(0, camera.transform.eulerAngles.y + 180,0);
	}
}

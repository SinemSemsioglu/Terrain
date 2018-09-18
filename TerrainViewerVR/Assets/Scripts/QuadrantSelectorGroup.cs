using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadrantSelectorGroup : MonoBehaviour {
	public QuadrantSelector[] selectors;
	public int currentQuadrant = 1;
	// Use this for initialization
	void Start () {
		//selectors = new QuadrantSelector[4];
		Debug.Log("initial size " + selectors.Length);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void quadrantSelected(int q) {
		currentQuadrant = q;
		Debug.Log("size " + selectors.Length );
		for (int i=0; i < selectors.Length; i++) {
			Debug.Log(i);
			QuadrantSelector selector = selectors[i];
			if (selector.quadrant != q) {
				Debug.Log("not selected " + selector.quadrant);
				selector.isSelected = false;
			} else {
				Debug.Log("selected " + selector.quadrant);

				selector.isSelected = true;
			}
		}

		//Debug.Log(currentQuadrant);
	}
}

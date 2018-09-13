using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ImageSlider : MonoBehaviour {
	public Sprite[] imageArray;
	int currentImage;
	Image image;
	public LightPlacer lightPlacer;
	public int quadrant;
	//Rect buttonRect;
	
	void Start()
	{
		image = GetComponent<Image>();
		currentImage = 0;
		image.sprite = imageArray[currentImage];
		//imageRect = new Rect(Screen.width-600, Screen.height-300, 100, 100);
		//buttonRect = new Rect(Screen.width-600, Screen.height - 300, 100, 20);
	}
	
	public void OnNextClick()
	{
	
		currentImage ++;

		if(currentImage >= imageArray.Length)
			currentImage = 0;

		image.sprite = imageArray[currentImage];
	}

	public void onImageClick() {
		lightPlacer.setQuadrantObject(quadrant, imageArray[currentImage].name);
	}
}

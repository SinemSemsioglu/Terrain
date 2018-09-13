using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit.Examples.UI;
using UnityEngine.UI;

namespace ProceduralToolkit.Examples
{
	public class PathPanel : ConfiguratorBase {

		public RectTransform leftPanel;
		public RectTransform rightPanel;
		
		public LightPlacer lightPlacer;


		// Use this for initialization
		void Start () {
			lightPlacer.enabled = true;

			InstantiateControl<TextControl>(leftPanel)
                .Initialize("On this menu you can place objects on the terrain to define paths (paths will be automatically created in between the points)");
			
			InstantiateControl<TextControl>(leftPanel)
                .Initialize("To place an object you can press the UP arrow while your mouse is on the intended position");
			
			InstantiateControl<TextControl>(leftPanel)
                .Initialize("And to delete one, you can press the DOWN arrow, while your mouse is on the intended object");

			InstantiateControl<TextControl>(leftPanel)
                .Initialize("You can select different object for each quadrant (from the right-side panel).");

		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}
//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Demonstrates how to create a simple interactable object
//
//=============================================================================

using UnityEngine;
using System.Collections;

using Valve.VR.InteractionSystem;

	//-------------------------------------------------------------------------
	[RequireComponent( typeof( Interactable ) )]
	public class QuadrantSelector : MonoBehaviour
	{
		public bool isSelected = false;
		public int quadrant = 1;
		public QuadrantSelectorGroup group;

		private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & ( ~Hand.AttachmentFlags.SnapOnAttach ) & ( ~Hand.AttachmentFlags.DetachOthers );

		//-------------------------------------------------
		void Awake()
		{
			//textMesh = GetComponentInChildren<TextMesh>();
			//textMesh.text = "No Hand Hovering";
		}

		void Update() {
			if (isSelected) {
				GetComponent<Renderer>().material.color = Color.red;
			} else {
				GetComponent<Renderer>().material.color = Color.white;
			}
		}

		//-------------------------------------------------
		// Called when a Hand starts hovering over this object
		//-------------------------------------------------
		private void OnHandHoverBegin( Hand hand )
		{
			//textMesh.text = "Hovering hand: " + hand.name;
		}


		//-------------------------------------------------
		// Called when a Hand stops hovering over this object
		//-------------------------------------------------
		private void OnHandHoverEnd( Hand hand )
		{
			//textMesh.text = "No Hand Hovering";
		}


		//-------------------------------------------------
		// Called every Update() while a Hand is hovering over this object
		//-------------------------------------------------
		private void HandHoverUpdate( Hand hand )
		{
			if ( hand.GetStandardInteractionButtonDown() || ( ( hand.controller != null ) && hand.controller.GetPressDown( Valve.VR.EVRButtonId.k_EButton_Grip ) ) )
			{
                //textMesh.text = "Trigerred";
				//isSelected = true;
				group.quadrantSelected(quadrant);
			}
		}

	}


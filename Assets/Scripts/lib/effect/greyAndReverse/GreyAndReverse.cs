using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using xy3d.tstd.lib.publicTools;
using System.Collections.Generic;

public class GreyAndReverse : MonoBehaviour
{
    [SerializeField]
    public Material externMat;

	Material ControlMat;

	List<Text> texts = new List<Text> ();

	List<Shadow> shadows = new List<Shadow> ();

	List<Color> oldTextColors = new List<Color> ();

	List<Color> oldShadowColors = new List<Color> ();

	[SerializeField]
	Color
		greyShadowColor = new Color (0, 0, 0, 0.5f);

	[SerializeField]
	Color
		greyTextColor = new Color (1f, 1f, 1f, 0.75f);

	[HideInInspector]
	public bool
		isGrey;

    [SerializeField]
    public bool isMask2;

	void Awake () {

		CreatGreyMaterial ();
		 
		SetBtnImage ();

		SetChildrenImage ();

		GetTextAndColor ();

		GetShadowAndColor ();
	}

	//0变回正常的颜色  1变灰
	public void SetGrey () {

		ControlMat.SetFloat ("_ShowGrayState", 1f);

		foreach (Shadow shadow in shadows) {

			shadow.effectColor = greyShadowColor;
		}

		foreach (Text text in texts) {

			text.color = greyTextColor;
		}

		isGrey = true;
	}

	public void ReverseColor () {

		ControlMat.SetFloat ("_ShowGrayState", 0f);

		for (int i = 0; i <shadows.Count; i++) {

			shadows [i].effectColor = oldShadowColors [i];
		}
	
		for (int i = 0; i <texts.Count; i++) {
			
			texts [i].color = oldTextColors [i];
		}

		isGrey = false;

	}

	void CreatGreyMaterial () {
        if (externMat != null)
        {
            ControlMat = externMat;
            return;
        }
        if (isMask2)
		    ControlMat = new Material (Shader.Find ("Custom/UI/GrayMask_2"));
        else
            ControlMat = new Material(Shader.Find("Custom/UI/GrayMask"));
	}

	void SetBtnImage () {

		Image btnImage = GetComponent<Image> ();
		
		if (btnImage != null) {
			
			btnImage.material = ControlMat;
		}
	}

	void SetChildrenImage () {

		Image[] images = transform.GetComponentsInChildren<Image> ();

		if (images != null) {

			foreach (Image image in images) {
				
				image.material = ControlMat;
			}
		}
	}

	void GetTextAndColor () {

		Text[] textArr = transform.GetComponentsInChildren<Text> ();

		if (textArr != null) {

			texts = new List<Text> (textArr);
			
			foreach (Text text in texts) {
				
				oldTextColors.Add (text.color);
			}
		}
	}

	void GetShadowAndColor () {

		Shadow[] shadowArr = transform.GetComponentsInChildren<Shadow> ();

		if (shadowArr != null) {

			shadows = new List<Shadow> (shadowArr);
			
			foreach (Shadow shadow in shadows) {
				
				oldShadowColors.Add (shadow.effectColor);
			}
		}
	}

}

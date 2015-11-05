using UnityEngine;
using System.Collections;

public enum FOXFADE_STATE
{
	NON,
	IN,
	OUT,
};

public class zFoxFadeFilter : MonoBehaviour {
	public static zFoxFadeFilter instance = null;

	public GameObject fadeFilterObject = null;
	public string attacheObject = "FadeFilterPoint";

	[System.NonSerialized] public FOXFADE_STATE fadeState;

	private float startTime;
	private float fadeTime;
	private Color fadeColor;

	void Awake(){
		instance = this;
		fadeState = FOXFADE_STATE.NON;
	}

	void SetFadeAction(FOXFADE_STATE state,Color color,float time){
		fadeState = state;
		startTime = Time.time;
		fadeTime = time;
		fadeColor = color;
	}

	public void FadeIn(Color color,float time){
		SetFadeAction(FOXFADE_STATE.IN,color,time);
	}
	
	public void FadeOut(Color color,float time){
		SetFadeAction(FOXFADE_STATE.OUT,color,time);
	}

	void SetFadeFilterColor(bool enabled,Color color){
		if(fadeFilterObject){
			fadeFilterObject.renderer.enabled = enabled;
			fadeFilterObject.renderer.material.color = color;
			SpriteRenderer sprite = fadeFilterObject.GetComponent<SpriteRenderer>();
			if(sprite){
				sprite.enabled = enabled;
				sprite.color = color;
				fadeFilterObject.SetActive(enabled);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(attacheObject != null){
			GameObject go = GameObject.Find(attacheObject);
			fadeFilterObject.transform.position = go.transform.position;
		}

		switch(fadeState){
			case FOXFADE_STATE.NON:
				break;

			case FOXFADE_STATE.IN:
				fadeColor.a = 1.0f - ((Time.time - startTime)/fadeTime);
				if(fadeColor.a > 1.0f || fadeColor.a <0.0f){
					fadeColor.a = 0.0f;
					fadeState = FOXFADE_STATE.NON;
					SetFadeFilterColor(false,fadeColor);
					break;
				}
			SetFadeFilterColor(true,fadeColor);
			break;

			case FOXFADE_STATE.OUT:
				fadeColor.a = (Time.time - startTime)/fadeTime;
				if(fadeColor.a >1.0f || fadeColor.a<0.0f){
					fadeColor.a = 1.0f;
					fadeState = FOXFADE_STATE.NON;
				}
				SetFadeFilterColor(true,fadeColor);
				break;
		}	
	}
}

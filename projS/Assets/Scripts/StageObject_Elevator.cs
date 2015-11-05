using UnityEngine;
using System.Collections;

public class StageObject_Elevator : MonoBehaviour {
	public float switchingTime = 5.0f;
	SliderJoint2D slide;

	float changeTime;

	// Use this for initialization
	void Start () {
		slide  = GetComponent<SliderJoint2D>();
		changeTime = Time.fixedTime;	
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.fixedTime > changeTime + switchingTime){
			slide.useMotor = (slide.useMotor)? false : true;
			changeTime = Time.fixedTime;
		}	
	}
}

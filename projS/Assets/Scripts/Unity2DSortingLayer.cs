﻿using UnityEngine;
using System.Collections;

public class Unity2DSortingLayer : MonoBehaviour {
	public string sortingLayerName = "Front";
	public int sortingOrder = 0;

	void Awake(){
		renderer.sortingLayerName = sortingLayerName;
		renderer.sortingOrder = sortingOrder;
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

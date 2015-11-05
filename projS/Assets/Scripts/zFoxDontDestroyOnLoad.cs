using UnityEngine;
using System.Collections;

public class zFoxDontDestroyOnLoad : MonoBehaviour {
	public bool DontDestroyEnabled = true;

	// Use this for initialization
	void Start () {
		if(DontDestroyEnabled){
			DontDestroyOnLoad(this);
		}
	}
}

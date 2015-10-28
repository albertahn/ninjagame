using UnityEngine;
using System.Collections;

public class Effect_ParticleChrDir : MonoBehaviour {

	Rigidbody2D rootObject;

	// Use this for initialization
	void Start () {
		rootObject = GetComponentInParent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		float ra = (rootObject.transform.localScale.x < 0) ? +50.0f : -50.0f;
		transform.transform.localRotation = Quaternion.Euler (270 + ra, 90, 0);
	}
}

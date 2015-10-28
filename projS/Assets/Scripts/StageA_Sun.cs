using UnityEngine;
using System.Collections;

public class StageA_Sun : MonoBehaviour {
	Transform playerTrfm;

	// Use this for initialization
	void Start () {
		playerTrfm = PlayerController.GetTransform ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetPosition = playerTrfm.position + new Vector3 (8.0f, 5.0f, 0.0f);
		transform.position = new Vector3 (targetPosition.x,
		                                 Mathf.Lerp (transform.position.y, targetPosition.y, 0.001f),
		                                 targetPosition.z);
	}
}

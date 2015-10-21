using UnityEngine;
using System.Collections;

public class PlayerMain : MonoBehaviour {

	public PlayerController playerCtrl;

	void Awke(){
		playerCtrl = GetComponent<PlayerController> ();
	}

	// Update is called once per frame
	void Update () {
		if (!playerCtrl.activeSts)
			return;
		float joyMv = Input.GetAxis ("Horizontal");
		playerCtrl.ActionMove (joyMv);

		if (Input.GetButtonDown ("Jump")) {
			playerCtrl.ActionJump();
			return;
		}

		if (Input.GetButtonDown ("Fire1") || Input.GetButtonDown ("Fire2") || Input.GetButtonDown ("Fire3")) {
			playerCtrl.ActionAttack();
		}
	}
}

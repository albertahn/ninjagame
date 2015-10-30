using UnityEngine;
using System.Collections;

public class PlayerMain : MonoBehaviour {

	public PlayerController playerCtrl;
	bool actionEtcRun = true;

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
			if(Input.GetAxisRaw("Vertical")<0.5f){
				playerCtrl.ActionAttack();
			}else{
				playerCtrl.ActionAttackJump();
			}
		}
		if(Input.GetAxisRaw("Vertical") > 0.7f){
			if(actionEtcRun){
				playerCtrl.ActionEtc();
				actionEtcRun=false;
			}else{
				actionEtcRun = true;
			}
		}
	}
}

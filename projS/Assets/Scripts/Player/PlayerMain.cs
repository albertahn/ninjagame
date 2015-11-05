using UnityEngine;
using System.Collections;

public class PlayerMain : MonoBehaviour {

	public PlayerController playerCtrl;
	public zFoxVirtualPad vpad;

	bool actionEtcRun = true;

	void Awke(){
		playerCtrl = GetComponent<PlayerController> ();
		vpad = GameObject.FindObjectOfType<zFoxVirtualPad>();
	}

	// Update is called once per frame
	void Update () {
		if (!playerCtrl.activeSts)
			return;


		float vpad_vertical = 0.0f;
		float vpad_horizontal = 0.0f;
		zFOXVPAD_BUTTON vpad_btnA = zFOXVPAD_BUTTON.NON;
		zFOXVPAD_BUTTON vpad_btnB = zFOXVPAD_BUTTON.NON;
		if(vpad !=null){
			vpad_vertical = vpad.vertical;
			vpad_horizontal = vpad.horizontal;
			vpad_btnA = vpad.buttonA;
			vpad_btnB = vpad.buttonB;
		}


		float joyMv = Input.GetAxis ("Horizontal");
		joyMv = Mathf.Pow(Mathf.Abs(joyMv),3.0f)*Mathf.Sign(joyMv);

		float vpadMv = vpad_horizontal;
		vpadMv = Mathf.Pow(Mathf.Abs(vpadMv),1.5f)*Mathf.Sign(vpadMv);

		playerCtrl.ActionMove (joyMv+ vpadMv);

		if (Input.GetButtonDown ("Jump")||vpad_btnA==zFOXVPAD_BUTTON.DOWN) {
			playerCtrl.ActionJump();
			return;
		}

		if (Input.GetButtonDown ("Fire1") || Input.GetButtonDown ("Fire2") || Input.GetButtonDown ("Fire3")
		    ||vpad_btnB == zFOXVPAD_BUTTON.DOWN) {
			if(Input.GetAxisRaw("Vertical") + vpad_vertical<0.5f){
				playerCtrl.ActionAttack();
			}else{
				playerCtrl.ActionAttackJump();
			}
			return;
		}

		if(Input.GetAxisRaw("Vertical") + vpad_vertical> 0.7f){
			if(actionEtcRun){
				playerCtrl.ActionEtc();
				actionEtcRun=false;
			}
		}else{
			actionEtcRun = true;
		}
	}
}

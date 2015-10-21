using UnityEngine;
using System.Collections;


public class ForCombo_p240 : MonoBehaviour {
	
	public PlayerController _playerCtrl;

	// Use this for initialization
	void Start () {
	
	}

	public void EnableAttackInput(){
		_playerCtrl.EnableAttackInput ();
	}

	public void SetNextAttack(string name){
		_playerCtrl.SetNextAttack (name);
	}

	// Update is called once per frame
	void Update () {
	
	}
}

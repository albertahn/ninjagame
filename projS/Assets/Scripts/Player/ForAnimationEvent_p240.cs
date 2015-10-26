using UnityEngine;
using System.Collections;


public class ForAnimationEvent_p240 : MonoBehaviour {
	
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



	public void AddForceAnimatorVx(float vx){
	}
	
	public void AddForceAnimatorVy(float vy){
		_playerCtrl.AddForceAnimatorVy (vy);
	}
	
	public void AddVelocityVx(float vx){
	}
	
	public void AddVelocityVy(float vy){
	}
	
	public void SetVelocityVx(float vx){
	}
	
	public void SetVelocityVy(float vy){
		_playerCtrl.SetVelocityVy (vy);
	}
	
	public void SetLightGravity(){
		_playerCtrl.SetLightGravity ();
	}
}

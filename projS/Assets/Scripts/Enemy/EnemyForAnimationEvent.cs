using UnityEngine;
using System.Collections;

public class EnemyForAnimationEvent : MonoBehaviour {
	public EnemyController _enemyCtrl;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	public void SetLightGravity(){
		_enemyCtrl.SetLightGravity ();
	}

	public void AddForceAnimatorVy(float vy){
		_enemyCtrl.AddForceAnimatorVy (vy);
	}

	public void AddVelocityVx(float vx){
		_enemyCtrl.AddForceAnimatorVx (vx);
	}
	public void ActionFire(){		
		_enemyCtrl.ActionFire ();
	}
}

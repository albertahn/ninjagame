using UnityEngine;
using System.Collections;

public class PlayerBodyController : MonoBehaviour {

	PlayerController playerCtrl;

	void Awake(){
		playerCtrl = transform.parent.GetComponent<PlayerController>();
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "EnemyArm"){
			EnemyController enemyCtrl = other.GetComponentInParent<EnemyController>();
			if(enemyCtrl.attackEnabled){
				enemyCtrl.attackEnabled = false;
				playerCtrl.dir = 
					(playerCtrl.transform.position.x < enemyCtrl.transform.position.x)?+1:-1;
				playerCtrl.AddForceAnimatorVx(-enemyCtrl.attackNockBackVector.x);
				playerCtrl.AddForceAnimatorVy(enemyCtrl.attackNockBackVector.y);
				playerCtrl.ActionDamage(enemyCtrl.attackDamage);
			}
		}else if(other.tag == "EnemyArmBullet"){
			FireBullet fireBullet = other.transform.GetComponent<FireBullet>();
			if(fireBullet.attackEnabled){
				fireBullet.attackEnabled = false;
				playerCtrl.dir = (playerCtrl.transform.position.x<fireBullet.transform.position.x)?+1:-1;
				playerCtrl.AddForceAnimatorVx(-fireBullet.attackNockBackVector.x);
				playerCtrl.AddForceAnimatorVy(fireBullet.attackNockBackVector.y);
				playerCtrl.ActionDamage(fireBullet.attackDamage);
				Destroy(other.gameObject);
			}
		}
	}

	void OnCollisionStay2D(Collision2D col){
		if(!playerCtrl.jumped &&
		   (col.gameObject.tag=="Road" ||
		 	col.gameObject.tag=="MoveObject" ||
		 	col.gameObject.tag=="Enemy")){
			playerCtrl.groundY = transform.parent.position.y;
		}
	}
}
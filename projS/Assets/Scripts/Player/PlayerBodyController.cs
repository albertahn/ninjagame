﻿using UnityEngine;
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
		}else if(other.tag=="CameraTrigger"){
			Camera.main.GetComponent<CameraFollow>().SetCamera(
				other.GetComponent<StageTrigger_Camera>().param);
		}
		else if(other.tag == "Item"){
			if(other.name == "Item_Koban"){
				PlayerController.score +=10;
			}else if(other.name =="Item_Ohoban"){
				PlayerController.score +=100000;
			}else if(other.name =="Item_Hyoutan"){
				playerCtrl.SetHP(playerCtrl.hp + playerCtrl.hpMax/3,playerCtrl.hpMax);
			}else if(other.name == "Item_Makimono"){
				playerCtrl.superMode = true;
				playerCtrl.GetComponent<Stage_AfterImage>().afterImageEnabled = true;
				playerCtrl.basScaleX = 2.0f;
				playerCtrl.transform.localScale = new Vector3(playerCtrl.basScaleX,2.0f,1.0f);
				Invoke("SuperModeEnd",10.0f);
			}
			Destroy(other.gameObject);
		}
	}
	void SuperModeEnd(){
		playerCtrl.superMode = false;
		playerCtrl.GetComponent<Stage_AfterImage> ().afterImageEnabled = false;
		playerCtrl.basScaleX = 1.0f;
		playerCtrl.transform.localScale = new Vector3 (playerCtrl.basScaleX, 1.0f, 1.0f);
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
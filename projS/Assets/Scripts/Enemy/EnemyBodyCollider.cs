using UnityEngine;
using System.Collections;

public class EnemyBodyCollider : MonoBehaviour {

	EnemyController enemyCtrl;
	Animator playerAnim;
	int attackHash = 0;

	void Awake(){
		enemyCtrl = GetComponentInParent<EnemyController> ();
		playerAnim = PlayerController.GetAnimator ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "PlayerArm"){
			AnimatorStateInfo stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
			if(attackHash != stateInfo.nameHash){
				attackHash = stateInfo.nameHash;
				enemyCtrl.ActionDamage();
			}
		}else if(other.tag == "PlayerArmBullet"){
			Destroy (other.gameObject);
			enemyCtrl.ActionDamage();
		}
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo stateInfo = playerAnim.GetCurrentAnimatorStateInfo (0);
		if(attackHash!=0 && stateInfo.nameHash == PlayerController.ANISTS_Idle){
			attackHash = 0;
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatAI : MonoBehaviour {
	public int freeAIMax = 3;
	public int blockAttackAIMax = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		var activeEnemyMainList = new List<EnemyMain> ();

		GameObject[] enemyList = GameObject.FindGameObjectsWithTag ("Enemy");
		if(enemyList == null){
			return;
		}
		foreach(GameObject enemy in enemyList){
			EnemyMain enemyMain = enemy.GetComponent<EnemyMain>();
			if(enemyMain !=null){
				if(enemyMain.combatAIOerder && enemyMain.cameraEnabled){
					activeEnemyMainList.Add(enemyMain);
				}
			}else{
				Debug.LogWarning(string.Format("CombatAI : EnemyMain null : {0} {1}", enemy.name,enemy.transform.position));
			}
		}
		int i = 0;
		foreach(EnemyMain enemyMain in activeEnemyMainList){
			if(i<freeAIMax){

			}else if(i<freeAIMax+blockAttackAIMax){
				if(enemyMain.aiState == ENEMYAISTS.RUNTOPLAYER)
					enemyMain.SetCombatAIState(ENEMYAISTS.WAIT);
			}else{
				if(enemyMain.aiState != ENEMYAISTS.WAIT){
					enemyMain.SetCombatAIState(ENEMYAISTS.WAIT);
				}
			}
			i++;
		}
	}
}

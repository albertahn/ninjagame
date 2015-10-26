using UnityEngine;
using System.Collections;

public class EnemyMain_B : EnemyMain {
	public int aiIfRUNTOPLAYER = 30;
	public int aiIfESCAPE = 20;
	public int aiIfRETURNTODOGPILE = 10;

	public int damageAttack_A = 1;
	public int damageAttack_B = 2;

	public override void FixedUpdateAI(){
		switch(aiState){
		case ENEMYAISTS.ACTIONSELECT:
				int n = SelectRandomAIState();
			if(n<aiIfRUNTOPLAYER){
				SetAIState(ENEMYAISTS.RUNTOPLAYER,3.0f);
			}else if(n<aiIfRUNTOPLAYER+aiIfESCAPE){
				SetAIState(ENEMYAISTS.ESCAPE,Random.Range(2.0f,5.0f));
			}else if(n<aiIfRUNTOPLAYER+aiIfESCAPE+aiIfRETURNTODOGPILE){
				if(dogPile!=null)
					SetAIState(ENEMYAISTS.RETURNTODOGPILE,3.0f);
			}else{
				SetAIState(ENEMYAISTS.WAIT,1.0f + Random.Range(0.0f,1.0f));
			}
			enemyCtrl.ActionMove(0.0f);
			break;

		case ENEMYAISTS.WAIT :
			enemyCtrl.ActionLookUp(player,0.1f);
			enemyCtrl.ActionMove(0.0f);
			break;
	
		case ENEMYAISTS.RUNTOPLAYER :
			if(GetDistancePlayerY()<3.0f){
				if(!enemyCtrl.ActionMoveToNear(player,2.0f)){
					Attack_A();
				}
			}else{				
				if(GetDistancePlayerX()>3.0f && !enemyCtrl.ActionMoveToNear(player,5.0f)){
					Attack_A();
				}
			}
			break;
		case ENEMYAISTS.RETURNTODOGPILE:
			if(!enemyCtrl.ActionMoveToNear(dogPile,2.0f)){
				SetAIState(ENEMYAISTS.ACTIONSELECT,1.0f);
			}
			break;
		case ENEMYAISTS.ESCAPE:
			if(!enemyCtrl.ActionMoveToFar(player,4.0f)){
				Attack_B();
			}
			break;
		}
	}

	void Attack_A(){
		enemyCtrl.ActionLookUp (player, 0.1f);
		enemyCtrl.ActionMove (0.0f);
		enemyCtrl.ActionAttack ("Attack_A", damageAttack_A);
		enemyCtrl.attackNockBackVector = new Vector2 (500.0f, 2000.0f);
		SetAIState (ENEMYAISTS.WAIT, 3.0f);
	}

	void Attack_B(){
		enemyCtrl.ActionLookUp (player, 0.1f);
		enemyCtrl.ActionMove (0.0f);
		enemyCtrl.ActionAttack("Attack_B", damageAttack_B);
		enemyCtrl.attackNockBackVector = new Vector2 (500.0f, 1000.0f);
		SetAIState (ENEMYAISTS.FREEZ, 5.0f);
	}
}

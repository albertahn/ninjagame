using UnityEngine;
using System.Collections;

public enum ENEMYAISTS
{
	ACTIONSELECT,
	WAIT,
	RUNTOPLAYER,
	JUMPTOPLAYER,
	ESCAPE,
	RETURNTODOGPILE,
	ATTACKONSIGHT,
	FREEZ,
}

public class EnemyMain : MonoBehaviour {
	public int debug_SelectRandomAIState = -1;
	public bool cameraSwitch = true;
	public bool inActiveZoneSwitch = false;
	public bool combatAIOerder = true;
	public float dogPileReturnLength = 10.0f;

	[System.NonSerialized] public bool cameraEnabled = false;
	[System.NonSerialized] public bool inActiveZone = false;
	[System.NonSerialized] public ENEMYAISTS aiState = ENEMYAISTS.ACTIONSELECT;
	[System.NonSerialized] public GameObject dogPile;

	protected EnemyController enemyCtrl;
	protected GameObject player;
	protected PlayerController playerCtrl;

	protected float aiActionTimeLength = 0.0f;
	protected float aiActionTimeStart = 0.0f;
	protected float distanceToPlayer = 0.0f;
	protected float distanceToPlayerPrev = 0.0f;

	public virtual void Awake(){
		enemyCtrl = GetComponent<EnemyController> ();
		player = PlayerController.GetGameObject ();
		playerCtrl = player.GetComponent<PlayerController> ();
	}

	// Use this for initialization
	void Start () {
		StageObject_DogPile[] dogPileList = GameObject.FindObjectsOfType<StageObject_DogPile> ();

		foreach(StageObject_DogPile findDogPile in dogPileList){
			foreach(GameObject go in findDogPile.enemyList){
				if(gameObject == go){
					dogPile = findDogPile.gameObject;
					break;
				}
			}
		}
	}

	
	// Update is called once per frame
	public virtual void Update () {
		cameraEnabled = false;
	}

	void OnTriggerStay2D(Collider2D other){
		if(enemyCtrl.grounded && CheckAction ()){
			if(other.name == "EnemyJumpTrigger_L"){
				if(enemyCtrl.ActionJump ()){
					enemyCtrl.ActionMove (-1.0f);
				}
			}else if(other.name == "EnemyJumpTrigger_R"){
				if(enemyCtrl.ActionJump()){
					enemyCtrl.ActionMove (1.0f);
				}
			}else if(other.name == "EnemyJumpTrigger"){
				enemyCtrl.ActionJump();
			}
		}
	}

	public virtual void FixedUpdate(){
		if(BeginEnemyCommonWork()){
			FixedUpdateAI();
			EndEnemyCommonWork();
		}
	}

	public virtual void FixedUpdateAI(){

	}

	public bool BeginEnemyCommonWork(){
		if(enemyCtrl.hp<=0){
			return false;
		}

		if(inActiveZoneSwitch){
			inActiveZone = false;
			Vector3 vecA = player.transform.position + playerCtrl.enemyActiveZonePointA;
			Vector3 vecB = player.transform.position + playerCtrl.enemyActiveZonePointB;

			if(transform.position.x > vecA.x && transform.position.x <vecB.x &&
			   transform.position.y > vecA.y && transform.position.y <vecB.y){
				inActiveZone = true;
			}
		}


		if(enemyCtrl.grounded){
			if(cameraSwitch && !cameraEnabled && !inActiveZone){
				enemyCtrl.ActionMove(0.0f);
				enemyCtrl.cameraRendered = false;
				enemyCtrl.animator.enabled = false;
				rigidbody2D.Sleep();
				return false;
			}
		}

		enemyCtrl.animator.enabled = true;
		enemyCtrl.cameraRendered = true;

		if(!CheckAction()){
			return false;
		}

		if(dogPile != null){
			if(GetDistanceDogPile() > dogPileReturnLength){
				aiState = ENEMYAISTS.RETURNTODOGPILE;
			}
		}

		return true;
	}
	public float GetDistanceDogPile(){
		return Vector3.Distance (transform.position, dogPile.transform.position);
	}

	public void EndEnemyCommonWork(){
		float time = Time.fixedTime - aiActionTimeStart;
		if(time > aiActionTimeLength){
			aiState = ENEMYAISTS.ACTIONSELECT;
		}
	}

	public bool CheckAction(){
		AnimatorStateInfo stateInfo = enemyCtrl.animator.GetCurrentAnimatorStateInfo (0);
		if(stateInfo.tagHash == EnemyController.ANITAG_ATTACK ||
		   stateInfo.tagHash == EnemyController.ANISTS_DMG_A ||
		   stateInfo.tagHash == EnemyController.ANISTS_DMG_B ||
		   stateInfo.tagHash == EnemyController.ANISTS_Dead){
			return false;
		}
		return true;
	}

	public int SelectRandomAIState(){
		#if UNITY_EDITOR
			if(debug_SelectRandomAIState>=0){
			return debug_SelectRandomAIState;
			}
		#endif
		return Random.Range (0, 100 + 1);
	}

	public void SetAIState(ENEMYAISTS sts,float t){
		aiState = sts;
		aiActionTimeStart = Time.fixedTime;
		aiActionTimeLength = t;
	}

	public virtual void SetCombatAIState(ENEMYAISTS sts){
		aiState = sts;
		aiActionTimeStart = Time.fixedTime;
		enemyCtrl.ActionMove (0.0f);
	}

	public float GetDistancePlayer(){
		distanceToPlayerPrev = distanceToPlayer;
		distanceToPlayer = Vector3.Distance(transform.position,playerCtrl.transform.position);
		return distanceToPlayer;
	}

	public bool IsChangeDistancePlayer(float l){
		return(Mathf.Abs (distanceToPlayer - distanceToPlayerPrev) > l);
	}

	public float GetDistancePlayerX(){
		Vector3 posA = transform.position;
		Vector3 posB = playerCtrl.transform.position;
		posA.y = 0;posA.z = 0;
		posB.y = 0;posB.z = 0;
		return Vector3.Distance(posA,posB);
	}

	public float GetDistancePlayerY(){
		Vector3 posA = transform.position;
		Vector3 posB = playerCtrl.transform.position;
		posA.x = 0;posA.z = 0;
		posB.x = 0;posB.z = 0;
		return Vector3.Distance(posA,posB);
	}
}

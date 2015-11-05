using UnityEngine;
using System.Collections;

public class PlayerController : BaseCharacterController {
	public float initHpMax = 20.0f;
	[Range(0.1f,100.0f)] public float initSpeed = 12.0f;

	int jumpCount = 0;
	bool breakEnabled = true;
	float groundFriction = 0.0f;

	public readonly static int ANISTS_Idle = Animator.StringToHash("Base Layer.Player_Idle");
	public readonly static int ANISTS_Walk = Animator.StringToHash("Base Layer.Player_Walk");
	public readonly static int ANISTS_Run = Animator.StringToHash("Base Layer.Player_Run");
	public readonly static int ANISTS_Jump = Animator.StringToHash("Base Layer.Player_Jump");
	public readonly static int ANISTS_DEAD = Animator.StringToHash("Base Layer.Player_Dead");
	public readonly static int ANISTS_ATTACK_A = Animator.StringToHash("Base Layer.Player_ATK_A");
	public readonly static int ANISTS_ATTACK_B = Animator.StringToHash("Base Layer.Player_ATK_B");
	public readonly static int ANISTS_ATTACK_C = Animator.StringToHash("Base Layer.Player_ATK_C");
	public readonly static int ANISTS_ATTACK_JUMP_A = Animator.StringToHash("Base Layer.Player_ATKJUMP_A");
	public readonly static int ANISTS_ATTACK_JUMP_B = Animator.StringToHash("Base Layer.Player_ATKJUMP_B");

	volatile bool atkInputEnabled = false;
	volatile bool atkInputNow = false;
	public static float nowHpMax = 0;
	public static float nowHp = 0;
	public static int score = 0;

	public static bool itemKeyA = false;
	public static bool itemKeyB = false;
	public static bool itemKeyC = false;


	[System.NonSerialized]public Vector3 enemyActiveZonePointA;
	[System.NonSerialized]public Vector3 enemyActiveZonePointB;
	[System.NonSerialized]public float groundY = 0.0f;
	[System.NonSerialized]public int comboCount = 0;
	[System.NonSerialized]public bool superMode = false;

	LineRenderer hudHpBar;
	TextMesh hudScore;
	TextMesh hudCombo;

	float comboTimer = 0.0f;

	public static bool checkPointEnabled = false;
	public static string checkPointSceneName = "";
	public static string checkPointLabelName = "";
	public static float checkPointHp = 0;
	public static bool initParam = true;

	public static float startFadeTime = 2.0f;

	public static GameObject GetGameObject(){
		return GameObject.FindGameObjectWithTag ("Player");
	}
	public static Transform GetTransform(){
		return GameObject.FindGameObjectWithTag ("Player").transform;
	}
	public static PlayerController GetController(){
		return GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
	}
	public static Animator GetAnimator(){
		return GameObject.FindWithTag ("Player").GetComponentInChildren<Animator> ();
	}

	protected override void Awake(){
		base.Awake ();

		GameObject.Find("VRPad").SetActive(true);

		hudHpBar = GameObject.Find ("HUD_HPBar").GetComponent<LineRenderer> ();
		hudScore = GameObject.Find ("HUD_Score").GetComponent<TextMesh> ();
		hudCombo = GameObject.Find ("HUD_Combo").GetComponent<TextMesh> ();

		speed = initSpeed;
		SetHP (initHpMax, initHpMax);

		BoxCollider2D boxCol2D = transform.Find ("Collider_EnemyActiveZone").GetComponent<BoxCollider2D> ();
		enemyActiveZonePointA = new Vector3
			(boxCol2D.center.x - boxCol2D.size.x / 2.0f, boxCol2D.center.y - boxCol2D.size.y / 2.0f);
		enemyActiveZonePointB = new Vector3
			(boxCol2D.center.x + boxCol2D.size.x / 2.0f, boxCol2D.center.y + boxCol2D.size.y / 2.0f);
		boxCol2D.transform.gameObject.SetActive (false);

		if(SaveData.continuePlay){
			if(!SaveData.LoadGamePlay(true)){
				initParam = false;
			}
			SaveData.continuePlay = false;
		}

		if(initParam){
			SetHP(initHpMax,initHpMax);
			PlayerController.score = 0;
			PlayerController.checkPointEnabled = false;
			PlayerController.checkPointLabelName = "";
			PlayerController.checkPointSceneName = Application.loadedLevelName;
			PlayerController.checkPointHp = initHpMax;
			PlayerController.itemKeyA = false;
			PlayerController.itemKeyB = false;
			PlayerController.itemKeyC = false;
			SaveData.DeleteAndInit(false);
			initParam = false;
		}else{
			SaveData.LoadGamePlay(false);
		}

		if(SetHP(PlayerController.nowHp,PlayerController.nowHpMax)){
			SetHP(1,initHpMax);
		}

		if(checkPointEnabled){
			StageTrigger_CheckPoint[] triggerList = GameObject.Find("Stage").GetComponentsInChildren<StageTrigger_CheckPoint>();
			foreach(StageTrigger_CheckPoint trigger in triggerList){
				if(trigger.labelName == checkPointLabelName){
					transform.position = trigger.transform.position;
					groundY = transform.position.y;
					Camera.main.GetComponent<CameraFollow>().SetCamera(trigger.cameraParam);
					break;
				}
			}
		}
		Camera.main.transform.position = new Vector3 (
			transform.position.x, groundY, Camera.main.transform.position.z);

		GameObject.Find("VRPad").SetActive(SaveData.VRPadEnabled);

		Transform hud = GameObject.FindGameObjectWithTag("SubCamera").transform;
		hud.Find("Stage_Item_Key_A").GetComponent<SpriteRenderer>().enabled = itemKeyA;
		hud.Find("Stage_Item_Key_B").GetComponent<SpriteRenderer>().enabled = itemKeyB;
		hud.Find("Stage_Item_Key_C").GetComponent<SpriteRenderer>().enabled = itemKeyC;
	}

	protected override void Start(){
		base.Start ();

		zFoxFadeFilter.instance.FadeIn(Color.black,startFadeTime);
		startFadeTime = 2.0f;
	}

	protected override void Update(){
		base.Update ();
		hudHpBar.SetPosition (1, new Vector3 (5.0f * (hp / hpMax), 0.0f, 0.0f));
		hudScore.text = string.Format ("Score {0}", score);

		if(comboTimer <= 0.0f){
			hudCombo.gameObject.SetActive(false);
			comboCount = 0;
			comboTimer = 0.0f;
		}else{
			comboTimer -= Time.deltaTime;
			if(comboTimer>5.0f){
				comboTimer = 5.0f;
			}
			float s = 0.3f + 0.5f * comboTimer;
			hudCombo.gameObject.SetActive(true);
			hudCombo.transform.localScale = new Vector3(s,s,1.0f);
		}
	}

	protected override void FixedUpdateCharacter(){
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo (0);

		if (jumped) {
			if((grounded&&!groundedPrev)||(grounded&&Time.fixedTime>jumpStartTime + 1.0f)){
				animator.SetTrigger ("Idle");
				jumped = false;
				jumpCount = 0;
				rigidbody2D.gravityScale = gravityScale;
			}
			if(Time.fixedTime > jumpStartTime + 1.0f){
				if(stateInfo.nameHash == ANISTS_Idle ||
				   stateInfo.nameHash == ANISTS_Walk ||
				   stateInfo.nameHash == ANISTS_Run ||
				   stateInfo.nameHash == ANISTS_Jump){
					rigidbody2D.gravityScale = gravityScale;
				}
			}
		}else{
			jumpCount=0;
			rigidbody2D.gravityScale = gravityScale;
		}

		if (stateInfo.nameHash == ANISTS_ATTACK_A ||
			stateInfo.nameHash == ANISTS_ATTACK_B ||
			stateInfo.nameHash == ANISTS_ATTACK_C ||
			stateInfo.nameHash == ANISTS_ATTACK_A ||
			stateInfo.nameHash == ANISTS_ATTACK_B) {
			speedVx = 0;
		}

		transform.localScale = new Vector3 (basScaleX * dir, transform.localScale.y, transform.localScale.z);

		if (jumped && !grounded) {
			if(breakEnabled){
				breakEnabled = false;
				speedVx *= 0.9f;
			}
		}

		if (breakEnabled) {
			speedVx *= groundFriction;
		}

		//Camera.main.transform.position = transform.position - Vector3.forward;
	}

	public void ActionEtc(){
		Collider2D[] otherAll = Physics2D.OverlapPointAll (groundCheck_C.position);
		foreach(Collider2D other in otherAll){
			if(other.tag == "EventTrigger"){
				StageTrigger_Link link = other.GetComponent<StageTrigger_Link>();
				if(link!=null){
					link.Jump();
				}
			}else if(other.tag =="KeyDoor"){
				StageObject_KeyDoor keydoor = other.GetComponent<StageObject_KeyDoor>();
				keydoor.OpenDoor();
			}else if(other.name =="Stage_Switch_Body"){
				StageObject_Switch sw = other.transform.parent.GetComponent<StageObject_Switch>();
				sw.SwitchTurn();
			}
		}
	}


	public override void ActionMove(float n){
		if (!activeSts) {
			return;
		}

		float dirOld = dir;
		breakEnabled = false;

		float moveSpeed = Mathf.Clamp (Mathf.Abs (n), -1.0f, +1.0f);
		animator.SetFloat ("MovSpeed", moveSpeed);

		if (n != 0.0f) {
			dir = Mathf.Sign (n);
			moveSpeed = (moveSpeed < 0.5f) ? (moveSpeed * (1.0f / 0.5f)) : 1.0f;
			speedVx = initSpeed * moveSpeed * dir;
		} else {
			breakEnabled = true;
		}

		if (dirOld != dir) {
			breakEnabled = true;
		}
	}

	public void EnableAttackInput(){
		atkInputEnabled = true;
	}

	public void SetNextAttack(string name){
		if (atkInputNow == true) {
			atkInputNow = false;
			animator.Play(name);
		}
	}

	public void ActionAttack(){
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo (0);
		if(stateInfo.nameHash == ANISTS_Idle ||
		   stateInfo.nameHash == ANISTS_Walk ||
		   stateInfo.nameHash == ANISTS_Run  ||
		   stateInfo.nameHash == ANISTS_Jump ||
		   stateInfo.nameHash == ANISTS_ATTACK_C){
			animator.SetTrigger("Attack_A");
			if(stateInfo.nameHash == ANISTS_Jump ||
			   stateInfo.nameHash == ANISTS_ATTACK_C) {
				rigidbody2D.velocity = Vector2.zero;
				rigidbody2D.gravityScale = 0.1f;
			}
		}else{
			if(atkInputEnabled){
				atkInputEnabled = false;
				atkInputNow = true;
			}
		}
	}

	public void ActionJump(){
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo (0);
		if(stateInfo.nameHash == ANISTS_Idle ||
		   stateInfo.nameHash == ANISTS_Walk ||
		   stateInfo.nameHash == ANISTS_Run  ||
		   (stateInfo.nameHash == ANISTS_Jump &&
		 	rigidbody2D.gravityScale >= gravityScale)){
			switch(jumpCount){
				case 0:
					if(grounded){
						animator.SetTrigger("Jump");
						rigidbody2D.velocity = Vector2.up * 30.0f;
						jumpStartTime = Time.fixedTime;
						jumped = true;
						jumpCount++;
					}
					break;
				case 1:
					if(!grounded){
						animator.Play ("Player_Jump",0,0.0f);
						rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,20.0f);
						jumped = true;
						jumpCount++;
					}
				break;
			}
		}
	}

	public void ActionAttackJump(){
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo (0);
		if(grounded &&
		   (stateInfo.nameHash == ANISTS_Idle ||
		 	stateInfo.nameHash == ANISTS_Walk ||
			stateInfo.nameHash == ANISTS_Run  ||
			stateInfo.nameHash == ANISTS_ATTACK_A  ||
		 	stateInfo.nameHash == ANISTS_ATTACK_B)){
			animator.SetTrigger("Attack_C");
			jumpCount = 2;
		}else{
			if(atkInputEnabled){
				atkInputEnabled = false;
				atkInputNow = true;
			}
		}
	}

	public void ActionDamage(float damage){
		if(SaveData.debug_Invicible){
			return;
		}

		if(!activeSts)
			return;

		animator.SetTrigger("DMG_A");
		speedVx = 0;
		rigidbody2D.gravityScale = gravityScale;

		if(jumped){
			damage *= 1.5f;
		}
		if(SetHP(hp-damage,hpMax)){
			Dead (true);
		}
	}

	public override void Dead(bool gameOver){
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
		if(!activeSts || stateInfo.nameHash == ANISTS_DEAD){
			return;
		}
		base.Dead (gameOver);

		zFoxFadeFilter.instance.FadeOut(Color.black,2.0f);

		if(gameOver){
			SetHP(0,hpMax);
			Invoke("GameOver",3.0f);
		}else{
			SetHP(hp/2,hpMax);
			Invoke ("GameReset", 3.0f);
		}
	
		GameObject.Find ("HUD_Dead").GetComponent<MeshRenderer> ().enabled = true;
	//	GameObject.Find ("HUD_DeadShadow").GetComponent<MeshRenderer> ().enabled = true;
		if(GameObject.Find("VRPad")!=null){
			GameObject.Find("VRPad").SetActive(false);
		}
	}

	public void GameOver(){
		SaveData.SaveHiScore(score);
		PlayerController.score = 0;
		PlayerController.nowHp = PlayerController.checkPointHp;
		SaveData.SaveGamePlay();

		AppSound.instance.fm.Stop("BGM");
		if(SaveData.newRecord>0){
			AppSound.instance.BGM_HISCORE_RANKIN.Play();
		}else{
			AppSound.instance.BGM_HISCORE.Play();
		}

		Application.LoadLevel (Application.loadedLevelName);
	}
	void GameReset(){
		SaveData.SaveGamePlay();
		Application.LoadLevel (Application.loadedLevelName);
	}

	public override bool SetHP(float _hp,float _hpMax){
		if(_hp>_hpMax){
			_hp = _hpMax;
		}
		nowHp = _hp;
		nowHpMax = _hpMax;
		return base.SetHP (_hp, _hpMax);
	}
	public void AddCombo(){
		comboCount++;
		comboTimer += 1.0f;
		hudCombo.text = string.Format ("Combo {0}", comboCount);
	}
}

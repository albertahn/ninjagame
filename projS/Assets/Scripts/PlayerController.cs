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
	public readonly static int ANISTS_ATTACK_A = Animator.StringToHash("Base Layer.Player_ATK_A");
	public readonly static int ANISTS_ATTACK_B = Animator.StringToHash("Base Layer.Player_ATK_B");
	public readonly static int ANISTS_ATTACK_C = Animator.StringToHash("Base Layer.Player_ATK_C");
	public readonly static int ANISTS_ATTACK_JUMP_A = Animator.StringToHash("Base Layer.Player_ATKJUMP_A");
	public readonly static int ANISTS_ATTACK_JUMP_B = Animator.StringToHash("Base Layer.Player_ATKJUMP_B");

	volatile bool atkInputEnabled = false;
	volatile bool atkInputNow = false;

	protected override void Awake(){
		base.Awake ();

		speed = initSpeed;
		SetHP (initHpMax, initHpMax);
	}

	protected override void FixedUpdateCharacter(){
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo (0);

		if (jumped) {
			if((grounded&&!groundedPrev)||(grounded&&Time.fixedTime>jumpStartTime + 1.0f)){
				animator.SetTrigger ("Idle");
				jumped = false;
				jumpCount = 0;
			}
		}
		if (!jumped) {
			jumpCount = 0;
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

		Camera.main.transform.position = transform.position - Vector3.forward;
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

	public void ActionJump(){
		switch (jumpCount) {
			case 0:
				if(grounded){
					animator.SetTrigger("Jump");
					rigidbody2D.velocity = Vector2.up *30.0f;
					jumpStartTime = Time.fixedTime;
					jumped = true;
					jumpCount++;
				}
			break;
			case 1:
				if(!grounded){
					animator.Play("Player_Jump",0,0.0f);
					rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,20.0f);
					jumped = true;
					jumpCount++;
				}
			break;
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
		if (stateInfo.nameHash == ANISTS_Idle ||
			stateInfo.nameHash == ANISTS_Walk ||
			stateInfo.nameHash == ANISTS_Run ||
			stateInfo.nameHash == ANISTS_Jump) {
			animator.SetTrigger ("Attack_A");
		} else {
			if(atkInputEnabled){
				atkInputEnabled = false;
				atkInputNow=true;
			}
		}
	}
}

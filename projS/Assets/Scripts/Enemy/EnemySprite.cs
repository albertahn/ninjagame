using UnityEngine;
using System.Collections;

public class EnemySprite : MonoBehaviour {
	EnemyMain enemyMain;

	// Use this for initialization
	void Start () {
		enemyMain = GetComponentInParent<EnemyMain> ();
	}

	void OnWillRenderObject(){
		if(Camera.current.tag=="MainCamera"){
			enemyMain.cameraEnabled = true;
		}
	}
}

using UnityEngine;
using System.Collections;

public class StageObject_DogPile : MonoBehaviour {
	public GameObject[] enemyList;
	public GameObject[] destroyObjectList;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("CheckEnemy", 0.0f, 1.0f);
	}

	void CheckEnemy(){
		bool flag = true;
		foreach(GameObject enemy in enemyList){
			if(enemy != null){
				flag = false;
			}
		}
		if(flag){
			foreach(GameObject destroyObject in destroyObjectList){
				destroyObject.AddComponent<Effect_FadeObject>();
				destroyObject.SendMessage("FadeStart");
				Destroy(destroyObject,1.0f);
			}
			CancelInvoke("CheckEnemy");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

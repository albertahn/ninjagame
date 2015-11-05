using UnityEngine;
using System.Collections;

public class StageTrigger_EventSound : MonoBehaviour {
	public string playGroup = "BGM";
	public string playAudio = "";
	public bool loop = true;
	public bool stopPlayGround = true;

	void OnTriggerEnter2D_PlayerEvent(GameObject go){
		if(stopPlayGround){
			if(!AppSound.instance.fm.FindAudioSource(playGroup,playAudio).isPlaying){
				AppSound.instance.fm.FadeOutVolumeGroup(
					playGroup,playAudio,0.0f,1.0f,false
				);
			}
			if(playAudio !=""){
				AppSound.instance.fm.SetVolume(playGroup,playAudio,SaveData.SoundBGMVolume);
				AppSound.instance.fm.PlayDontOverride(playGroup,playAudio,loop);
			}
		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using UnityEditor;
using System.Collections;

public class zFoxMenuGenerateUID{
	[MenuItem("zFoxTools/UID/Generate")]

	public static void GererateUID(){
		int guidIndex = 0;

		if(!EditorUtility.DisplayDialog(
			"UID Generate","Generate UID?","Ok","cancle")){
			return;
		}

		zFoxUID[] uidList = GameObject.Find("Stage").GetComponentsInChildren<zFoxUID>();

		foreach(zFoxUID uidItem in uidList){
			if(uidItem.uid !=null){
				switch(uidItem.type){
				case zFOXUID_TYPE.NUMBER:
					uidItem.uid = guidIndex.ToString();
					guidIndex++;
					break;
				case zFOXUID_TYPE.GUID:
					uidItem.uid = System.Guid.NewGuid().ToString();
					break;
				}
			}
			EditorUtility.SetDirty(uidItem);
		}
	}

	[MenuItem("zFoxTools/UID/Delete")]
	public static void DeleteUID(){
		if(EditorUtility.DisplayDialog("UID DELETE","Delete UID?","ok","cancel")){
			zFoxUID[] uidList = GameObject.Find("Stage").GetComponentsInChildren<zFoxUID>();
			foreach(zFoxUID uidItem in uidList){
				uidItem.uid = "(non)";
				EditorUtility.SetDirty(uidItem);
			}
		}
	}
}

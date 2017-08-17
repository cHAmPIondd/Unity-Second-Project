using UnityEngine;
using System.Collections;

public class SpaceInvader2_App : App {
	
	public SpaceInvader2_App(DesktopChildSystem DeskChiSy)
	{
		m_AppID = DeskChiSy.appList.Count;
		m_DesktopParent = DeskChiSy;
		m_IsRegister = false;

		m_IconRect = new Rect (0.0557f,0.7167f,0.1016f,0.1447f);//数据根据UI图修改
		m_IconRect=RectChange(m_IconRect);

	}
	public override IEnumerator Open() // 打开软件
	{
		GameObject tempGO;
		GameObjectManager.instance.rendererDict.TryGetValue ("GAME_ICON", out tempGO);
		tempGO.SetActive (true);
		GameObject tempGO2;
		GameObjectManager.instance.rendererDict.TryGetValue ("GAME_ICON(0)", out tempGO2);
		tempGO2.SetActive (false);	
		m_DesktopParent.appMono.desktopChildSystem.appList [7].isRegister = true;
		m_DesktopParent.appList.Remove (this);
		yield return 0;	
	}
	public override string ToString()
	{
		return "SpaceInvader2_App";
	}
}

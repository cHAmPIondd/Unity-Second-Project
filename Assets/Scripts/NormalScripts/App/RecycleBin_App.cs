using UnityEngine;
using System.Collections;

public class RecycleBin_App : DesktopApp {
	
	public RecycleBin_App(DesktopChildSystem DeskChiSy)
		:base(DeskChiSy)
	{
		m_IsRegister = true;

		m_IconRect = new Rect (0.0099f,0.1288f,0.1016f,0.1447f);//数据根据UI图修改
		m_IconRect=RectChange(m_IconRect);

		desktopChildSystem.AddApp1(m_DesktopParent.appMono.stayBG[10],m_DesktopParent.appMono.selectionBG[10]);
		desktopChildSystem.AddApp2 (new SpaceInvader2_App(desktopChildSystem));

	}	
	public override string ToString()
	{
		return "RecycleBin_App";
	}

}

using UnityEngine;
using System.Collections;

public class DesktopApp : App {

	private DesktopChildSystem m_DesktopChildSystem;
	public DesktopChildSystem desktopChildSystem{get{ return m_DesktopChildSystem;}}

	public DesktopApp(DesktopChildSystem DeskChiSy)
		:base(DeskChiSy)
	{
		m_IsRegister = true;

		m_IconRect = new Rect (0.0099f,0.1288f,0.1016f,0.1447f);//数据根据UI图修改
		m_IconRect=RectChange(m_IconRect);

		m_AppTransform.gameObject.AddComponent<DesktopChildSystem> ();
		m_DesktopChildSystem = m_AppTransform.GetComponent<DesktopChildSystem> ();
		m_DesktopChildSystem.appMono = m_DesktopParent.appMono;

	}	
	public override void UpdateApp()//当使用此软件时Update
	{
		if(isOpen)
			m_DesktopChildSystem.UpdateDesktopSystem ();
	}
	public override string ToString()
	{
		return "DesktopApp";
	}
	public override bool IsCanEsc()
	{
		return m_DesktopChildSystem.IsCanEsc();
	}

}

using UnityEngine;
using System.Collections;

public class Help_App : App {
	private HelpMono m_HelpMono;
	public Help_App(DesktopChildSystem DeskChiSy)
		:base(DeskChiSy)
	{
		m_IsRegister = false;
		if (DeskChiSy.appMono as ComputerMono != null) 
		{
			m_IconRect = new Rect (0.0943f, 0.8752f, 0.0365f, 0.0682f);//数据根据UI图修改
			m_IconRect = RectChange (m_IconRect);
			m_CloseRect = new Rect (0.9416f, 0.9198f, 0.0584f, 0.0702f);
			m_CloseRect = RectChange (m_CloseRect);
		} 
		else 
		{
			m_IconRect = new Rect (0.9174f, 0.9194f, 0.0778f, 0.0368f);//数据根据UI图修改
			m_IconRect = RectChange (m_IconRect);
		//	m_CloseRect = new Rect (0.9416f, 0.9198f, 0.0584f, 0.0702f);
		//	m_CloseRect = RectChange (m_CloseRect);
		}
		m_HelpMono = m_AppTransform.GetComponent<HelpMono> ();
	}
	public override void UpdateApp()//当使用此软件时Update
	{
		if (m_IsOpen) {
			m_HelpMono.update ();
		}
	}
	protected override void OpenOperation()
	{
		m_HelpMono.Init ();
	}
	protected override void CloseOperation()
	{
		m_HelpMono.Close ();
	}
	public override string ToString()
	{
		return "Help_App";
	}
}

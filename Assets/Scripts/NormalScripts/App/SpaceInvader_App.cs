using UnityEngine;
using System.Collections;

public class SpaceInvader_App : App{

	private SpaceInvaderMono m_GameMono;
	private bool m_IsOver;
	private bool m_IsVictory;
	private float m_Timer=0;
	public SpaceInvader_App(DesktopChildSystem DeskChiSy)
		:base(DeskChiSy)
	{
		m_IsRegister = false;

		m_IconRect = new Rect (0.1387f,0.4748f,0.1016f,0.1447f);//数据根据UI图修改
		m_IconRect=RectChange(m_IconRect);
		m_GameMono = m_AppTransform.GetComponent<SpaceInvaderMono> ();
		m_GameMono.appParent = this;
	}
	public override void UpdateApp()//当使用此软件时Update
	{
		if (m_IsOpen) 
		{
			if (Input.GetKeyDown (KeyCode.Escape)) 
			{
				m_GameMono.Close ();
			}
			if (!m_IsOver&&!m_IsVictory) 
			{
				if (m_GameMono.hero == null) {
					m_GameMono.GameOver ();
					m_IsOver = true;
				}
				if (m_GameMono.boss == null) {
					m_GameMono.Victory ();
					m_IsVictory = true;
				}
			}
			else 
			{
				m_Timer += Time.deltaTime;
				if (m_Timer > 0.7f) 
				{
					if (Input.anyKeyDown) {
						m_Timer = 0;
						if (m_IsOver) {
							m_GameMono.Init ();
							m_IsOver = false;
							m_IsVictory = false;
						}
						if (m_IsVictory) {
							m_IsVictory = false;
							m_IsOver = false;
							m_GameMono.Close ();
						}
					}
				}
			}
		}
	}
	protected override void OpenOperation()
	{
		m_Timer = 0;
		m_GameMono.Init ();
		m_IsOver = false;
	}  
	public override string ToString()
	{
		return "SpaceInvader_App";
	}
	public override bool IsCanEsc()
	{
		return true;
	}
}
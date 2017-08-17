using UnityEngine;
using System.Collections;

public class PhoneMono : AppMono {


	void Start()
	{
		m_DesktopChildSystem.AddApp1(m_StayBG[0],m_SelectionBG[0]);
		m_DesktopChildSystem.AddApp2 (new VS_App(m_DesktopChildSystem));
	//	m_DesktopChildSystem.AddApp1(m_StayBG[1],m_SelectionBG[1]);
	//	m_DesktopChildSystem.AddApp2 (new Note_App(m_DesktopChildSystem));
	}


}

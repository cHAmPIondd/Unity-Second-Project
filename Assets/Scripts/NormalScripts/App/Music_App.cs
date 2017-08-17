using UnityEngine;
using System.Collections;

public class Music_App :  App {

	public Music_App(DesktopChildSystem DeskChiSy)
		:base(DeskChiSy)
	{
		m_IsRegister = false;

		m_IconRect = new Rect (0.1387f,0.6478f,0.1016f,0.1447f);//数据根据UI图修改
		m_IconRect=RectChange(m_IconRect);

	}
	public override string ToString()
	{
		return "Music_App";
	}
}

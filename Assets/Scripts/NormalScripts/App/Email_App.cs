using UnityEngine;
using System.Collections;

public class Email_App :  App {

	public Email_App(DesktopChildSystem DeskChiSy)
		:base(DeskChiSy)
	{
		m_IsRegister = false;

		m_IconRect = new Rect (0.0099f,0.3018f,0.1016f,0.1447f);//数据根据UI图修改
		m_IconRect=RectChange(m_IconRect);

	}
	public override string ToString()
	{
		return "Email_App";
	}
}

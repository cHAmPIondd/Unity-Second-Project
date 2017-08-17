using UnityEngine;
using System.Collections;

public class Note2_App : App {

	public Note2_App(DesktopChildSystem DeskChiSy)
		:base(DeskChiSy)
	{
		m_IsRegister = true;

		m_IconRect = new Rect (0.0557f,0.7167f,0.1016f,0.1447f);//数据根据UI图修改
		m_IconRect=RectChange(m_IconRect);

		m_CloseRect = new Rect (0.8817f,0.9f,0.0584f,0.07f);
		m_CloseRect = RectChange (m_CloseRect);
	}
	public override void UpdateApp()
	{
		if(!CodeLibrary.instance.codeList.Contains("MOVE_CODE"))
		{
			GameProgressManager.instance.TipAnimation ("点击文字获得代码",false);
		}
	}
	public override string ToString()
	{
		return "Note2_App";
	}
}

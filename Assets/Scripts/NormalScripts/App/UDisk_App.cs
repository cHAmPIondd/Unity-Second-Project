using UnityEngine;
using System.Collections;

public class UDisk_App : DesktopApp {

	private Rect m_ExtractRect;
	public Rect extractRect
	{
		get{ return m_ExtractRect;}
	}

	public UDisk_App(DesktopChildSystem DeskChiSy)
		:base(DeskChiSy)
	{
		m_IsRegister = true;

		m_IconRect = new Rect (0.8235f,0.8088f,0.0526f,0.0381f);//数据根据UI图修改
		m_IconRect=RectChange(m_IconRect);

		m_ExtractRect = new Rect (0.8809f,0.8088f,0.0526f,0.0381f);//数据根据UI图修改
		m_ExtractRect=RectChange(m_ExtractRect);


		desktopChildSystem.AddApp1(m_DesktopParent.appMono.stayBG[8],m_DesktopParent.appMono.selectionBG[8]);
		desktopChildSystem.AddApp2 (new Note2_App(desktopChildSystem));

	}
	public override string ToString()
	{
		return "UDisk_App";
	}
}

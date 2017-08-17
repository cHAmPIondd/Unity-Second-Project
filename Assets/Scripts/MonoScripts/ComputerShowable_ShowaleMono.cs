using UnityEngine;
using System.Collections;

public class ComputerShowable_ShowaleMono : ShowableMono {

	[SerializeField] private GameObject m_GameIcon;
	[SerializeField] private AppMono m_AppMono;
	public override void Show()
	{
		m_GameIcon.SetActive (true);
		((RecycleBin_App)m_AppMono.desktopChildSystem.appList [4]).desktopChildSystem.appList[0].isRegister=true;//appList[4]为桌面垃圾桶
	}
}

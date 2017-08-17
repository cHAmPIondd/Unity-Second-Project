using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComputerMono : AppMono {

	protected List<GameObject> m_USBUsedList;
	public List<GameObject> USBUsedList
	{
		get{ return m_USBUsedList; }
	}
	protected List<App> m_USBAppList;
	public List<App> USBAppList
	{
		get{ return m_USBAppList; }
	}
	protected App m_UDiskApp;
	public App UDiskApp{ get{ return m_UDiskApp;}}

	void Awake () 
	{		
		gameObject.AddComponent<DesktopChildSystem> ();
		m_DesktopChildSystem = GetComponent<DesktopChildSystem> ();
		m_DesktopChildSystem.appMono = this;
		m_USBUsedList = new List<GameObject> ();
		m_USBAppList = new List<App> ();
	}
	void Start()
	{
		m_DesktopChildSystem.AddApp1(m_StayBG[0],m_SelectionBG[0]);
		m_DesktopChildSystem.AddApp2 (new MyComputer_App(m_DesktopChildSystem));

		m_DesktopChildSystem.AddApp1(m_StayBG[1],m_SelectionBG[1]);
		m_DesktopChildSystem.AddApp2 (new Internet_App(m_DesktopChildSystem));

		m_DesktopChildSystem.AddApp1(m_StayBG[2],m_SelectionBG[2]);
		m_DesktopChildSystem.AddApp2 (new Note_App(m_DesktopChildSystem));

		m_DesktopChildSystem.AddApp1(m_StayBG[3],m_SelectionBG[3]);
		m_DesktopChildSystem.AddApp2 (new Email_App(m_DesktopChildSystem));

		m_DesktopChildSystem.AddApp1 (m_StayBG[4],m_SelectionBG[4]);
		m_DesktopChildSystem.AddApp2 (new RecycleBin_App(m_DesktopChildSystem));

		m_DesktopChildSystem.AddApp1(m_StayBG[5],m_SelectionBG[5]);
		m_DesktopChildSystem.AddApp2 (new VS_App(m_DesktopChildSystem));

		m_DesktopChildSystem.AddApp1(m_StayBG[6],m_SelectionBG[6]);
		m_DesktopChildSystem.AddApp2 (new Music_App(m_DesktopChildSystem));

		m_DesktopChildSystem.AddApp1(m_StayBG[9],m_SelectionBG[9]);
		m_DesktopChildSystem.AddApp2 (new SpaceInvader_App(m_DesktopChildSystem));

		m_DesktopChildSystem.AddApp1(m_StayBG[7],m_SelectionBG[7]);
		m_UDiskApp = new UDisk_App (m_DesktopChildSystem);
		foreach (GameObject tempGO in m_USBUsedList) //先插入u盘再打开电脑时
		{
			if (tempGO.name == "UDisk") 
			{
				m_USBAppList.Add (m_UDiskApp);
			}
		}
	}
	
	void Update () 
	{
		m_DesktopChildSystem. UpdateDesktopSystem ();
		if (m_DesktopChildSystem.currentUsingApp == null) //桌面update
		{
			m_DesktopChildSystem.currentStayApp = null;
			if (Input.GetMouseButtonDown (0)) {//监听鼠标点击鼠标事件
				//U盘
				for (int i=0;i<m_USBAppList.Count;i++) 
				{
					if (m_USBAppList[i].IsMouseStay (m_USBAppList[i].iconRect)) 
					{
						m_DesktopChildSystem.currentStayApp = m_USBAppList [i];
						StartCoroutine (m_USBAppList[i].Open ());
					}
					if (m_USBAppList[i].IsMouseStay (((UDisk_App)m_USBAppList[i]).extractRect)) 
					{
						m_USBUsedList[0].GetComponent<Rigidbody> ().isKinematic = false;
						GameObject.Find ("Computer/Desktop/UDisk_Icon").SetActive (false);
						m_USBAppList.Clear();
						m_USBUsedList.Remove (m_USBUsedList[i]);
						break;//目前只鞥插入一个物体且是U盘
					}
				}
			}
		} 
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DesktopChildSystem : MonoBehaviour {

	public AppMono appMono{ get; set;}
	//子系统
	private List<App> m_AppList;
	public List<App> appList
	{
		get{ return m_AppList;}
	}

	private List<App> m_AppList2;
	public List<App> appList2
	{
		get{return m_AppList2;}
	}

	private App m_CurrentUsingApp;
	public App currentUsingApp
	{
		get{return m_CurrentUsingApp;}
		set{m_CurrentUsingApp=value;}
	}
	private App m_CurrentStayApp;
	public App currentStayApp
	{
		get{return m_CurrentStayApp;}
		set{m_CurrentStayApp=value;}
	}
	private App m_SelectedApp;
	private float m_LastClickTime=-10f;

	private List<GameObject> m_StayBG;
	public List<GameObject> stayBG{get{return m_StayBG;}}
	private List<GameObject> m_SelectionBG;
	public List<GameObject> selectionBG{get{return m_SelectionBG;}}


	public DesktopChildSystem()
	{
		m_AppList = new List<App> ();
		m_AppList2 = new List<App> ();
		m_StayBG=new List<GameObject>();
		m_SelectionBG = new List<GameObject> ();
	}

	public void UpdateDesktopSystem()//当使用此软件时Update
	{
		if (m_CurrentUsingApp == null)
		{
			//普通App
			m_CurrentStayApp = null;
			foreach(App app in m_AppList)//监听鼠标是否有停留在应用上
			{
				m_StayBG [app.appID].SetActive (false);
				if (app.IsMouseStay (app.iconRect)) 
				{
					m_CurrentStayApp = app;
					m_StayBG [app.appID].SetActive (true);
					break;
				}
			}
			if (Input.GetMouseButtonDown (0)) {//监听鼠标点击鼠标事件 
				if (m_CurrentStayApp != null) 
				{
					if (m_SelectedApp == m_CurrentStayApp) 
					{
						if (m_LastClickTime + 0.5f > Time.time) 
						{
							m_StayBG [m_SelectedApp.appID].SetActive (false);
							m_SelectionBG [m_SelectedApp.appID].SetActive (false);
							m_SelectedApp = null;
							if (m_CurrentStayApp.isRegister) {
								StartCoroutine (m_CurrentStayApp.Open ());
							}
						}
					} 
					else 
					{
						if (m_SelectedApp != null)
							m_SelectionBG [m_SelectedApp.appID].SetActive (false);
						m_SelectedApp = m_CurrentStayApp;
						m_SelectionBG [m_SelectedApp.appID].SetActive (true);
					}
					m_LastClickTime = Time.time;
				} 
				else 
				{
					if (m_SelectedApp != null)
					{
						m_SelectionBG [m_SelectedApp.appID].SetActive (false);
						m_SelectedApp = null;
					}
				}
			}

			//单击App
			if (Input.GetMouseButtonDown (0)) {//监听鼠标点击鼠标事件 
				foreach (App app in m_AppList2)
				{
					if (app.IsMouseStay (app.iconRect)) 
					{
						StartCoroutine (app.Open ());
						currentUsingApp = app;
					}
				}
			}
		}
		else //应用update
		{
			if (m_CurrentUsingApp.isOpen) 
			{
				if (m_CurrentUsingApp as DesktopApp==null||((DesktopApp)m_CurrentUsingApp).desktopChildSystem.currentUsingApp==null)//Debug只适用于双重桌面系统
				{
					if (Input.GetMouseButtonDown (0)) {
						if (m_CurrentUsingApp.IsMouseStay (m_CurrentUsingApp.closeRect)) {//关闭软件
							StartCoroutine (m_CurrentUsingApp.Close ());	
						}
					}
				}
			}
			m_CurrentUsingApp.UpdateApp ();
		}
	}

	public void AddApp1(GameObject stayBG,GameObject seletionBG)
	{
		m_StayBG.Add (stayBG);
		m_SelectionBG.Add (seletionBG);
	}
	public void AddApp2(App app)
	{
		m_AppList.Add (app);
	}
	public void AddApp3(App app)
	{
		m_AppList2.Add (app);
	}
	public bool IsCanEsc()
	{
		if (currentUsingApp!=null)
			return currentUsingApp.IsCanEsc ();
		return false;
	}
}

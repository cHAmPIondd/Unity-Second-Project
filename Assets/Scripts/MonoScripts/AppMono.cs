using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AppMono : ControlMono {

	[SerializeField] protected Transform m_DesktopLeftDown;
	[SerializeField] protected Transform m_DesktopRightUp;
	[SerializeField] protected Camera m_Camera;
	[SerializeField] protected GameObject[] m_StayBG;
	[SerializeField] protected GameObject[] m_SelectionBG;
	[SerializeField] protected GameObject m_ControlUI;



	public Transform desktopLeftDown{ get{ return m_DesktopLeftDown;}}
	public Transform desktopRightUp{ get{ return m_DesktopRightUp;}}
	public GameObject[] stayBG{get{ return m_StayBG;}}
	public GameObject[] selectionBG{get{ return m_SelectionBG; }}

	protected Rect m_AppRect;//电脑桌面与app界面的大小
	public Rect appRect
	{ 
		get{ return m_AppRect; }
	} 



	protected DesktopChildSystem m_DesktopChildSystem;
	public DesktopChildSystem desktopChildSystem{get{ return m_DesktopChildSystem;}}
	void Awake()
	{
		gameObject.AddComponent<DesktopChildSystem> ();
		m_DesktopChildSystem = GetComponent<DesktopChildSystem> ();
		m_DesktopChildSystem.appMono = this;
	}
	void OnEnable()
	{
		Vector2 appLeftDown=m_Camera.WorldToScreenPoint(m_DesktopLeftDown.position);
		Vector2 appRightUp=m_Camera.WorldToScreenPoint(m_DesktopRightUp.position);
		m_AppRect = new Rect (appLeftDown.x,appLeftDown.y,appRightUp.x-appLeftDown.x,appRightUp.y-appLeftDown.y);
		Cursor.visible = true;
		m_CenterSight.SetActive (false);
		m_ControlUI.SetActive (true);
	}
	void OnDisable()
	{
		Cursor.visible = false;
		m_CenterSight.SetActive (true);
		if(m_ControlUI!=null)
			m_ControlUI.SetActive (false);
	}

	void Update () 
	{
		m_DesktopChildSystem. UpdateDesktopSystem ();
	}
	public override bool IsCanEsc()
	{
		return m_DesktopChildSystem.IsCanEsc ();
	}
}

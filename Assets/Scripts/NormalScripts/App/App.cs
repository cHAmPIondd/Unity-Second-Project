using UnityEngine;
using System.Collections;

public class App  {
	protected DesktopChildSystem m_DesktopParent;//使用该App的桌面系统

	protected int m_AppID;
	public int appID
	{
		get{ return m_AppID;}
	}
	protected bool m_IsRegister;
	public bool isRegister
	{
		get{ return m_IsRegister;}
		set{ m_IsRegister = value;}
	}
		
	//图标按钮
	protected Rect m_IconRect;
	public Rect iconRect
	{
		get{return m_IconRect;}
	}
	//app界面
	protected Transform m_AppTransform;

	//
	protected Rect m_CurrentAppRect;
	protected bool m_IsOpen;
	public bool isOpen
	{
		get{return m_IsOpen;}
	}

	//关闭按钮
	protected Rect m_CloseRect;
	public Rect closeRect
	{
		get{return m_CloseRect;}
	}


	protected float m_MaxScale;

	public App(){}
	public App(DesktopChildSystem DeskChiSy)
	{
		m_MaxScale = DeskChiSy.appMono.desktopRightUp.parent.localScale.x;
		m_AppID = DeskChiSy.appList.Count;
		m_DesktopParent = DeskChiSy;
		m_CloseRect = new Rect (0.8917f,0.9198f,0.0584f,0.0588f);
		m_CloseRect = RectChange (m_CloseRect);
		if(m_DesktopParent.appMono as ComputerMono!=null)
			m_AppTransform = GameObject.Find ("Computer/App/"+ToString()).transform;
		else
			m_AppTransform = GameObject.Find ("ComputerCamera/Phone/App/"+ToString()).transform;
	}
	public virtual void UpdateApp()//当使用此软件时Update
	{
		
	}
	public virtual IEnumerator Open() // 打开软件
	{  
		m_DesktopParent.currentUsingApp =m_DesktopParent.currentStayApp;
		m_CurrentAppRect=new Rect(m_IconRect.x+m_IconRect.width/2,m_IconRect.y+m_IconRect.height/2,0,0);
		for (float timer = 0.8f; timer >= 0; timer -= Time.deltaTime) 
		{
			m_CurrentAppRect.size=Vector2.Lerp(m_CurrentAppRect.size,m_DesktopParent.appMono.appRect.size,10*Time.deltaTime);

			m_AppTransform.localScale = Vector3.Lerp(m_AppTransform.localScale,Vector3.one*m_MaxScale,10*Time.deltaTime);
			yield return 0;  
		}
		m_CurrentAppRect = m_DesktopParent.appMono.appRect;
		m_AppTransform.localScale=Vector3.one*m_MaxScale;
		m_IsOpen = true;
		OpenOperation ();
	}  
	public virtual IEnumerator Close()//关闭软件
	{
		m_IsOpen = false;
		for (float timer = 0.8f; timer >= 0; timer -= Time.deltaTime) 
		{
			m_CurrentAppRect.position=Vector2.Lerp(m_CurrentAppRect.position,new Vector2(m_IconRect.x+m_IconRect.width/2,m_IconRect.y+m_IconRect.height	/2),10*Time.deltaTime);
			m_CurrentAppRect.size=Vector2.Lerp(m_CurrentAppRect.size,Vector2.zero,10*Time.deltaTime);

			m_AppTransform.localScale = Vector3.Lerp(m_AppTransform.localScale,Vector3.zero,10*Time.deltaTime);
			yield return 0;  


		}
		m_DesktopParent.currentUsingApp = null;
		CloseOperation ();
	}
	protected virtual void OpenOperation()
	{
	}
	protected virtual void CloseOperation()
	{
	}
	public override string ToString()
	{
		return "App";
	}
	public bool IsMouseStay(Rect Rect)
	{
		if (Rect.x < Input.mousePosition.x
			&& Rect.y < Input.mousePosition.y
			&& Rect.x + Rect.width > Input.mousePosition.x
			&& Rect.y + Rect.height > Input.mousePosition.y)
			return true;
		return false;
	}
	protected Rect RectChange(Rect Rect)
	{
		float appRectX = m_DesktopParent.appMono.appRect.x;
		float appRectY = m_DesktopParent.appMono.appRect.y;
		float appRectWidth = m_DesktopParent.appMono.appRect.width;
		float appRectHeight = m_DesktopParent.appMono.appRect.height;
		return new Rect (appRectX+Rect.x * appRectWidth, 
						appRectY+Rect.y * appRectHeight,
						Rect.width * appRectWidth,
						Rect.height * appRectHeight);
	}			
	public virtual bool IsCanEsc()
	{
		return false;
	}
}

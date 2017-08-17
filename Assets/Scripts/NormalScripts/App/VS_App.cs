using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class VS_App :DesktopApp {
	private RectTransform m_SignTransform;

	private int m_LineNum=0;

	private int m_MaxLineNum=20;
	private InputField[] m_InputField;
	private RectTransform[] m_InputFieldTranform;
	private RectTransform m_ErrorInfoTransform;
	private Text m_ErrorText;

	private int m_CaretPosition;
	private bool m_IsCancalSeletion;
	private bool m_IsLastAnyKeyDown;

	private Rect m_RunRect;

	private App m_HelpApp;


	private float m_SignHeight;
	private float m_ErrorInfoY;
	private float m_InputFieldX;
	private float m_InputFieldY;

	public VS_App(DesktopChildSystem DeskChiSy)
		:base(DeskChiSy)
	{
		m_IsRegister = true;

		if (DeskChiSy.appMono as ComputerMono != null) 
		{
			m_SignHeight=0.0303f;
        	m_ErrorInfoY=0.0704f;
        	m_InputFieldX=0.027f;
         	m_InputFieldY=0.8257f;

			desktopChildSystem.AddApp1 (m_DesktopParent.appMono.stayBG [11], m_DesktopParent.appMono.selectionBG [11]);//电脑里第十二个App是VS
			desktopChildSystem.AddApp3 (new Help_App (desktopChildSystem));

			m_IconRect = new Rect (0.1387f, 0.8208f, 0.1016f, 0.1447f);//数据根据UI图修改
			m_RunRect = new Rect (0.0396f, 0.8752f, 0.0365f, 0.0682f);//数据根据UI图修改
			m_IconRect = RectChange (m_IconRect);
			m_RunRect = RectChange (m_RunRect);

			m_CloseRect = new Rect (0.9441f, 0.9456f, 0.0377f, 0.0544f);
			m_CloseRect = RectChange (m_CloseRect);

			m_InputField = new InputField[m_MaxLineNum];
			m_InputFieldTranform = new RectTransform[m_MaxLineNum];
			for (int i = 0; i < m_MaxLineNum; i++) {
				m_InputField [i] = GameObject.Find ("Canvas/Computer/VS_App/InputField (" + i + ")").GetComponent<InputField> ();
				m_InputFieldTranform [i] = m_InputField [i].GetComponent<RectTransform> ();
			}	
			m_SignTransform = GameObject.Find ("Canvas/Computer/VS_App/VS_Sign").GetComponent<RectTransform> ();
			m_ErrorInfoTransform = GameObject.Find ("Canvas/Computer/VS_App/ErrorText").GetComponent<RectTransform> ();
			m_ErrorText = m_ErrorInfoTransform.GetComponent<Text> ();
		} 
		else 
		{
			m_SignHeight=0.0316f;
        	m_ErrorInfoY=0.03f;
			m_InputFieldX=0.07f;
        	m_InputFieldY=0.814f;
			desktopChildSystem.AddApp1 (m_DesktopParent.appMono.stayBG [0], m_DesktopParent.appMono.selectionBG [0]);//手机里第一个App是VS
			desktopChildSystem.AddApp3 (new Help_App (desktopChildSystem));

			m_IconRect = new Rect (0.6614f, 0.2165f, 0.1978f, 0.1080f);//数据根据UI图修改
			m_RunRect = new Rect (0.8392f, 0.9194f, 0.0778f, 0.0368f);//数据根据UI图修改
			m_IconRect = RectChange (m_IconRect);
			m_RunRect = RectChange (m_RunRect);

			m_CloseRect = new Rect (0, 0.9194f, 0.1f, 0.0368f);
			m_CloseRect = RectChange (m_CloseRect);

			m_InputField = new InputField[m_MaxLineNum];
			m_InputFieldTranform = new RectTransform[m_MaxLineNum];
			for (int i = 0; i < m_MaxLineNum; i++) 
			{
				m_InputField [i] = GameObject.Find ("Canvas/Phone/VS_App/InputField (" + i + ")").GetComponent<InputField> ();
				m_InputFieldTranform [i] = m_InputField [i].GetComponent<RectTransform> ();
			}	
			m_SignTransform = GameObject.Find ("Canvas/Phone/VS_App/VS_Sign").GetComponent<RectTransform> ();
			m_ErrorInfoTransform = GameObject.Find ("Canvas/Phone/VS_App/ErrorText").GetComponent<RectTransform> ();
			m_ErrorText = m_ErrorInfoTransform.GetComponent<Text> ();
		}
	}

	public override void UpdateApp()
	{
		base.UpdateApp ();
		if (isOpen)
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				for (m_LineNum=0; m_LineNum < m_MaxLineNum; m_LineNum++) 
				{
					if (IsMouseStay(new Rect((Vector2)m_InputFieldTranform[m_LineNum].position-m_InputFieldTranform[m_LineNum].sizeDelta/2f,m_InputFieldTranform[m_LineNum].sizeDelta))) 
					{
						m_CaretPosition = m_InputField [m_LineNum].caretPosition;
						break;
					}
				}
				if(IsMouseStay(m_RunRect))
				{
					Run ();
				}
			}
			if (m_LineNum < m_MaxLineNum) 
			{
				if (m_IsCancalSeletion) {//取消当前选区
					m_InputField [m_LineNum].ActivateInputField ();
					m_InputField [m_LineNum].caretPosition = m_CaretPosition;
					m_IsCancalSeletion = false;
				}
				if (m_IsLastAnyKeyDown) {
					m_CaretPosition = m_InputField [m_LineNum].caretPosition;
					m_IsLastAnyKeyDown = false;
				}
				if (Input.GetKeyDown (KeyCode.F5)) {
					Run ();
				}
				if (Input.GetKeyDown (KeyCode.Return)) {
					if (m_InputField [m_MaxLineNum - 1].text == "" && m_LineNum < m_MaxLineNum - 1) {
						for (int i = m_MaxLineNum - 1; i > m_LineNum + 1; i--) {
							m_InputField [i].text = m_InputField [i - 1].text;
						}
						m_InputField [m_LineNum + 1].text = m_InputField [m_LineNum].text.Substring
						(m_CaretPosition, m_InputField [m_LineNum].text.Length - m_CaretPosition);
						m_InputField [m_LineNum].text = m_InputField [m_LineNum].text.Substring (0, m_CaretPosition);
						m_LineNum++;
						m_CaretPosition = 0;
					} 
					m_InputField [m_LineNum].ActivateInputField ();
					m_IsCancalSeletion = true;
				}
				if (Input.GetKeyDown (KeyCode.Backspace)) {
					if (m_CaretPosition <= 0 && m_LineNum > 0) {
						for (int i = m_LineNum + 1; i < m_MaxLineNum - 1; i++) {
							m_InputField [i].text = m_InputField [i + 1].text;
						}
						m_CaretPosition = m_InputField [m_LineNum - 1].text.Length;
						m_InputField [m_LineNum - 1].text = m_InputField [m_LineNum - 1].text + m_InputField [m_LineNum].text;
						if (m_LineNum < m_MaxLineNum - 1)
							m_InputField [m_LineNum].text = m_InputField [m_LineNum + 1].text;
						m_InputField [m_MaxLineNum - 1].text = "";
						m_LineNum--;
						m_InputField [m_LineNum].ActivateInputField ();
						m_IsCancalSeletion = true;	
					} 		
				}
				if (Input.GetKeyDown (KeyCode.UpArrow)) {
					if (m_LineNum > 0) {
						m_InputField [m_LineNum].DeactivateInputField ();
						m_LineNum--;
						m_InputField [m_LineNum].ActivateInputField ();
						m_IsCancalSeletion = true;
					}
				}
				if (Input.GetKeyDown (KeyCode.DownArrow)) {
					if (m_LineNum < m_MaxLineNum - 1) {
						m_InputField [m_LineNum].DeactivateInputField ();
						m_LineNum++;
						m_InputField [m_LineNum].ActivateInputField ();
						m_IsCancalSeletion = true;
					}
				}
				if (Input.GetKeyDown (KeyCode.RightArrow)) {
					if (m_CaretPosition >= m_InputField [m_LineNum].text.Length) {
						m_InputField [m_LineNum].DeactivateInputField ();
						m_LineNum++;
						m_CaretPosition = 0;
						m_InputField [m_LineNum].ActivateInputField ();
						m_IsCancalSeletion = true;
					}
					
				}
				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					if (m_CaretPosition <= 0 && m_LineNum > 0) {
						m_InputField [m_LineNum].DeactivateInputField ();
						m_LineNum--;
						m_CaretPosition = m_InputField [m_LineNum].text.Length;
						m_InputField [m_LineNum].ActivateInputField ();
						m_IsCancalSeletion = true;
					}
				}
				if (Input.anyKeyDown|| Input.GetKey (KeyCode.Backspace) || Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow)) 
				{
					m_IsLastAnyKeyDown = true;
				}
			}
		}//isOpen
		//标记栏
		if (m_LineNum < m_MaxLineNum) 
		{
			m_SignTransform.sizeDelta =	new Vector2 (m_CurrentAppRect.width, m_CurrentAppRect.height);
			m_SignTransform.position = new Vector2 (m_CurrentAppRect.x,m_CurrentAppRect.y - m_LineNum * m_SignHeight * m_CurrentAppRect.height)+m_SignTransform.sizeDelta/2f;
		}
		else
			m_SignTransform.sizeDelta =	Vector2.zero;

	}
	public override IEnumerator Open() // 打开软件
	{  
		m_DesktopParent.currentUsingApp =m_DesktopParent.currentStayApp;
		m_CurrentAppRect=new Rect(m_IconRect.x+m_IconRect.width/2,m_IconRect.y+m_IconRect.height/2,0,0);
		for (float timer = 0.8f; timer >= 0; timer -= Time.deltaTime) 
		{
			m_CurrentAppRect.position=Vector2.Lerp(m_CurrentAppRect.position,m_DesktopParent.appMono.appRect.position,10*Time.deltaTime);
			m_CurrentAppRect.size=Vector2.Lerp(m_CurrentAppRect.size,m_DesktopParent.appMono.appRect.size,10*Time.deltaTime);
			//VS窗口
			m_AppTransform.localScale = Vector3.Lerp(m_AppTransform.localScale,Vector3.one*m_MaxScale,10*Time.deltaTime);
			//错误信息
			m_ErrorInfoTransform.sizeDelta=new Vector2(m_CurrentAppRect.width,m_CurrentAppRect.height * m_SignHeight);
			m_ErrorInfoTransform.position = new Vector2 (m_CurrentAppRect.x,m_CurrentAppRect.y + m_ErrorInfoY* m_CurrentAppRect.height)+m_ErrorInfoTransform.sizeDelta/2f;

			//代码行
			for (int i = 0; i < m_MaxLineNum; i++) 
			{
				m_InputFieldTranform[i].sizeDelta = new Vector2 (m_CurrentAppRect.width, m_CurrentAppRect.height * m_SignHeight);
				m_InputFieldTranform[i].position = new Vector2 (m_CurrentAppRect.x + m_InputFieldX * m_CurrentAppRect.width,
					m_CurrentAppRect.y + m_InputFieldY * m_CurrentAppRect.height - i* m_SignHeight * m_CurrentAppRect.height)
				+ m_InputFieldTranform[i].sizeDelta / 2f;
			}
			yield return 0;  
		}
		m_CurrentAppRect = m_DesktopParent.appMono.appRect;
		m_AppTransform.localScale=Vector3.one*m_MaxScale;
		m_IsOpen = true;
		if(m_LineNum<m_MaxLineNum)
			m_InputField[m_LineNum].ActivateInputField ();
	}
	public override IEnumerator Close()//关闭软件
	{
		m_IsOpen = false;
		m_CurrentAppRect=new Rect(m_IconRect.x+m_IconRect.width/2,m_IconRect.y+m_IconRect.height/2,0,0);
		for (float timer = 0.8f; timer >= 0; timer -= Time.deltaTime) 
		{
			m_CurrentAppRect.size=Vector2.Lerp(m_CurrentAppRect.size,Vector2.zero,10*Time.deltaTime);
			//VS窗口
			m_AppTransform.localScale = Vector3.Lerp(m_AppTransform.localScale,Vector3.zero,10*Time.deltaTime);
			//标记行
			if (m_LineNum < m_MaxLineNum) 
			{
				m_SignTransform.sizeDelta =	new Vector2 (m_CurrentAppRect.width, m_CurrentAppRect.height);
				m_SignTransform.position = new Vector2 (m_CurrentAppRect.x, m_CurrentAppRect.y + m_LineNum * m_SignHeight * m_CurrentAppRect.height) + m_SignTransform.sizeDelta / 2f;
			}
			//错误信息
			m_ErrorInfoTransform.sizeDelta=new Vector2(m_CurrentAppRect.width,m_CurrentAppRect.height * m_SignHeight);
			m_ErrorInfoTransform.position = new Vector2 (m_CurrentAppRect.x,m_CurrentAppRect.y +m_ErrorInfoY * m_CurrentAppRect.height)+m_ErrorInfoTransform.sizeDelta/2f;
			//代码行
			for (int i = 0; i < m_MaxLineNum; i++) 
			{
				m_InputFieldTranform [i].sizeDelta = new Vector2 (m_CurrentAppRect.width, m_CurrentAppRect.height * m_SignHeight);
				m_InputFieldTranform [i].position = new Vector2 (m_CurrentAppRect.x + m_InputFieldX* m_CurrentAppRect.width,
					m_CurrentAppRect.y + m_InputFieldY * m_CurrentAppRect.height - i * m_SignHeight * m_CurrentAppRect.height)
				+ m_InputFieldTranform [i].sizeDelta / 2f;
			}
			yield return 0;  
		}
		m_DesktopParent.currentUsingApp =null;
	}
	private void Run()
	{
		List<Code> executeCode=new List<Code>();
		string errorInfomation=null;
		int currentLineNum = 0;
		foreach(InputField input in m_InputField)
		{
			string text = input.text.ToUpper();
			currentLineNum++;
			if (text == null)
				break;
			if (text.Length == 0)//空行
				continue;
			if (text [text.Length - 2] != ')' || text [text.Length - 1] != ';') {//格式错误
				errorInfomation = "格式错误"+"（在第"+currentLineNum+"行）";
				break;
			}
			int location = 0;
			foreach (char ch in text) 
			{
				if (ch == '(') {
					break;
				}
				location++;
			}

			if (location == text.Length) {//格式错误
				errorInfomation="格式错误"+"（在第"+currentLineNum+"行）";
				break;
			}
			string codeName=text.Substring (0, location)+"_CODE";
			Code currentCode = null;
			foreach (string code in CodeLibrary.instance.codeList) 
			{
				if(code.Equals(codeName))
				{
					currentCode = Code.CodeFactory(code);
					break;
				}
			}
			if (currentCode == null) {//函数名错误
				errorInfomation="函数名错误"+"（在第"+currentLineNum+"行）";
				break;
			}
			string parameterInfo;
			if(text.Length - 3-location==0)
				parameterInfo=currentCode.IsLegalParameter("");
			else
				parameterInfo=currentCode.IsLegalParameter (text.Substring (location+1, text.Length - 3-location));
			if (parameterInfo != "")//参数错误
			{
				errorInfomation = parameterInfo+"（在第"+currentLineNum+"行）";
			}
			else //正确输入
				executeCode.Add (currentCode);
		}
		if (errorInfomation != null) {
			m_ErrorText.text = errorInfomation;
		} else {//执行代码
			m_ErrorText.text = "";
			if (executeCode.Count != 0) {
				m_DesktopParent.appMono.enabled = false;
				foreach (Code code in executeCode) {
					code.Init ();
				}
				foreach (Code code in executeCode) {
					GameProgressManager.instance.AddAnimation (new GameProgressManager.animation (code.Function));
				}
				if (m_DesktopParent.appMono as ComputerMono != null) 
				{
					GameProgressManager.instance.AddAnimation (new GameProgressManager.animation (PlayerControl.instance.ReturnAnimation));
				} 
				else 
				{
					GameProgressManager.instance.AddAnimation (new GameProgressManager.animation (PlayerControl.instance.ReturnPhoneAnimation));
					PlayerControl.instance.LeavePhoneAnimation ();
				}
			}
		}
	}


	public override string ToString()
	{
		return "VS_App";
	}
}
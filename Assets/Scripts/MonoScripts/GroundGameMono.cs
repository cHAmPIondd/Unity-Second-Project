using UnityEngine;
using System.Collections;

public class GroundGameMono : ControlMono {
	[SerializeField] private Camera m_Camera;
	[SerializeField] private Transform m_Transform;
	[SerializeField] private Vector3 m_MoveVector3;
	[SerializeField] private SpriteRenderer[] m_TimeBG;
	[SerializeField] private Transform m_CrossTransform;
	[SerializeField] private GameObject m_WrongGameObject;
	[SerializeField] private AudioClip m_CrossRotateClip;
	[SerializeField] private AudioClip m_WrongClip;
	[SerializeField] private AudioClip m_DidaClip;
	private AudioSource m_AudioSource;
	private bool m_IsNotCan;
	private Vector3 m_OriginalPosition;
	private IDMono[] m_IDMonoArray;
	private IDMono m_LastIDMono;
	private bool m_IsTimeLoop;
	private bool m_IsOpen;
	void Awake()
	{
		m_IDMonoArray = GetComponentsInChildren<IDMono> ();
		m_OriginalPosition = m_Transform.position;
		m_AudioSource = GetComponent<AudioSource> ();
	}
	void Update () {
		if (!m_IsNotCan) {
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit hitObject;
				if (Physics.Raycast (m_Camera.ScreenPointToRay (Input.mousePosition), out hitObject, 1000, 1 << 10)) {
					if (m_LastIDMono != null)
						SwitchPosition (m_LastIDMono, hitObject.transform.GetComponent<IDMono> ());
					else 
					{
						m_LastIDMono = hitObject.transform.GetComponent<IDMono> ();
						m_LastIDMono.GetComponent<SpriteRenderer> ().color = new Color (1, 0.7f, 0.7f, 1);//选中反馈 变灰
					}
				}
			}
		}
		if (!m_IsTimeLoop&&!m_IsOpen) {
			StartCoroutine ("TimeLoop");
		}
	}
	void OnEnable()
	{
		Cursor.visible = true;
		m_CenterSight.SetActive (false);
		if (!m_IsTimeLoop) 
		{
			StartCoroutine ("TimeLoop");
			m_IsTimeLoop = true;
		}
	}
	void OnDisable()
	{
		Cursor.visible = false;
		m_CenterSight.SetActive (true);
	//	for(int i=0;i<m_TimeBG.Length;i++)
	//		m_TimeBG [i].color = Color.white;
		m_WrongGameObject.SetActive (false);
	}

	private IEnumerator TimeLoop()
	{
		int num = -1;
		while (true) 
		{	
			num++;
			yield return new WaitForSeconds(1);
			//计时结束
			if (num == 9) {
				//	for(int i=0;i<m_TimeBG.Length;i++)
				//		m_TimeBG [i].color = Color.white;
				num = -1;
				m_IsNotCan = true;
				//取消选中
				if (m_LastIDMono != null) {
					m_LastIDMono.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);//取消选中反馈 变白
					m_LastIDMono = null;
				}
				IDMono[] tempGOArray = new IDMono[5];
				int num1 = 0;
				//寻找旋转物
				foreach (IDMono id in m_IDMonoArray) {
					if (id.ID / 100 == 2 || id.ID / 10 % 10 == 2) {
						tempGOArray [num1] = id;
						id.transform.parent = m_CrossTransform;
						num1++;
					}
				}
				//开始旋转!
				float target = m_CrossTransform.localRotation.eulerAngles.z + 90;
				m_AudioSource.clip = m_CrossRotateClip;
				m_AudioSource.Play ();
				for (float i = 2; i >= 0; i -= Time.deltaTime) {
					m_CrossTransform.Rotate (new Vector3 (0, 0, Time.deltaTime * 45));
					yield return 0;
				}
				m_AudioSource.Stop ();
				m_CrossTransform.localRotation = Quaternion.Euler (new Vector3 (0, 0, target));
				//处理旋转物
				foreach (IDMono id in tempGOArray) {
					id.transform.parent = transform;
					switch (id.ID / 10) {
					case 12:
						id.ID = 230 + id.ID % 10;
						break;
					case 21:
						id.ID = 120 + id.ID % 10;
						break;
					case 22:
						id.ID = 220 + id.ID % 10;
						break;
					case 23:
						id.ID = 320 + id.ID % 10;
						break;
					case 32:
						id.ID = 210 + id.ID % 10;
						break;
					default:
						Debug.Log ("Bug");
						break;
					}
				}
				yield return new WaitForSeconds (0.5f);
				//判断是否胜利
				if (CheckVictory ()) {
					if (!m_IsOpen) {
						GameProgressManager.instance.TipAnimation ("似乎触动了什么东西", true);
						m_Transform.GetComponent<AudioSource> ().Play ();
						for (float i = 0.7f; i >= 0; i -= Time.deltaTime) {
							m_Transform.position = Vector3.Lerp (m_Transform.position, m_OriginalPosition + m_MoveVector3, 5 * Time.deltaTime);
							yield return 0;
						}
						m_IsOpen = true;
						m_IsNotCan = false;
						m_IsTimeLoop = false;
						StopCoroutine ("TimeLoop");
					}
				} else if (m_IsOpen) {
					GameProgressManager.instance.TipAnimation ("似乎触动了什么东西", true);
					m_Transform.GetComponent<AudioSource> ().Play ();
					for (float i = 0.7f; i >= 0; i -= Time.deltaTime) {
						m_Transform.position = Vector3.Lerp (m_Transform.position, m_OriginalPosition, 5 * Time.deltaTime);
						yield return 0;
					}
					m_IsOpen = false;
				} else {
					m_AudioSource.clip = m_WrongClip;
					m_AudioSource.Play ();
					m_WrongGameObject.SetActive (true);
					yield return new WaitForSeconds (0.1f);
					m_WrongGameObject.SetActive (false);
					yield return new WaitForSeconds (0.1f);
					m_WrongGameObject.SetActive (true);
					yield return new WaitForSeconds (0.1f);
					m_WrongGameObject.SetActive (false);
					m_AudioSource.Stop ();
				}
				m_IsNotCan = false;
				m_AudioSource.clip=m_DidaClip;
				m_IsTimeLoop = false;
				StopCoroutine ("TimeLoop");
			} 
			else
			{
				m_AudioSource.Play ();
				//m_TimeBG [num].color = new Color (0.8f, 0.8f, 0.8f, 1);
			}
		}


	}

	private void SwitchPosition(IDMono first,IDMono second)
	{
		//取消选中
		m_LastIDMono.GetComponent<SpriteRenderer> ().color = new Color (1,1,1,1);//取消选中反馈 变白
		m_LastIDMono = null;

		//交换位置
		if(Mathf.Abs(first.ID/10-second.ID/10)==10||Mathf.Abs(first.ID/10-second.ID/10)==1)//判断相邻
		{
			int tempInt = first.ID;
			first.ID = second.ID / 10 * 10 + first.ID % 10;
			second.ID = tempInt / 10 * 10 + second.ID % 10;
			Vector3 tempV3 = first.transform.position;
			first.transform.position = second.transform.position;
			second.transform.position =tempV3;
		}
	}
	private bool CheckVictory()
	{
		int[] num1=new int[3]{0,0,0};
		bool horizontal=true;
		foreach(IDMono temp in m_IDMonoArray)
		{
			int tempNum = temp.ID / 100;//行数
			if (num1 [tempNum-1] == 0)
				num1 [tempNum-1] = temp.ID % 10;
			else if (num1 [tempNum-1] != temp.ID % 10) //不同行
			{
				horizontal = false;
				break;
			}
		}
		int[] num2=new int[3]{0,0,0};
		bool vertical=true;
		foreach(IDMono temp in m_IDMonoArray)
		{
			int tempNum = temp.ID / 10%10;//列数
			if (num2 [tempNum-1] == 0)
				num2 [tempNum-1] = temp.ID % 10;
			else if (num2[tempNum-1] != temp.ID % 10) //不同列
			{
				vertical = false;
				break;
			}
		}
		return horizontal || vertical;
	}

}

using UnityEngine;
using System.Collections;

public class HelpMono : MonoBehaviour {
	[SerializeField] private GameObject m_WelcomeUI;
	[SerializeField] private GameObject[] m_IconUI;
	[SerializeField] private GameObject[] m_FunctionUI;
	[SerializeField] private Camera m_Camera;
	[SerializeField] private GameObject m_VSUI;
	private GameObject m_CurrentUI;
	private int m_CodeNum;


	// Use this for initialization
	void Start () {
	}
	
	//自己调用
	public void update () {
		if (Input.GetMouseButton (0)) {
			RaycastHit hitObject;
			if (Physics.Raycast (m_Camera.ScreenPointToRay(Input.mousePosition),out hitObject,1000,1<<10)) {
				StartCoroutine( ChangeCode(hitObject.transform.GetComponent<IDMono> ().ID));
			}
		}
	}
	public void Init()
	{
		int num = CodeLibrary.instance.codeList.Count;
		for (int i = 0; i < num; i++) 
		{
			m_IconUI [i].SetActive (true);
		}
		m_CurrentUI = m_WelcomeUI;
		m_VSUI.SetActive (false);

	}
	public void Close()
	{
		m_VSUI.SetActive (true);
		m_WelcomeUI.SetActive (true);
		foreach (GameObject temp in m_FunctionUI) {
			temp.SetActive (false);
		}
	}
	private IEnumerator ChangeCode(int num)
	{
		m_CurrentUI.SetActive (false);
		m_CurrentUI = m_FunctionUI [num];
		yield return 0;
		m_CurrentUI.SetActive (true);
	}
}

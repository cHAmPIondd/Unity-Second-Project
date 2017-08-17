using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameProgressManager : MonoBehaviour {
	public static GameProgressManager instance;


	public delegate IEnumerator animation ();

	private bool m_IsAnimation;
	private Queue<animation> m_AnimationQueue;

	private bool m_IsTipShow;
	[SerializeField] private Text m_TipText;

	void Awake()
	{
		instance = this;
		Cursor.visible = false;
	}
	void Start () {
		m_AnimationQueue = new Queue<animation> ();
	}


	// Update is called once per frame
	void Update () 
	{
		if (m_IsAnimation == false) 
		{
			if (m_AnimationQueue.Count>0) 
			{
				StartCoroutine (ExecuteAnimation(m_AnimationQueue.Dequeue ()));
				m_IsAnimation = true;
			}
		}
	}	
	public void AddAnimation(animation newAni)
	{
		m_AnimationQueue.Enqueue (newAni);
	}
	private IEnumerator ExecuteAnimation(animation executeAni)
	{
		yield return StartCoroutine (executeAni());
		m_IsAnimation = false;
	}

	public void TipAnimation(string text,bool isHold)
	{
		if (isHold) 
		{
			StopCoroutine ("ShowTip");
			StopCoroutine ("ShowTipWithTime");
			StartCoroutine ("ShowTipWithTime",text);
		} 
		else 
		{
			if (m_IsTipShow == false) {
				StopCoroutine ("ShowTip");
				StartCoroutine ("ShowTip",text);
			}
		}
	}
	private IEnumerator ShowTipWithTime(string text)
	{
		m_IsTipShow = true;
		m_TipText.text = text;
		m_TipText.transform.parent.gameObject.SetActive (true);
		for(float i=2;i>=0;i-=Time.deltaTime)
			yield return 0;
		m_TipText.text = "";
		m_TipText.transform.parent.gameObject.SetActive (false);
		m_IsTipShow = false;
	}
	private IEnumerator ShowTip(string text)
	{
		m_TipText.text = text;
		m_TipText.transform.parent.gameObject.SetActive (true);
		yield return 0;
		m_TipText.text = "";
		m_TipText.transform.parent.gameObject.SetActive (false);
	}
}

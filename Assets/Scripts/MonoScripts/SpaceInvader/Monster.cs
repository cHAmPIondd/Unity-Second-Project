using UnityEngine;
using System.Collections;

public class Monster: MonoBehaviour {

	[SerializeField]private float m_Period=1f;
	[SerializeField]private float m_Distance=0.5f;
	[SerializeField]private float m_Time=12;
	[SerializeField]private float m_DelayTime;
	private int m_Timer;
	private IEnumerator PatrolCoroutine()
	{
		yield return new WaitForSeconds (m_DelayTime);
		while (true) 
		{
			yield return new WaitForSeconds (m_Period);
			transform.position += transform.right * m_Distance;
			m_Timer++;
			if (m_Timer >= m_Time) 
			{
				m_Timer = 0;
				m_Distance = -m_Distance;
			}
		}
	}
	// Use this for initialization
	void Start () {
		StartCoroutine (PatrolCoroutine());
	}

}


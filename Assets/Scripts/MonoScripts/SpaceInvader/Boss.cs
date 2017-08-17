using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

	[SerializeField]private float m_Rate=1;
	[SerializeField]private float m_Period=10;
	[SerializeField]private float m_DelayTime=0;

	private Vector3 m_OriginalPosition;
	private IEnumerator PatrolCoroutine()
	{
		yield return new WaitForSeconds (m_DelayTime);
		m_OriginalPosition = transform.position;
		while (true) 
		{
			for (float i = 0; i < 5.5f / m_Rate; i += Time.deltaTime) 
			{
				transform.position += (-transform.right * m_Rate * Time.deltaTime);
				yield return 0;
			}
			transform.position=m_OriginalPosition;
			yield return new WaitForSeconds (m_Period);
		}
	}
	// Use this for initialization
	void Start () {
		StartCoroutine (PatrolCoroutine());
	}
}

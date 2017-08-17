using UnityEngine;
using System.Collections;

public class ReadCodeCanBeGet_Only : MonoBehaviour {

	[SerializeField]private Transform[] m_TransformArray;
	private bool m_CanBeGet;
	private EntryableMono m_EntryableMono;
	private EKeyTipMono m_EKeyTipMono;
	void Start()
	{
		m_EntryableMono = GetComponent<EntryableMono> ();
		m_EKeyTipMono = GetComponent<EKeyTipMono> ();
		StartCoroutine (update ());
	}
	private IEnumerator update()
	{
		while (true) 
		{
			yield return new WaitForSeconds (1);
			m_CanBeGet = true;
			foreach (Transform temp in m_TransformArray) 
			{
				if (Physics.Raycast (temp.position, temp.forward, 1f, 1 << 15)) //1<<15 书
				{
					m_CanBeGet = false;
					break;
				}
			}
			if (m_CanBeGet) 
			{
				m_EntryableMono.enabled = true;
				m_EKeyTipMono.enabled = false;
			}
			else
			{
				m_EntryableMono.enabled = false;
				m_EKeyTipMono.enabled = true;
			}
		}
	}
}

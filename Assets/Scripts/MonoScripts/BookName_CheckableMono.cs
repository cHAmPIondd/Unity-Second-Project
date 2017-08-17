using UnityEngine;
using System.Collections;

public class BookName_CheckableMono : CheckableMono {
	[SerializeField] private GameObject[] m_Book;
	public override void CheckFinished()
	{		
		m_Book [0].name = "Java";
		m_Book [1].name = "Geography";
		m_Book [2].name = "CodeBook";
		GameObjectManager.instance.rendererDict.Add(m_Book [0].name.ToUpper(),m_Book [0]);
		GameObjectManager.instance.rendererDict.Add(m_Book [1].name.ToUpper(),m_Book [1]);
		GameObjectManager.instance.rendererDict.Add(m_Book [2].name.ToUpper(),m_Book [2]);
	}
}

using UnityEngine;
using System.Collections;

public class Read_Code : Code {

	private ReadableMono m_ReadableMono;

	public Read_Code()
		:base()
	{
	}
	public override string IsLegalParameter(string parameter)
	{
		parameter = parameter.Trim ();
		string[] tempString = parameter.Split (',');//DeBug‘，’在最前面与最后面的情况

		if (tempString.Length > 1) {
			return "参数过多";
		} 
		else 
		{	
			for (int i=0;i<tempString.Length;i++) 
			{
				tempString [i] = tempString [i].Trim ();
			}

			GameObject tempGO;
			if (GameObjectManager.instance.rendererDict.TryGetValue (tempString[0], out tempGO)) 
			{
				m_ReadableMono = tempGO.GetComponent<ReadableMono> ();//第一个参数
				if (m_ReadableMono == null) 
				{
					return "不可移动物体";
				} 
			} 
			else 
			{
				return "找不到指定物体";
			}
			return "";
		} 
	}
	public override IEnumerator Function()
	{
		if (!(m_ReadableMono.bookMono.lastReadBook && m_ReadableMono.bookMono.lastReadBook == m_ReadableMono))
		{
			//移动镜头
			for (float timer = 1; timer >= 0; timer -= Time.deltaTime) {
				if (Vector3.SqrMagnitude (m_ReadableMono.computerCameraTran.position - m_ReadableMono.feedbackCameraTarget.position) < 1
					&& Quaternion.Angle(m_ReadableMono.computerCameraTran.rotation,m_ReadableMono.feedbackCameraTarget.rotation)<1)
					break;
				m_ReadableMono.computerCameraTran.position = Vector3.Lerp (m_ReadableMono.computerCameraTran.position, m_ReadableMono.feedbackCameraTarget.position, 5 * Time.deltaTime);
				m_ReadableMono.computerCameraTran.rotation = Quaternion.Lerp (m_ReadableMono.computerCameraTran.rotation, m_ReadableMono.feedbackCameraTarget.rotation, 5 * Time.deltaTime);
				yield return 0;  
			}

			//书移动并打开

			m_ReadableMono.originalPosition = m_ReadableMono.transform.position;
			m_ReadableMono.originalRotation = m_ReadableMono.transform.rotation;
			if (m_ReadableMono.bookMono.lastReadBook) 
			{
				for (float timer = 0.5f; timer >= 0; timer -= Time.deltaTime) 
				{
					m_ReadableMono.bookMono.lastReadBook.transform.position = Vector3.Lerp (m_ReadableMono.bookMono.lastReadBook.transform.position, m_ReadableMono.bookMono.lastReadBook.originalPosition+Vector3.forward*10, 4 * Time.deltaTime);
					m_ReadableMono.bookMono.lastReadBook.transform.rotation = Quaternion.Lerp (m_ReadableMono.bookMono.lastReadBook.transform.rotation, m_ReadableMono.bookMono.lastReadBook.originalRotation, 4 * Time.deltaTime);
					yield return 0;
				}
				for (float timer = 1f; timer >= 0; timer -= Time.deltaTime) 
				{
					m_ReadableMono.bookMono.lastReadBook.transform.position = Vector3.Lerp (m_ReadableMono.bookMono.lastReadBook.transform.position, m_ReadableMono.bookMono.lastReadBook.originalPosition, 4 * Time.deltaTime);
					m_ReadableMono.bookMono.lastReadBook.transform.rotation = Quaternion.Lerp (m_ReadableMono.bookMono.lastReadBook.transform.rotation, m_ReadableMono.bookMono.lastReadBook.originalRotation, 4 * Time.deltaTime);
					yield return 0;
				}
				m_ReadableMono.bookMono.lastReadBook.transform.position=m_ReadableMono.bookMono.lastReadBook.originalPosition;
				m_ReadableMono.bookMono.lastReadBook.transform.rotation=m_ReadableMono.bookMono.lastReadBook.originalRotation;
			}
			for (float timer = 0.5f; timer >= 0; timer -= Time.deltaTime) 
			{
				m_ReadableMono.transform.position = Vector3.Lerp (m_ReadableMono.transform.position, m_ReadableMono.bookMono.openBookTranform.position+Vector3.forward*10, 4 * Time.deltaTime);
				m_ReadableMono.transform.rotation = Quaternion.Lerp (m_ReadableMono.transform.rotation, m_ReadableMono.bookMono.openBookTranform.rotation, 4 * Time.deltaTime);
				yield return 0;
			}
			for (float timer =1f; timer >= 0; timer -= Time.deltaTime) 
			{
				m_ReadableMono.transform.position = Vector3.Lerp (m_ReadableMono.transform.position, m_ReadableMono.bookMono.openBookTranform.position, 4 * Time.deltaTime);
				m_ReadableMono.transform.rotation = Quaternion.Lerp (m_ReadableMono.transform.rotation, m_ReadableMono.bookMono.openBookTranform.rotation, 4* Time.deltaTime);
				yield return 0;
			}
			m_ReadableMono.transform.position=m_ReadableMono.bookMono.openBookTranform.position;
			m_ReadableMono.transform.rotation=m_ReadableMono.bookMono.openBookTranform.rotation;

			//改变书的性质
			if (m_ReadableMono.bookMono.lastReadBook) {
				m_ReadableMono.bookMono.lastReadBook.GetComponent<MovableMono> ().enabled = true;
				m_ReadableMono.bookMono.lastReadBook.GetComponent<EntryableMono> ().enabled = false;
			}
				m_ReadableMono.bookMono.lastReadBook = m_ReadableMono;
			m_ReadableMono.bookMono.lastReadBook.GetComponent<MovableMono> ().enabled = false;
			m_ReadableMono.bookMono.lastReadBook.GetComponent<EntryableMono> ().enabled = true;
		}
	}
	public override string ToString()
	{
		return "Read_Code";
	}
}

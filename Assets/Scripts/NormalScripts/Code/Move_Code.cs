using UnityEngine;
using System.Collections;

public class Move_Code : Code {

	private MovableMono m_MovableMono;
	private float m_Parameter2=-12580;

	public Move_Code()
		:base()
	{
	}
	public override string IsLegalParameter(string parameter)
	{
		parameter = parameter.Trim ();
		string[] tempString = parameter.Split (',');//DeBug‘，’在最前面与最后面的情况

		if (tempString.Length > 2) {
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
				m_MovableMono = tempGO.GetComponent<MovableMono> ();//第一个参数
				m_Parameter2 = m_MovableMono.defaultDistance;//第二个默认参数
				if (m_MovableMono == null) 
				{
					return "不可移动物体";
				} 
				else 
				{
					if (tempString.Length == 2) {
						try {
							m_Parameter2 = float.Parse (tempString [1]);//第二个参数
							Debug.Log (m_Parameter2);
							if (m_Parameter2 > m_MovableMono.maxDistance)
								return "参数过大";
							if (m_Parameter2 < m_MovableMono.minDistance)
								return "参数过小";
						} catch (System.Exception ex) {
							return "第二个参数错误";
						}
					} 
				}
			} 
			else 
			{
				return "找不到指定物体";
			}
			return "";

		} 
	}
	public override void Init()
	{
		//重置位置
		m_MovableMono.transform.position = m_MovableMono.originalPosition;
	}
	public override IEnumerator Function()
	{

		//移动镜头
		for (float timer = 1; timer >= 0; timer -= Time.deltaTime)
		{
			if (Vector3.SqrMagnitude (m_MovableMono.computerCameraTran.position - m_MovableMono.feedbackCameraTarget.position) < 1
				&& Quaternion.Angle(m_MovableMono.computerCameraTran.rotation,m_MovableMono.feedbackCameraTarget.rotation)<1)
				break;
			m_MovableMono.computerCameraTran.position = Vector3.Lerp (m_MovableMono.computerCameraTran.position,m_MovableMono.feedbackCameraTarget.position, 5 * Time.deltaTime);
			m_MovableMono.computerCameraTran.rotation = Quaternion.Lerp (m_MovableMono.computerCameraTran.rotation,m_MovableMono.feedbackCameraTarget.rotation, 5 * Time.deltaTime);
			yield return 0;  
		}

		//移动物体
		for (float timer = 1; timer >= 0; timer -= Time.deltaTime) 
		{
			m_MovableMono.transform.position = Vector3.Lerp (m_MovableMono.transform.position, m_MovableMono.originalPosition + m_MovableMono.direction * m_Parameter2, 5 * Time.deltaTime);
			yield return 0;
		}
		m_MovableMono.transform.position = m_MovableMono.originalPosition +m_MovableMono.direction * m_Parameter2;

	}
	public override string ToString ()
	{
		return "Move_Code";
	}
}

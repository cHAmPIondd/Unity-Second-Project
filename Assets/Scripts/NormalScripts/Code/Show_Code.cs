using UnityEngine;
using System.Collections;
using HighlightingSystem;

public class Show_Code : Code {

	private ShowableMono m_ShowableMono;

	public Show_Code()
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
				m_ShowableMono = tempGO.GetComponent<ShowableMono> ();//第一个参数
				if (m_ShowableMono == null) 
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
		//移动镜头
		for (float timer = 1; timer >= 0; timer -= Time.deltaTime) 
		{
			if (Vector3.SqrMagnitude (m_ShowableMono.computerCameraTran.position - m_ShowableMono.feedbackCameraTarget.position) < 1
				&& Quaternion.Angle(m_ShowableMono.computerCameraTran.rotation,m_ShowableMono.feedbackCameraTarget.rotation)<1)
				break;
			m_ShowableMono.computerCameraTran.position = Vector3.Lerp (m_ShowableMono.computerCameraTran.position, m_ShowableMono.feedbackCameraTarget.position, 5 * Time.deltaTime);
			m_ShowableMono.computerCameraTran.rotation = Quaternion.Lerp (m_ShowableMono.computerCameraTran.rotation, m_ShowableMono.feedbackCameraTarget.rotation, 5 * Time.deltaTime);
			yield return 0;  
		}

		//闪烁
		Highlighter h=m_ShowableMono.GetComponent<Highlighter>();
		h.FlashingOn (Color.clear,Color.blue);
		for (float timer = 1; timer >= 0; timer -= Time.deltaTime) 
		{
			yield return 0;
		}
		h.FlashingOff ();
		m_ShowableMono.Show ();

	}
	public override string ToString()
	{
		return "Show_Code";
	}
}

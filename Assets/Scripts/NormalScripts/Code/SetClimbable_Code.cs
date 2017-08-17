using UnityEngine;
using System.Collections;

public class SetClimbable_Code : Code {
	private ClimbableMono m_ClimbableMono;

	public SetClimbable_Code()
		:base()
	{
	}
	public override string IsLegalParameter(string parameter)
	{
		if (parameter == "") {//默认参数
			parameter = "DOOR1";
			GameObject tempGO = GameObject.Find ("AirWall");
			if (tempGO != null)
				tempGO.SetActive (false);
		}
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
				m_ClimbableMono = tempGO.GetComponent<ClimbableMono> ();//第一个参数
				if (m_ClimbableMono == null) 
				{
					return "不可攀爬物体";
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
			if (Vector3.SqrMagnitude (m_ClimbableMono.computerCameraTran.position - m_ClimbableMono.feedbackCameraTarget.position) < 1
				&& Quaternion.Angle(m_ClimbableMono.computerCameraTran.rotation,m_ClimbableMono.feedbackCameraTarget.rotation)<1)
				break;
			m_ClimbableMono.computerCameraTran.position = Vector3.Lerp (m_ClimbableMono.computerCameraTran.position, m_ClimbableMono.feedbackCameraTarget.position, 5 * Time.deltaTime);
			m_ClimbableMono.computerCameraTran.rotation = Quaternion.Lerp (m_ClimbableMono.computerCameraTran.rotation, m_ClimbableMono.feedbackCameraTarget.rotation, 5 * Time.deltaTime);
			yield return 0;  
		}

		//闪烁
		m_ClimbableMono.ladder.SetActive(true);
		yield return new WaitForSeconds (0.15f);
		m_ClimbableMono.ladder.SetActive(false);
		yield return new WaitForSeconds (0.15f);
		m_ClimbableMono.ladder.SetActive(true);
		yield return new WaitForSeconds (0.15f);
		m_ClimbableMono.ladder.SetActive(false);
		yield return new WaitForSeconds (0.15f);
		m_ClimbableMono.ladder.SetActive(true);
		yield return new WaitForSeconds (0.15f);
		m_ClimbableMono.ladder.SetActive(false);
		yield return new WaitForSeconds (0.15f);
		m_ClimbableMono.ladder.SetActive(true);
		yield return new WaitForSeconds (0.15f);

	}
	public override string ToString()
	{
		return "SetClimbable_Code";
	}
}

using UnityEngine;
using System.Collections;

public class DormLightOn_Code : Code {
	private static GameObject DormLight;
	private float m_Parameter1;
	private float m_MaxParameter1=1;
	private float m_MinParameter1=0;
	public DormLightOn_Code()
		:base()
	{
		
	}
	public override string IsLegalParameter(string parameter)
	{
		bool hasPeriod=false;
		foreach (char ch in parameter) 
		{
			if (ch != '.') {
				if (ch < '0' || ch > '9')
					return "参数不符";
			} else {
				if (!hasPeriod)
					hasPeriod = true;
				else
					return "多个小数点是什么鬼";
			}
		}
		float parameterFloat= float.Parse (parameter);
		if (parameterFloat > m_MaxParameter1)
			return "参数太大";
		if (parameterFloat < m_MinParameter1)
			return "参数太小";
		m_Parameter1 = parameterFloat;
		return "";	
	}
	public override IEnumerator Function()
	{
		if(!DormLight)
			DormLight=GameObject.Find ("DormLight");
		DormLight.GetComponent<Light> ().intensity = m_Parameter1*6;
		yield return 0;
	}
	public override string ToString ()
	{
		return "DormLightOn_Code";
	}
}

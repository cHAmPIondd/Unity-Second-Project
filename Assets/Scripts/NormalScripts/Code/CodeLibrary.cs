using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CodeLibrary {
	private static CodeLibrary m_Instance;
	public static CodeLibrary instance
	{
		get
		{
			if (m_Instance == null) {
				m_Instance = new CodeLibrary();
			}
			return m_Instance;
		}
	}
	private List<string> m_CodeList;
	public List<string> codeList
	{
		get{return m_CodeList;}
	}

	private CodeLibrary()
	{
		m_CodeList = new List<string>();
	}
	public void AddCode(string code)
	{
		if (!m_CodeList.Contains (code.ToUpper())) {
			GameProgressManager.instance.TipAnimation ("获得代码:" + code, true);
			m_CodeList.Add (code.ToUpper () + "_CODE");
		}
	}
}

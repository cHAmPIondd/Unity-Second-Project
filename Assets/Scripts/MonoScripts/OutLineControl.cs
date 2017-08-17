using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using HighlightingSystem;
[RequireComponent(typeof (Renderer))]
public class OutLineControl : MonoBehaviour {
	private enum OutLineColor
	{
		Red,
		Green
	}
	protected Highlighter h;
	[SerializeField]
	private OutLineColor m_Color=OutLineColor.Red;

	private Color m_TrueColor;


	[SerializeField]private Text m_SelectionText;
	void Awake()
	{
		h = GetComponent<Highlighter>();
		if (h == null) { h = gameObject.AddComponent<Highlighter>(); }
	}
	void Start()
	{
		if (m_Color == OutLineColor.Red)
			m_TrueColor = Color.red;
		else if (m_Color == OutLineColor.Green)
			m_TrueColor = Color.green;
	}
	// 
	public void OutLineOn()
	{
		h.ConstantOn (m_TrueColor);
		m_SelectionText.text = transform.name;
	}
	public void OutLineOff()
	{
		h.ConstantOff ();
		m_SelectionText.text = "";
	}
}

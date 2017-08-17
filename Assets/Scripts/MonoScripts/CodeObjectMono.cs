using UnityEngine;
using System.Collections;

public class CodeObjectMono : MonoBehaviour {
	private enum CodeType
	{
		Move_Code,
		Show_Code
	}
	[SerializeField] private CodeType m_Type; 

	private bool m_HasBeGet;

	void Start()
	{
	}

	void OnMouseDown()
	{
		if(!m_HasBeGet)
		StartCoroutine (DestoryAnimation ());
	}	
	private IEnumerator DestoryAnimation()
	{
		m_HasBeGet = true;
		SpriteRenderer Sprite = GetComponent<SpriteRenderer> ();
		BeGet ();
		for (float i = 0; i < 0.1f; i+=Time.deltaTime) 
		{
			Sprite.color = Color.Lerp (Sprite.color, Color.black, 20 * Time.deltaTime);
			yield return 0;
		}
		for (float i = 0; i < 0.8f; i+=Time.deltaTime) 
		{
			Sprite.color = Color.Lerp (Sprite.color, Color.clear, 7* Time.deltaTime);
			yield return 0;
		}
		Destroy (this.gameObject);
	}
	public void BeGet()
	{
		if (m_Type == CodeType.Move_Code) {
			CodeLibrary.instance.AddCode ("Move");
		} else if (m_Type == CodeType.Show_Code) {
			CodeLibrary.instance.AddCode ("Show");
		}

	}
}

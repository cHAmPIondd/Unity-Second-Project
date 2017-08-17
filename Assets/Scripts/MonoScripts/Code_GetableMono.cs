using UnityEngine;
using System.Collections;

public class Code_GetableMono : GetableMono{
	private enum CodeType
	{
		Read_Code
	}
	[SerializeField] private CodeType m_Type; 
	private IEnumerator DestoryAnimation()
	{
		SpriteRenderer Sprite = GetComponent<SpriteRenderer> ();
		for (float i = 0; i < 0.8f; i+=Time.deltaTime) 
		{
			Sprite.color = Color.Lerp (Sprite.color, Color.clear, 10 * Time.deltaTime);
			yield return 0;
		}
		Destroy (this.gameObject);
	}
	public override void BeGet()
	{
		if (m_Type == CodeType.Read_Code) 
		{
			CodeLibrary.instance.AddCode ("Read");
		}
		StartCoroutine (DestoryAnimation ());
	}
}

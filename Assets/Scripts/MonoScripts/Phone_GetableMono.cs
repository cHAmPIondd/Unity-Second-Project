using UnityEngine;
using System.Collections;

public class Phone_GetableMono : GetableMono {
	public override void BeGet()
	{
		PlayerControl.instance.GetPhone ();
		GameProgressManager.instance.TipAnimation("获得手机一部",true);
		transform.localScale = Vector3.zero;
		StartCoroutine (TipAnimation());
	}
	private IEnumerator TipAnimation()
	{
		while (true) 
		{
			GameProgressManager.instance.TipAnimation("按Q进入手机界面",false);
			yield return 0;
		}
	}

}

using UnityEngine;
using System.Collections;

public class Code {


	public Code()
	{
	}

	public static Code CodeFactory(string name)
	{
		if (name == "MOVE_CODE")
			return new Move_Code ();
		else if (name == "READ_CODE")
			return new Read_Code ();
		else if (name == "SHOW_CODE")
			return new Show_Code ();
		else if (name == "SETCLIMBABLE_CODE")
			return new SetClimbable_Code ();
		return null;
	}

	public virtual string IsLegalParameter(string parameter)
	{
		return "Error";	
	}
	public virtual void Init()
	{
	}
	public virtual IEnumerator Function()
	{
		Debug.Log ("I don't have any function");		
		yield return 0;
	}
	public virtual string ToString()
	{
		return "Code";
	}
}

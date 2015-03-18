using UnityEngine;
using System.Collections;

[System.Serializable]
public class TBFloatScale{
	public float inputMin = 0;
	public float inputMax = 1;
	public float outputMin = 0;
	public float outputMax = 1;
	public float value;
	public int inputCount = 0;
	public bool limit;
	
	public float Scale(float original)
	{
		if(limit)
		{
			original = Mathf.Clamp(original, inputMin, inputMax);
		}
		value = original;
		inputCount++;
		return (original-outputMin)/(outputMax-outputMin);
	}
}

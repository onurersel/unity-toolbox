using UnityEngine;
using System.Collections;

[System.Serializable]
public class TBFloatScale{
	public float min;
	public float max;

	public float Scale(float original)
	{
		return (original-min)/(max-min);
	}
}

using UnityEngine;
public class TBRandom : MonoBehaviour {
	static public float safeValue{
		get{
			return Random.Range(0f, 0.9999999f);
		}
	}

	static public int Dice(int side)
	{
		return Mathf.FloorToInt(Random.value * (side-0.00000001f));
	}
}

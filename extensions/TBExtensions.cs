using UnityEngine;

public static class TBExtensions {

	//Rect
	public static Rect Add(this Rect r1, Rect rect)
	{
		return new Rect(r1.x+rect.x, r1.y+rect.y, r1.width+rect.width, r1.height+rect.height);
	}
	public static Rect Subtract(this Rect r1, Rect rect)
	{
		return new Rect(r1.x-rect.x, r1.y-rect.y, r1.width-rect.width, r1.height-rect.height);
	}
	public static Rect Multiply(this Rect r1, Rect rect)
	{
		return new Rect(r1.x*rect.x, r1.y*rect.y, r1.width*rect.width, r1.height*rect.height);
	}
	public static Rect Divide(this Rect r1, Rect rect)
	{
		return new Rect(r1.x/rect.x, r1.y/rect.y, r1.width/rect.width, r1.height/rect.height);
	}

	//string
	public static string SafeSubstring(this string str, int startIndex, int length)
	{
		if(str.Length >= startIndex+length)
		{
			return str.Substring(startIndex, length);
		}
		else
		{
			return str.Substring(startIndex, str.Length-startIndex);
		}
	}
}

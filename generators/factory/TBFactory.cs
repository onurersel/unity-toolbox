using UnityEngine;
using System.Collections.Generic;
using TBFactory_private;

[RequireComponent(typeof(TBPool))]
public class TBFactory : MonoBehaviour {

	private TBPool m_pool;
	private List<TBFactoryItem> sortedItems;
	[Range(0f,1f)]
	public float hardness;
	public List<TBFactoryItem> items;

	void Awake()
	{
		m_pool = GetComponent<TBPool>();
	}

	void Start()
	{
		sortedItems = new List<TBFactoryItem>(items);
		sortedItems.Sort(delegate(TBFactoryItem x, TBFactoryItem y) {
				 if(x.hardness == y.hardness)		return 0;
			else if (x.hardness > y.hardness)		return 1;
			else									return -1;
		});
	}

	public GameObject Request(float hardness)
	{
		this.hardness = hardness;
		return Request();
	}
	public GameObject Request()
	{
		int l = 0;
		for(int i = 0; i < sortedItems.Count; ++i)
		{

		}
		return null;
	}
}

namespace TBFactory_private
{
	[System.Serializable]
	public class TBFactoryItem
	{
		public GameObject template;
		[Range(0f,1f)]
		public float hardness;
	}
}
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
    public bool isForcingIndex;
    public int forcedIndex;

	void Awake()
	{
		m_pool = GetComponent<TBPool>();
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
	    if (isForcingIndex)
	    {
            var forcedTemplate = sortedItems[forcedIndex].template;
            return m_pool.RequestWithTemplate(forcedTemplate);
	    }

		int l = 0;
		for(int i = 0; i < sortedItems.Count; ++i)
		{
			var item = sortedItems[i];
			if(item.hardness > hardness)
			{
				break;
			}
			l++;
		}

		if(l == 0)
			return null;

		var dice = TBRandom.Dice(l);
		var template = sortedItems[dice].template;

		return m_pool.RequestWithTemplate(template);
	}

    public void Recycle(GameObject gameObject)
    {
        m_pool.Recycle(gameObject);
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
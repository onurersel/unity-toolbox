using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TBPool_private;


public class TBPool : MonoBehaviour {
	
	private List<TBPoolItem> m_poolList;
	public List<GameObject> requestedObjects;
	public Transform container;

	public TBPool()
	{
		m_poolList = new List<TBPoolItem>();
		requestedObjects = new List<GameObject>();
	}
	
	public GameObject RequestWithTemplate(GameObject template, int preinstantiateCount = 0)
	{
		var pool = GetPoolItemWithTemplate(template, preinstantiateCount);
		var obj = pool.Request();
		requestedObjects.Add(obj);
		return obj;
	}
	
	public void Recycle(GameObject obj)
	{
		requestedObjects.Remove(obj);
		foreach(var pool in m_poolList)
		{
			if(pool.Contains(obj))
			{
				pool.Recycle(obj);
				return;
			}
		}
		
		Debug.LogError("Should recycle this object : " + obj.name + " " + obj);
	}
	
	private TBPoolItem GetPoolItemWithTemplate(GameObject template, int preinstantiateCount)
	{
		TBPoolItem pool;
		for(int i = 0; i < m_poolList.Count; ++i)
		{
			pool = m_poolList[i];
			if(pool.template == template)
			{
				return pool;
			}
		}
		
		pool = new TBPoolItem(template, preinstantiateCount, container);
		m_poolList.Add(pool);
		return pool;
	}


	/* INFO */

	public bool isAllocated
	{
		get{
			return (m_poolList != null);
		}
	}
	public int templateCount
	{
		get{
			return m_poolList.Count;
		}
	}
	public string GetTemplateName(int order)
	{
		return m_poolList[order].template.name;
	}
	public int GetTemplateRequestedCount(int order)
	{
		return m_poolList[order].requestedCount;
	}
	public int GetTemplateRecycledCount(int order)
	{
		return m_poolList[order].recycledCount;
	}
}


namespace TBPool_private
{
	class TBPoolItem
	{
		private List<GameObject> m_requestedList;
		private List<GameObject> m_recycledList;
		private GameObject m_template;
		private Transform m_container;
		
		public GameObject template	{get{  return m_template;  }}
		public int requestedCount  	{get{  return m_requestedList.Count;  }}
		public int recycledCount	{get{  return m_recycledList.Count;  }}
		
		public TBPoolItem(GameObject template, int preinstantiateCount, Transform container)
		{
			m_template = template;
			m_container = container;
			m_requestedList = new List<GameObject>();
			m_recycledList = new List<GameObject>();
			
			for(int i = 0; i < preinstantiateCount; ++i)
			{
				GameObject obj = GameObject.Instantiate(m_template);
				obj.SetActive(false);
				if(m_container != null)		obj.transform.parent = m_container;
				m_recycledList.Add(obj);
			}
		}
		
		public GameObject Request()
		{
			GameObject obj;
			if(m_recycledList.Count > 0)
			{
				obj = m_recycledList[0];
				m_recycledList.Remove(obj);
			}
			else
			{
				obj = GameObject.Instantiate(m_template);
				obj.name += " id:" + (requestedCount+recycledCount);
			}
			
			m_requestedList.Add(obj);
			obj.SetActive(true);
			return obj;
		}
		public void Recycle(GameObject obj)
		{
			obj.SetActive(false);
			if(m_container != null)		obj.transform.parent = m_container;
			m_requestedList.Remove(obj);
			m_recycledList.Add(obj);
		}
		
		public bool Contains(GameObject obj)
		{
			return m_requestedList.Contains(obj);
		}
	}

}



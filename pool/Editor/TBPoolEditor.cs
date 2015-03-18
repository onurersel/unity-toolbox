using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TBPool))]
public class TBPoolEditor : Editor {

	private static int valueFieldWidth = 30;

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		var pool = target as TBPool;

		EditorGUILayout.Space();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Total requested count");
		GUILayout.Space(30);
		GUILayout.Label(pool.requestedObjects.Count.ToString(), GUILayout.Width(valueFieldWidth));
		GUILayout.EndHorizontal();

		EditorGUILayout.Space();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Type");
		GUILayout.Space(30);
		GUILayout.Label("req", GUILayout.Width(valueFieldWidth));
		GUILayout.Label("rec", GUILayout.Width(valueFieldWidth));
		GUILayout.Label("total", GUILayout.Width(valueFieldWidth));
		GUILayout.EndHorizontal();

		if(pool.isAllocated)
		{
			int count = pool.templateCount;
			for(int i = 0; i < count; ++i)
			{
				string tName = pool.GetTemplateName(i);
				int tRequestedCount = pool.GetTemplateRequestedCount(i);
				int tRecycledCount = pool.GetTemplateRecycledCount(i);
				drawField(tName, tRequestedCount, tRecycledCount, tRequestedCount+tRecycledCount);
			}
		}
		else
		{
			GUILayout.Label("Not allocated yet");
		}

		EditorUtility.SetDirty(target);
	}
	
	void drawField(string name, int requestedCount, int recycledCount, int totalCount){
		GUILayout.BeginHorizontal();
		GUILayout.Label(name);
		GUILayout.Label(requestedCount.ToString(), GUILayout.Width(valueFieldWidth));
		GUILayout.Label(recycledCount.ToString(), GUILayout.Width(valueFieldWidth));
		GUILayout.Label(totalCount.ToString(), GUILayout.Width(valueFieldWidth));
		GUILayout.EndHorizontal();
	}
}

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(TBFloatScale))]
public class TBFloatScaleDrawer : PropertyDrawer {

	private static Rect newLine = new Rect(0,18,0,0);

	private bool m_isOpen = false;

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{

		position.height = newLine.y;
		float width = position.width;
		float w8 = width/8f;


		SerializedProperty inputMin = property.FindPropertyRelative("inputMin");
		SerializedProperty inputMax = property.FindPropertyRelative("inputMax");
		SerializedProperty outputMin = property.FindPropertyRelative("outputMin");
		SerializedProperty outputMax = property.FindPropertyRelative("outputMax");
		SerializedProperty limit = property.FindPropertyRelative("limit");


		//title
		m_isOpen = EditorGUI.Foldout(position, m_isOpen, label);

		if(m_isOpen)
		{
			EditorGUI.indentLevel = 1;

			//crop limits
			position = position.Add(newLine);
			limit.boolValue = EditorGUI.Toggle(position, "Crop limits", limit.boolValue);

			position = position.Add(newLine);
			EditorGUI.LabelField(position, "Input limits");

			position = position.Add(newLine);
			var fourColumn1Rect = new Rect(position.x, position.y, w8*2, newLine.y);
			var fourColumn2Rect = new Rect(position.x+w8, position.y, w8*3, newLine.y);
			var fourColumn3Rect = new Rect(position.x+w8*4, position.y, w8*2, newLine.y);
			var fourColumn4Rect = new Rect(position.x+w8*5, position.y, w8*3, newLine.y);

			//limits
			position = position.Add(newLine);
			EditorGUI.LabelField(position, "Output limits");

			EditorGUI.LabelField(fourColumn1Rect, "min");
			inputMin.floatValue = EditorGUI.FloatField(fourColumn2Rect, inputMin.floatValue);
			EditorGUI.LabelField(fourColumn3Rect, "max");
			inputMax.floatValue = EditorGUI.FloatField(fourColumn4Rect, inputMax.floatValue);

			EditorGUI.LabelField(fourColumn1Rect.Add(newLine).Add(newLine), "min");
			outputMin.floatValue = EditorGUI.FloatField(fourColumn2Rect.Add(newLine).Add(newLine), outputMin.floatValue);
			EditorGUI.LabelField(fourColumn3Rect.Add(newLine).Add(newLine), "max");
			outputMax.floatValue = EditorGUI.FloatField(fourColumn4Rect.Add(newLine).Add(newLine), outputMax.floatValue);
		}
	}
	/*
	private void DrawGraph(Rect position)
	{
		int resolution = 100;

		var indentedGraphRect = new Rect(position.x+3, position.y, position.width-6, position.height-newLine.y-2);
		var graphLabelRect = new Rect(indentedGraphRect.x-12, indentedGraphRect.y, 40,50);
		var graphRect = new Rect(position.x+18, position.y, position.width-18-4, position.height-newLine.y-2);
		EditorGUI.DrawRect(graphRect, Color.black);

		//draw bars
		float pieceWidth = graphRect.width/resolution;

		for(int i = 0; i < resolution; ++i)
		{
			float percent = .2f;
			float barHeight = percent*graphRect.height;
			EditorGUI.DrawRect(
				new Rect(graphRect.x+pieceWidth*i, graphRect.y+(graphRect.height-barHeight), pieceWidth, barHeight),
				Color.gray
				);
		}


		GUI.skin.label.alignment = TextAnchor.UpperCenter;
		GUI.skin.label.fontSize = 8;
		GUI.skin.label.normal.textColor = Color.white;
		for(int i = 0; i < 8; ++i)
		{
			EditorGUI.LabelField(graphLabelRect, , GUI.skin.label);
		}

		//graph limits
		m_graphMinX = EditorGUI.FloatField(new Rect(indentedGraphRect.x, indentedGraphRect.y+indentedGraphRect.height+2, 50f, newLine.y),m_graphMinX);
		m_graphMaxX = EditorGUI.FloatField(new Rect(indentedGraphRect.x+indentedGraphRect.width-50f, indentedGraphRect.y+indentedGraphRect.height+2, 50f, newLine.y),m_graphMaxX);

	}
	*/
	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		if(m_isOpen)
		{
			return newLine.y * 6f;
		}
		else
		{
			return newLine.y;
		}

	}


}
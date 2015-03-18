using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(TBFloatScale))]
public class TBFloatScaleDrawer : PropertyDrawer {

	private static Rect newLine = new Rect(0,18,0,0);

	private bool m_isOpen = false;
	private bool m_doCollectData = false;
	private int m_lastInputCount = 0;
	private float m_graphMinX = 0f;
	private float m_graphMaxX = 1f;
	private float m_graphMinY = 0f;
	private float m_graphMaxY = 1f;
	private AnimationCurve m_curve = new AnimationCurve();
	private Rect m_graphRange = new Rect(0,0,1,1);

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{

		position.height = newLine.y;
		float width = position.width;
		float w8 = width/8f;



		SerializedProperty inputMin = property.FindPropertyRelative("inputMin");
		SerializedProperty inputMax = property.FindPropertyRelative("inputMax");
		SerializedProperty outputMin = property.FindPropertyRelative("outputMin");
		SerializedProperty outputMax = property.FindPropertyRelative("outputMax");
		SerializedProperty value = property.FindPropertyRelative("value");
		SerializedProperty limit = property.FindPropertyRelative("limit");
		SerializedProperty inputCount = property.FindPropertyRelative("inputCount");


		//title
		m_isOpen = EditorGUI.Foldout(position, m_isOpen, label);

		if(m_isOpen)
		{
			EditorGUI.indentLevel = 1;

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

			position = position.Add(newLine);


			//crop limits
			position = position.Add(newLine);
			position = position.Add(newLine);
			limit.boolValue = EditorGUI.Toggle(position, "Crop limits", limit.boolValue);


			//graph
			position = position.Add(newLine);

			bool newCollectDataState = EditorGUI.Toggle(position, "Collect Data", m_doCollectData);
			position = position.Add(newLine);

			position = position.Add(new Rect(0,0,0,70));

			if(newCollectDataState)
			{
				//DrawGraph(position);
				m_graphRange.x = m_graphMinX;
				m_graphRange.width = m_graphMaxX-m_graphMinX;
				m_graphRange.y = m_graphMinY;
				m_graphRange.height = m_graphMaxY-m_graphMinY;
				EditorGUI.CurveField(position, m_curve, Color.green, m_graphRange);
				
				//min max
				position = position.Add(newLine);
				var indentedGraphRect = new Rect(position.x+3, position.y, position.width-6, position.height-newLine.y-2);
				m_graphMinX = EditorGUI.FloatField(new Rect(indentedGraphRect.x, indentedGraphRect.y+indentedGraphRect.height+2, 70, newLine.y),m_graphMinX);
				m_graphMaxX = EditorGUI.FloatField(new Rect(indentedGraphRect.x+indentedGraphRect.width-50f, indentedGraphRect.y+indentedGraphRect.height+2, 50f, newLine.y),m_graphMaxX);
			}

			if(! m_doCollectData &&  newCollectDataState)
			{
				m_curve = new AnimationCurve();


				float stepCount = 50f;
				float inc = (m_graphMaxX-m_graphMinX)/stepCount;
				for(int i = 0; i < stepCount; ++i)
				{
					m_curve.AddKey(new Keyframe(m_graphMinX+inc*i+inc/2f,0));
				}

				m_graphMinY = 0f;
				m_graphMaxY = 1f;
			}
			m_doCollectData = newCollectDataState;

			if(m_doCollectData)
			{
				if(inputCount.intValue != m_lastInputCount)
				{
					m_lastInputCount = inputCount.intValue;

					Keyframe[] oldkeys = m_curve.keys;
					m_curve = new AnimationCurve();
					bool didAdded = false;
					for(int i = oldkeys.Length-1; i >= 0 ; --i)
					{
						Keyframe k = oldkeys[i];
						
						if(didAdded == false  &&  k.time < value.floatValue)
						{
							didAdded = true;
							k.value++;
							
							if(k.value > m_graphMaxY)
								m_graphMaxY = k.value;
							else if(k.value < m_graphMinY)
								m_graphMinY = k.value;
						}
						
						m_curve.AddKey(new Keyframe(k.time, k.value));
					}
				}
			}
		}

		EditorUtility.SetDirty(property.serializedObject.targetObject);
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
			if(m_doCollectData)
			{
				return newLine.y * 14f;
			}
			else
			{
				return newLine.y * 8f;
			}

		}
		else
		{
			return newLine.y;
		}

	}


}
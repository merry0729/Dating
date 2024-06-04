using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
public class SerializableDictionaryEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var keysProperty = property.FindPropertyRelative("keys");
        var valuesProperty = property.FindPropertyRelative("values");

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        if (keysProperty.arraySize != valuesProperty.arraySize)
        {
            EditorGUI.LabelField(position, "Keys ¶û ValuesÀÇ size°¡ ´Ù¸§");
            EditorGUI.EndProperty();
            return;
        }

        float fieldWidth = position.width / 2;

        for (int i = 0; i < keysProperty.arraySize; i++)
        {
            Rect keyRect = new Rect(position.x, position.y + (i * EditorGUIUtility.singleLineHeight), fieldWidth, EditorGUIUtility.singleLineHeight);
            Rect valueRect = new Rect(position.x + fieldWidth, position.y + (i * EditorGUIUtility.singleLineHeight), fieldWidth, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(keyRect, keysProperty.GetArrayElementAtIndex(i), GUIContent.none);
            EditorGUI.PropertyField(valueRect, valuesProperty.GetArrayElementAtIndex(i), GUIContent.none);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var keysProperty = property.FindPropertyRelative("keys");

        return keysProperty.arraySize == 0 ? 
            (keysProperty.arraySize + 1) * EditorGUIUtility.singleLineHeight : 
            (keysProperty.arraySize) * EditorGUIUtility.singleLineHeight;
    }
}
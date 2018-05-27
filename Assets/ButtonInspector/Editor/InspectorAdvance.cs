using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using Object = UnityEngine.Object;

[CanEditMultipleObjects]
[CustomEditor(typeof(UnityEngine.Object), true, isFallback = false)]
public class InspectorAdvance : Editor
{

    MethodInfo currentMethod;
    object[] arguments;
    ParameterInfo currentParameter;
    int intTypeField;
    float floatTypeField;
    bool boolTypeField;
    string stringTypeField;
    bool isFoldOut;


    //    void OnEnable()
    //    {
    //        objectTypeField = new Object();
    //    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        Type type = target.GetType();
        MethodInfo[] methodInfo = type.GetMethods();
        for (int i = 0; i < methodInfo.Length; i++)
        {
            ButtonInspector attribute = Attribute.GetCustomAttribute(methodInfo[i], typeof(ButtonInspector)) as ButtonInspector;
            if (attribute != null)
            {
                if (attribute.mode == ButtonWorkMode.Both)
                {
                    DrawButtonAndInvoke(attribute, methodInfo[i]);
                }
                else
                {
                    if (Application.isPlaying)
                    {
                        if (attribute.mode == ButtonWorkMode.RuntimeOnly)
                        {
                            DrawButtonAndInvoke(attribute, methodInfo[i]);
                        }
                    }
                    else
                    {
                        if (attribute.mode == ButtonWorkMode.EditorOnly)
                        {
                            DrawButtonAndInvoke(attribute, methodInfo[i]);
                        }
                    }
                }
            }
        }
        EditorGUILayout.Space();
        DrawDefaultInspector();
    }

    void DrawButtonAndInvoke(ButtonInspector attribute, MethodInfo methodInfo)
    {
        if (GUILayout.Button(attribute.buttonName.Equals("") ? methodInfo.Name : attribute.buttonName))
        {
            foreach (var item in targets)
            {
                methodInfo.Invoke(item, null);
            }
        }
    }
}

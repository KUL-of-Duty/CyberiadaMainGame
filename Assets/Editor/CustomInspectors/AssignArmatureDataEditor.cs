using UnityEngine;
using UnityEditor;
using UnityEngine.Animations.Rigging;

[CustomEditor(typeof(AssignArmatureData))]
public class MyComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AssignArmatureData myComponent = (AssignArmatureData)target;

        if (GUILayout.Button("Assign Armature"))
        {
            myComponent.ExecuteAction();
        }
    }
}
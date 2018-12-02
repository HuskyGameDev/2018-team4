using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameManager gM = (GameManager)target;

        EditorGUILayout.BeginVertical();
        GUILayout.Label("Send Notifications Below");
        if (GUILayout.Button("Refresh"))
        {

        }
        GUILayout.Label("________________");
        List<string> notificationStrings = NotificationCenter.instance.GetNotificationKeys();
        for (int i = 0; i < notificationStrings.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Send : " + notificationStrings[i]))
            {
                this.PostNotification(notificationStrings[i]);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }
}

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(PlayerShootingController))]
//public class PlayerShootingControllerEditor : Editor
//{
//    override public void OnInspectorGUI()
//    {
//        serializedObject.Update();
//        var playerSC_Script = target as PlayerShootingController;
        
//        //EditorGUIUtility.LookLikeInspector();
//        SerializedProperty tps = serializedObject.FindProperty("bulletPositionOffset"); 
//        EditorGUI.BeginChangeCheck();
//        EditorGUILayout.PropertyField(tps, true);
//        if (EditorGUI.EndChangeCheck())
//            serializedObject.ApplyModifiedProperties();

//        //EditorGUIUtility.LookLikeControls();


//        //playerSC_Script.HasDoubleShoots = EditorGUILayout.Toggle("Double Shoots", playerSC_Script.HasDoubleShoots);
//        //if (playerSC_Script.HasDoubleShoots)
//        //{
//        //    playerSC_Script.BulletsPositionOffset = new Vector3[2];
//        //}

//        //using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(playerSC_Script.HasDoubleShoots)))
//        //{
//        //    if (group.visible == true)
//        //    {
//        //        EditorGUI.indentLevel++;

//        //        playerSC_Script.BulletsPositionOffset[0] = EditorGUILayout.Vector3Field("Bullet[0] Offset", playerSC_Script.BulletsPositionOffset[0]);
//        //        playerSC_Script.BulletsPositionOffset[1] = EditorGUILayout.Vector3Field("Bullet[1] Offset", playerSC_Script.BulletsPositionOffset[1]);
//        //        EditorGUI.indentLevel--;
//        //    }
//        //}
//    }
//}
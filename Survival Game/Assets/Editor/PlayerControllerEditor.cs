using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Game.Player.Controller;
using UnityEngine.UIElements;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    SerializedProperty player;

   
    bool gravityDataGroup;
    SerializedProperty[] gravityData;
    string[] gravityPropertyNames = new string[] { "useCustomGravity", "autoOrient", "oirientSlerpSpeed",
                                                    "celestialBody", "gravityForce", "stickToGroundGravity"};

    bool movementDataGroup;
    SerializedProperty[] movementData;
    string[] movementPropertyNames = new string[] { "playerMoveInput", "currentSpeed", "walkSpeed", "runSpeed",
                                                    "jumpForce"};

    bool groundCheckDataGroup;
    SerializedProperty[] groundCheckData;
    string[] groundCheckPropertyNames = new string[] { "useCustomGroundCheck", "playerIsGrounded", "groundCheckRadiusMultiplier",
                                                        "groundCheckDistance", "walkableMask", "feet", "capsuleCollider"};

    bool mouseSettingsDataGroup;
    SerializedProperty[] mouseSettingsData;
    string[] mouseSettingsPropertyNames = new string[] { "mouseSensitivity", "useInvertMouse" };

    bool cameraDataGroup;
    SerializedProperty[] camerasData;
    string[] cameraPropertyNames = new string[] { "firstPersonCamera", "firstPersonCameraPosition", "xRotationUpperLimit",
                                                    "xRotationLowerLimit"};

    bool otherDataGroup;
    SerializedProperty[] otherData;
    string[] otherPropertyNames = new string[] { "animator", "playerMass", "randomLocationAtStart", "planet"};


    private void OnEnable()
    {
        player = serializedObject.FindProperty("player");

        gravityData = new SerializedProperty[gravityPropertyNames.Length];
        InitializePropertys(gravityData, gravityPropertyNames);

        groundCheckData = new SerializedProperty[groundCheckPropertyNames.Length];
        InitializePropertys(groundCheckData, groundCheckPropertyNames);

        movementData = new SerializedProperty[movementPropertyNames.Length];
        InitializePropertys(movementData, movementPropertyNames);

        mouseSettingsData = new SerializedProperty[mouseSettingsPropertyNames.Length];
        InitializePropertys(mouseSettingsData, mouseSettingsPropertyNames);

        camerasData = new SerializedProperty[cameraPropertyNames.Length];
        InitializePropertys(camerasData, cameraPropertyNames);

        otherData = new SerializedProperty[otherPropertyNames.Length];
        InitializePropertys(otherData, otherPropertyNames);
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(player);


        gravityDataGroup = EditorGUILayout.BeginFoldoutHeaderGroup(gravityDataGroup, "Gravity settings");
        if (gravityDataGroup)
        {
            EditorGUILayout.PropertyField(gravityData[0]);
            if(gravityData[0].boolValue == true)
            {
                DisplayPropertys(gravityData, 1);
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        movementDataGroup = EditorGUILayout.BeginFoldoutHeaderGroup(movementDataGroup, "Movement settings");
        if (movementDataGroup)
        {
            DisplayPropertys(movementData);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        groundCheckDataGroup = EditorGUILayout.BeginFoldoutHeaderGroup(groundCheckDataGroup, "Ground check settings");
        if (groundCheckDataGroup)
        {
            EditorGUILayout.PropertyField(groundCheckData[0]);
            if (groundCheckData[0].boolValue == true)
            {
                DisplayPropertys(groundCheckData, 1);
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        mouseSettingsDataGroup = EditorGUILayout.BeginFoldoutHeaderGroup(mouseSettingsDataGroup, "Mouse settings");
        if (mouseSettingsDataGroup)
        {
            DisplayPropertys(mouseSettingsData);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        cameraDataGroup = EditorGUILayout.BeginFoldoutHeaderGroup(cameraDataGroup, "Camera settings");
        if (cameraDataGroup)
        {
            DisplayPropertys(camerasData);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();

        otherDataGroup = EditorGUILayout.BeginFoldoutHeaderGroup(otherDataGroup, "Other settings");
        if (otherDataGroup)
        {
            DisplayPropertys(otherData);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }

    private void InitializePropertys(SerializedProperty[] propertys, string[] names)
    {
        if(propertys.Length == names.Length)
        {
            for(int i = 0; i < propertys.Length; i++)
            {
                propertys[i] = serializedObject.FindProperty(names[i]);
            }
        }
    }

    private void DisplayPropertys(SerializedProperty[] propertys, int startIndex = 0)
    {
        for(int i = startIndex; i < propertys.Length; i++)
        {
            if (propertys[i] == null)
                continue;

            EditorGUILayout.PropertyField(propertys[i]);
        }
    }
}

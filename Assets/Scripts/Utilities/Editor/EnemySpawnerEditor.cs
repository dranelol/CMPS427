﻿using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        if (!Application.isPlaying)
        {
            EnemySpawner spawner = target as EnemySpawner;

            GUI.changed = false;
            spawner.enemyPrefab = EditorGUILayout.ObjectField("Enemy Prefab", spawner.enemyPrefab, typeof(GameObject), false) as GameObject;
            CheckDirty(spawner);

            GUI.changed = false;
            spawner.enemyCount = EditorGUILayout.IntSlider("Enemy Count", spawner.enemyCount, EnemySpawner.ENEMY_COUNT_MIN, EnemySpawner.ENEMY_COUNT_MAX);
            CheckDirty(spawner);

            GUI.changed = false;
            spawner.spawnRadius = Mathf.Clamp(EditorGUILayout.FloatField("Spawn Radius", spawner.spawnRadius), EnemySpawner.SPAWN_RADIUS_MIN, EnemySpawner.SPAWN_RADIUS_MAX);
            CheckDirty(spawner);

            GUI.changed = false;
            spawner.enemytype = EditorGUILayout.TextField("Enemy Type", spawner.enemytype);
            CheckDirty(spawner);

            GUI.changed = false;
            spawner.level = EditorGUILayout.IntSlider("Enemy Level", spawner.level, 1, 20);
            CheckDirty(spawner);

            EditorGUILayout.Space();

            GUI.changed = false;
            spawner.GenerateAppropriateOnTrigger = EditorGUILayout.Toggle("Generate Appropriate", spawner.GenerateAppropriateOnTrigger);
            CheckDirty(spawner);

            GUI.changed = false;
            spawner.critterPrefab = EditorGUILayout.ObjectField("critter Prefab", spawner.critterPrefab, typeof(GameObject), false) as GameObject;
            CheckDirty(spawner);
            GUI.changed = false;
            spawner.smallPrefab = EditorGUILayout.ObjectField("small Prefab", spawner.smallPrefab, typeof(GameObject), false) as GameObject;
            CheckDirty(spawner);
            GUI.changed = false;
            spawner.medPrefab = EditorGUILayout.ObjectField("med Prefab", spawner.medPrefab, typeof(GameObject), false) as GameObject;
            CheckDirty(spawner);
            GUI.changed = false;
            spawner.largePrefab = EditorGUILayout.ObjectField("large Prefab", spawner.largePrefab, typeof(GameObject), false) as GameObject;
            CheckDirty(spawner);

            EditorGUILayout.Space();

            GUI.changed = false;
            spawner.isStatic = EditorGUILayout.BeginToggleGroup("Always Generate", spawner.isStatic);
            CheckDirty(spawner);

            GUI.changed = false;
            spawner.spawnInterval = EditorGUILayout.IntSlider("Time Interval", spawner.spawnInterval, EnemySpawner.SPAWN_INTERVAL_MIN, EnemySpawner.SPAWN_INTERVAL_MAX);
            CheckDirty(spawner);

            GUI.changed = false;
            spawner.isTrigger = EditorGUILayout.Toggle("Trigger Spawn", spawner.isTrigger);
            CheckDirty(spawner);

            if (spawner.isTrigger)
            {
                GUI.changed = false;
                spawner.triggerRadius = Mathf.Clamp(EditorGUILayout.FloatField(_padding + "○ Radius", spawner.triggerRadius), EnemySpawner.TRIGGER_RADIUS_MIN, EnemySpawner.TRIGGER_RADIUS_MAX);
                CheckDirty(spawner);

            }

            EditorGUILayout.EndToggleGroup();
        }

    }

    private void CheckDirty(EnemySpawner script)
    {
        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);

        }
    }

    private string _padding = "     ";
}
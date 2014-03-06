using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
<<<<<<< HEAD
    private EnemySpawner spawner;

    public override void OnInspectorGUI()
    {
        try
        {
            spawner = target as EnemySpawner;

            spawner.enemyPrefab = EditorGUILayout.ObjectField("Enemy Prefab", spawner.enemyPrefab, typeof(GameObject), false) as GameObject;

            spawner.enemyCount = EditorGUILayout.IntSlider("Enemy Count", spawner.enemyCount, EnemySpawner.ENEMY_COUNT_MIN, EnemySpawner.ENEMY_COUNT_MAX);
            float _spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawner.spawnRadius);
            spawner.spawnRadius = Mathf.Clamp(_spawnRadius, EnemySpawner.SPAWN_RADIUS_MIN, EnemySpawner.SPAWN_RADIUS_MAX);

            EditorGUILayout.Space();

            spawner.isStatic = EditorGUILayout.BeginToggleGroup("Always Generate", spawner.isStatic);

            spawner.spawnInterval = EditorGUILayout.IntSlider("Time Interval", spawner.spawnInterval, EnemySpawner.SPAWN_INTERVAL_MIN, EnemySpawner.SPAWN_INTERVAL_MAX);

            spawner.isTrigger = EditorGUILayout.Toggle("Trigger Spawn", spawner.isTrigger);

            if (spawner.isTrigger)
            {
                spawner.trigger.enabled = true;
                float _triggerRadius = EditorGUILayout.FloatField(_padding + "○ Radius", spawner.triggerRadius);
                spawner.triggerRadius = Mathf.Clamp(_triggerRadius, EnemySpawner.TRIGGER_RADIUS_MIN, EnemySpawner.TRIGGER_RADIUS_MAX);
                spawner.trigger.radius = spawner.triggerRadius;
            }

            else
            {
                spawner.trigger.enabled = false;

                if (spawner.spawnCounter <= 0)
                {
                    spawner.spawnCounter = spawner.spawnInterval;
                }
=======
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

            float _spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawner.spawnRadius);

            GUI.changed = false;
            spawner.spawnRadius = Mathf.Clamp(_spawnRadius, EnemySpawner.SPAWN_RADIUS_MIN, EnemySpawner.SPAWN_RADIUS_MAX);
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
                float _triggerRadius = EditorGUILayout.FloatField(_padding + "○ Radius", spawner.triggerRadius);

                GUI.changed = false;
                spawner.triggerRadius = Mathf.Clamp(_triggerRadius, EnemySpawner.TRIGGER_RADIUS_MIN, EnemySpawner.TRIGGER_RADIUS_MAX);
                CheckDirty(spawner);
>>>>>>> upstream/master
            }

            EditorGUILayout.EndToggleGroup();
        }
<<<<<<< HEAD

        catch(Exception e)
        {
            //Debug.Log(e.ToString());
=======
    }

    private void CheckDirty(EnemySpawner script)
    {
        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
>>>>>>> upstream/master
        }
    }

    private string _padding = "     ";
}
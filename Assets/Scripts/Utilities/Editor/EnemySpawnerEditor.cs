using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
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
            }

            EditorGUILayout.EndToggleGroup();
        }

        catch
        {}
    }

    private string _padding = "     ";
}
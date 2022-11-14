using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class SetupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject personPrefab;
    [SerializeField] private int gridSize;
    [SerializeField] private int spread;
    [SerializeField] private Vector2 speedRange = new Vector2(4, 7);
    private BlobAssetStore blob;


    private void Start()
    {
        blob = new BlobAssetStore();
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob);
        var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(personPrefab, settings);
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;


        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                var instance = entityManager.Instantiate(entity);

                float3 position = new float3(x * spread, 0, z * spread);

                entityManager.SetComponentData(instance, new Translation
                {
                    Value = position
                });

                entityManager.SetComponentData(instance, new Destination
                {
                    Value = position
                });

                float speed = UnityEngine.Random.Range(speedRange.x, speedRange.y);
                entityManager.SetComponentData(instance, new MovementSpeed
                {
                    Value = speed
                });
            }
        }

        

    }

    private void OnDestroy()
    {
        blob.Dispose();
    }
}

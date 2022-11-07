using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class SetupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject personPrefab;
    [SerializeField] private int gridSize;
    [SerializeField] private int spread;
    private BlobAssetStore blob;


    private void Start()
    {
        blob = new BlobAssetStore();
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob);
        var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(personPrefab, settings);
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        entityManager.Instantiate(entity);

    }

    private void OnDestroy()
    {
        blob.Dispose();
    }
}

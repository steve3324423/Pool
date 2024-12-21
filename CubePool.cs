using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CubePool : MonoBehaviour
{
    [SerializeField] private CubeSpawned _cubePrefab;

    private ObjectPool<CubeSpawned> _pool;
    private List<CubeSpawned> _cubes;
    private bool _collectionChecks = true;
    private int _defaultCapacity = 10;
    private int _maxCapacity = 100;

    private void Awake()
    {
        _pool = new ObjectPool<CubeSpawned>(Create,OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, _collectionChecks, _defaultCapacity,_maxCapacity);
        _cubes = new List<CubeSpawned>();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void Start()
    {
        int countCubes = 10;
        for (int i = 0; i < countCubes; i++)
            _pool.Get();
    }

    private void OnDisable()
    {
        if(_cubes.Count < 1)
             return;

        foreach (CubeSpawned cube in _cubes)
            cube.DisableCube -= OnDisableCube;
    }

    private CubeSpawned Create()
    {
        int minValue = 0;
        int maxValue = 10;

        Vector3 randomPosition = new Vector3(Random.Range(minValue,maxValue),Random.Range(minValue,maxValue),Random.Range(minValue, maxValue));
        CubeSpawned cube = Instantiate(_cubePrefab,randomPosition,Quaternion.identity);
        _cubes.Add(cube);

        SubscribeEvents();
        return cube;
    }

    private void OnTakeFromPool(CubeSpawned cube)
    {
        cube.transform.position = Vector3.one;
        cube.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(CubeSpawned cube)
    {
        cube.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(CubeSpawned cube)
    {
        Destroy(cube.gameObject);
    }

    private void SubscribeEvents()
    {
        if (_cubes.Count < 1)
            return;

        foreach (CubeSpawned cube in _cubes)
            cube.DisableCube += OnDisableCube;
    }

    private void OnDisableCube(CubeSpawned cube)
    {
        _pool.Release(cube);
    }
}

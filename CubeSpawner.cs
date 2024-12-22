using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private CubeSpawned _cubePrefab;

    private ObjectPool<CubeSpawned> _pool;
    private List<CubeSpawned> _cubes;
    private WaitForSeconds _waitSeconds;
    private float _timeForCoroutine = 1f;
    private bool _collectionChecks = true;
    private int _defaultCapacity = 10;
    private int _maxCapacity = 100;

    private void Awake()
    {
        _pool = new ObjectPool<CubeSpawned>(Create,OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, _collectionChecks, _defaultCapacity,_maxCapacity);
        _cubes = new List<CubeSpawned>();

        _waitSeconds = new WaitForSeconds(_timeForCoroutine);
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void Start()
    {
        StartCoroutine(GetCubes());
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
        CubeSpawned cube = Instantiate(_cubePrefab,GetRandomPosition(),Quaternion.identity);
        _cubes.Add(cube);

        SubscribeEvents();
        return cube;
    }

    private void OnTakeFromPool(CubeSpawned cube)
    {
        cube.transform.position = GetRandomPosition();
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

    private Vector3 GetRandomPosition()
    {
        int minValue = 0;
        int maxValue = 10;

        return new Vector3(Random.Range(minValue, maxValue), Random.Range(minValue, maxValue), Random.Range(minValue, maxValue));
    }

    private IEnumerator GetCubes()
    {
        bool isCoroutineRun = true;

        while (isCoroutineRun)
        {
            _pool.Get();
            yield return _waitSeconds;
        }
    }
}

using System;
using UnityEngine;

public class CubeSpawned : MonoBehaviour
{
    private Renderer _renderer;
    private int _countCollision;
    private int _lifetime;

    public event Action<CubeSpawned> DisableCube;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void DisabledCube()
    {
        DisableCube?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        int minValueTime = 2;
        int maxValueTime = 5;

        if (_countCollision < 1)
            SetSolor();
       
        _lifetime = UnityEngine.Random.Range(minValueTime, maxValueTime);
        Invoke(nameof(DisabledCube), _lifetime);
        _countCollision++;
    }

    private void SetSolor()
    {
        int minValueColor = 0;
        int maxValueColor = 255;
        byte opacityValue = 0;

        byte randomValueColor = (byte)UnityEngine.Random.Range(minValueColor, maxValueColor);
        _renderer.material.color = new Color32(randomValueColor, randomValueColor, randomValueColor, opacityValue);
    }
}

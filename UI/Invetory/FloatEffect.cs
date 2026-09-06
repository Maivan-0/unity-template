using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    [Header("FloatEffect")]
    [SerializeField] private float floatStrength;
    [SerializeField] private float speed;

    private Vector2 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * floatStrength;
        transform.position = startPosition + new Vector2(0, offset);
    }
}

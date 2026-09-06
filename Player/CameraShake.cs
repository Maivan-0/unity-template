using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    [SerializeField] private float globalShakeForce;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CameraShakeForce(CinemachineImpulseSource impulseSource, Vector3 shakeDirection)
    {
        Vector3 impulse = shakeDirection * globalShakeForce;
        impulseSource.GenerateImpulse(impulse);
    }
}

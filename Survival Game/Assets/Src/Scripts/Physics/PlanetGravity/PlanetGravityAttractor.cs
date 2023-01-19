using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravityAttractor : MonoBehaviour
{
    [SerializeField, Range(-100f, 0f)]
    private float _gravity = -9.81f;
    [SerializeField, Range(-100f, 0f)]
    private float _minGravity = 0.0f;
    [SerializeField, Range(0f, 1000f)]
    private float _slerpTime = 10f;
    [SerializeField, Range(0f, 1000f)]
    private float _effectiveGravityRange = 10f;
    [SerializeField, Range(0f, 1000f)]
    private float _maxEffectiveGravityRange = 100f;

    public float Gravity { get { return _gravity; } }

    public float EffectiveGravityRange { get { return _effectiveGravityRange; } }

    public void Attract(Transform body, Rigidbody rb)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.up;

        /* TODO
         * Calculate gravity force using body distance
         */
        rb.AddForce(gravityUp * _gravity);

        /* TODO
         * Calculate rotation slerp using body distance
         */
        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;
        body.transform.rotation = Quaternion.Slerp(body.rotation, targetRotation, _slerpTime * Time.deltaTime);
    }
    public void Attract(Transform body, Rigidbody rb, float fallTime)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.up;

        /* TODO
         * Calculate gravity force using body distance
         */
        rb.AddForce(gravityUp * _gravity);

        /* TODO
         * Calculate rotation slerp using body distance
         */
        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;
        body.transform.rotation = Quaternion.Slerp(body.rotation, targetRotation, fallTime * Time.deltaTime);
    }


    public float GetCurrentGravityForce(float distance)
    {
        float finalGravityForce = _gravity;

        if(_effectiveGravityRange < distance)
        {
            if(_maxEffectiveGravityRange > distance)
            {
                float percent = distance / _maxEffectiveGravityRange;
                finalGravityForce = _gravity - (_gravity * percent);
            }
            else
            {
                finalGravityForce = 0.0f;
            }
        }

        return finalGravityForce;
    }
}

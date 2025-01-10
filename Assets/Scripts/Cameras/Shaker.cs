using UnityEngine;

public class Shaker : MonoBehaviour
{
    private float _duration = 0;
    private float _magnitude = 0;
    private bool _isShaking;
    private Vector2 _initialPosition;
    void Update()
    {
        if (_isShaking)
        {
            transform.localPosition = _initialPosition;
            
            if ( _duration <=  0 )
            {
                _duration = 0f;
                _magnitude = 0f;
                _isShaking = false;
                return;
            }

            Vector3 newPosition = new()
            {
                x = Random.Range(-1f, 1f) * _magnitude * _duration,
                y = Random.Range(-1f, 1f) * _magnitude * _duration,
                z = Random.Range(-1f, 1f) * _magnitude * _duration
            };

            transform.localPosition += newPosition;

            _duration -= Time.deltaTime;
        }
    }

    public void Shake(float duration, float magnitude)
    {
        _initialPosition = transform.localPosition;
        _duration = duration;
        _magnitude = magnitude;
        _isShaking = true;
    }
}
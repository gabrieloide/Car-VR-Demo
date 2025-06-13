using System;
using UnityEngine;

namespace Code
{
    public class Wiper : MonoBehaviour
    {
        private float _minRotationY = 0f;
        public float maxRotationY;
        [SerializeField] private float[] currentVelocity;
        public int wiperLevel = 0;
        private bool _rotatingUp = true;

        private void Start()
        {
            currentVelocity = new[]
            {
                0, 45f, 65f, 75f, 90f
            };
            _minRotationY = transform.eulerAngles.y;
        }

        private void Update()
        {
            float currentY = NormalizeAngle(transform.eulerAngles.y);


            if (wiperLevel > 0)
            {
                if (_rotatingUp)
                {
                    if (currentY >= maxRotationY)
                        _rotatingUp = false;
                }
                else
                {
                    if (currentY <= _minRotationY)
                        _rotatingUp = true;
                }

                float direction = _rotatingUp ? 1f : -1f;
                transform.Rotate(Vector3.up, currentVelocity[wiperLevel] * direction * Time.deltaTime, Space.Self);
            }
            else
            {
                float angleDiff = Mathf.DeltaAngle(currentY, _minRotationY);

                if (Mathf.Abs(angleDiff) > 0.1f)
                {
                    float step = currentVelocity[wiperLevel] * Time.deltaTime;
                    float direction = Mathf.Sign(angleDiff);
                    transform.Rotate(Vector3.up, step * direction, Space.Self);
                }
            }
        }

        private float NormalizeAngle(float angle)
        {
            angle %= 360f;
            if (angle < 0)
                angle += 360f;
            return angle;
        }
    }
}
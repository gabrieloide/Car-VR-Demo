using UnityEngine;

public class Wiper : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    public float maxRotationY;
    private float _minRotationY = 0f;

    [Header("Configuración de Velocidad")]
    [SerializeField] private float[] baseVelocities = { 0f, 45f, 65f, 75f, 90f };
    
    [Tooltip("0 = Apagado, 1-4 = Velocidades")]
    public int wiperLevel = 0;

    private bool _rotatingUp = true;

    private void Awake()
    {
        _minRotationY = transform.localEulerAngles.y;
    }

    public void RotateWiper(float currentSpeed)
    {
        float targetAngle;
        if (wiperLevel > 0)
        {
            targetAngle = _rotatingUp ? maxRotationY : _minRotationY;
        }
        else
        {
            targetAngle = _minRotationY;
        }

        float currentY = transform.localEulerAngles.y;
        float angleDifference = Mathf.DeltaAngle(currentY, targetAngle);

        if (Mathf.Abs(angleDifference) < 0.1f)
        {
            if (wiperLevel > 0)
            {
                _rotatingUp = !_rotatingUp;
            }
            return;
        }

        float direction = Mathf.Sign(angleDifference);
        float step = currentSpeed * Time.deltaTime;
        float rotationThisFrame = Mathf.Min(step, Mathf.Abs(angleDifference));

        transform.Rotate(Vector3.up, rotationThisFrame * direction, Space.Self);
    }

    public float GetSpeedForLevel(int level)
    {
        if (level >= 0 && level < baseVelocities.Length)
        {
            return baseVelocities[level];
        }
        return 0f;
    }
}
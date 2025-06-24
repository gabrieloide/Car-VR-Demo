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

    [Header("Sincronización (Opcional)")]
    [Tooltip("Arrastra aquí el otro Wiper para que este se sincronice con él. Déjalo vacío si este es el Wiper principal o 'maestro'.")]
    public Wiper masterWiper;

    private float[] _syncedVelocities;
    private bool _rotatingUp = true;

    private void Awake()
    {
        _minRotationY = transform.localEulerAngles.y;
        CalculateSyncedVelocities();
    }

    private void CalculateSyncedVelocities()
    {
        _syncedVelocities = new float[baseVelocities.Length];

        if (masterWiper == null)
        {
            // Soy el MAESTRO
            for (int i = 0; i < baseVelocities.Length; i++)
            {
                _syncedVelocities[i] = baseVelocities[i];
            }
        }
        else
        {
            // Soy el ESCLAVO
            for (int i = 0; i < baseVelocities.Length; i++)
            {
                float masterVelocity = masterWiper.baseVelocities[i];
                float masterRotation = masterWiper.maxRotationY;
                
                if (masterRotation > 0)
                {
                    _syncedVelocities[i] = masterVelocity * (this.maxRotationY / masterRotation);
                }
                else
                {
                    _syncedVelocities[i] = 0;
                }
            }
        }
    }

    // --- SECCIÓN DE UPDATE MODIFICADA ---
    private void Update()
    {
        // 1. Determinar el ángulo objetivo
        float targetAngle;
        if (wiperLevel > 0)
        {
            targetAngle = _rotatingUp ? maxRotationY : _minRotationY;
        }
        else
        {
            // Si está apagado, siempre vuelve al inicio
            targetAngle = _minRotationY;
        }

        // 2. Calcular la distancia angular restante usando DeltaAngle (maneja el ciclo 360->0)
        float currentY = transform.localEulerAngles.y;
        float angleDifference = Mathf.DeltaAngle(currentY, targetAngle);

        // Si la diferencia es muy pequeña, consideramos que ya llegamos.
        if (Mathf.Abs(angleDifference) < 0.1f)
        {
            // Si llegamos al destino Y el wiper debería estar moviéndose, invertimos la dirección.
            if (wiperLevel > 0)
            {
                _rotatingUp = !_rotatingUp;
            }
            return; // No hay más que hacer en este frame.
        }

        // 3. Determinar la dirección de la rotación (1 o -1)
        float direction = Mathf.Sign(angleDifference);

        // 4. Calcular el máximo paso de rotación posible en este frame
        // (Si el nivel es 0, usamos la velocidad del nivel 1 para que pueda regresar a su posición)
        int velocityIndex = (wiperLevel == 0) ? 1 : wiperLevel;
        float step = _syncedVelocities[velocityIndex] * Time.deltaTime;

        // 5. La cantidad a rotar es el paso, pero NUNCA más grande que la distancia restante.
        // Esto previene que se pase del límite (el efecto de MoveTowardsAngle).
        float rotationThisFrame = Mathf.Min(step, Mathf.Abs(angleDifference));

        // 6. Aplicar la rotación usando tu método preferido.
        transform.Rotate(Vector3.up, rotationThisFrame * direction, Space.Self);
    }
}
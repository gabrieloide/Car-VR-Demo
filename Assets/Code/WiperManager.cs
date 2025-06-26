using UnityEngine;

public class WiperManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Wiper wiperLeft;
    [SerializeField] private Wiper wiperRight;
    [SerializeField] private SteeringWheelRightControl steeringWheelRightControl;

    private int _currentWiperLevel = 0;

    private void Update()
    {
        if (_currentWiperLevel > 0)
        {
            wiperLeft.wiperLevel = _currentWiperLevel;
            wiperRight.wiperLevel = _currentWiperLevel;
            wiperLeft.RotateWiper(wiperLeft.GetSpeedForLevel(_currentWiperLevel));
            wiperRight.RotateWiper(wiperRight.GetSpeedForLevel(_currentWiperLevel));
        }
        else
        {
            // Si el nivel es 0 (apagado), asegúrate de que regresen a la posición inicial
            wiperLeft.wiperLevel = 0;
            wiperRight.wiperLevel = 0;
            wiperLeft.RotateWiper(wiperLeft.GetSpeedForLevel(1)); // Usar velocidad 1 para regresar
            wiperRight.RotateWiper(wiperRight.GetSpeedForLevel(1)); // Usar velocidad 1 para regresar
        }
    }

    // Método para cambiar el nivel de los limpiaparabrisas desde SteeringWheelRightControl
    public void ChangeWiperLevel(int change)
    {
        _currentWiperLevel = Mathf.Clamp(_currentWiperLevel + change, 0, 4);
    }
}
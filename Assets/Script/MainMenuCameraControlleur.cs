using UnityEngine;
using Cinemachine = Unity.Cinemachine;
public class MainMenuCameraControlleur : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineOrbitalFollow orbitalFollow;
    [SerializeField] private float rotationSpeed = 1f;
    // Update is called once per frame
    void Update()
    {
        orbitalFollow.HorizontalAxis.Value += rotationSpeed * Time.deltaTime;
    }
}

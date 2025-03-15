using System;
using UnityEngine;

public class TrailRendererHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TrailRenderer[] trailRenderers;

    private void Awake()
    {
        foreach (TrailRenderer trail in trailRenderers)
        {
            trail.emitting = false; //they already are false in the inspector, but just in case
        }
    }

    private void Update()
    {
        if (playerController.IsDrifting(out float lateralVelocity))
        {
            foreach (TrailRenderer trail in trailRenderers)
            {
                trail.emitting = true;
            }
        }
        else
        {
            foreach (TrailRenderer trail in trailRenderers)
            {
                trail.emitting = false;
            }
        }
    }
}

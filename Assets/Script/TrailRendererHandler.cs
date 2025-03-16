using System;
using System.Collections;
using UnityEngine;

public class TrailRendererHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController carController;
    [SerializeField] private TrailRenderer[] trailRenderers;
    
    float timeSpendDrifting = 0f;

    private void Awake()
    {
        foreach (TrailRenderer trail in trailRenderers)
        {
            trail.emitting = false; //they already are false in the inspector, but just in case
        }
    }

    private void Update()
    {
        if (carController.IsDrifting())
        {
            if(carController.isPlayer) ScoreManager.Instance.IncreaseScore(1); //Increase the score when the player is drifting
            foreach (TrailRenderer trail in trailRenderers)
            {
                trail.emitting = true;
            }
            timeSpendDrifting += Time.deltaTime;
        }
        else
        {
            //if(carController.isPlayer) ScoreManager.Instance.IncreaseScore((int)(timeSpendDrifting*100)); //Increase the score when the player boost after a drift
            foreach (TrailRenderer trail in trailRenderers)
            {
                trail.emitting = false;
            }
            if(timeSpendDrifting > 2f)
            {
                StartCoroutine(carController.BoostPostDrift(timeSpendDrifting / 2));
            }
            timeSpendDrifting = 0;
        }
    }
}

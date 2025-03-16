using UnityEngine;
using System.Collections;
using Cinemachine = Unity.Cinemachine;

public class CameraShakeHandler : MonoBehaviour
{
    //reference to the cinemachine camera
    [SerializeField] private Cinemachine.CinemachineBasicMultiChannelPerlin noise;
    public static CameraShakeHandler Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    public void ShakeCamera(float intensity, float time)
    {
        //create a noise profile
        noise.AmplitudeGain = intensity;
        noise.FrequencyGain = 1f;
        
        //start a coroutine to stop the noise after a certain amount of time
        StartCoroutine(StopShake(intensity, time));
    }

    IEnumerator StopShake(float intensity, float time)
    {
        //lerp the intensity to 0
        while (intensity > 0)
        {
            intensity -= Time.deltaTime / time;
            noise.AmplitudeGain = intensity;
            yield return null;
        }
        noise.AmplitudeGain = 0;
    }
}

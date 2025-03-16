using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    public static PostProcessManager Instance { get; private set; }
    [SerializeField] private Volume volume;
    private ChromaticAberration chromaticAberration;
     
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
     
    public void SetChromaticAberration(float value, float time)
    {
        StartCoroutine(ChangeChromaticAberration(value, time));
    }
     
    private IEnumerator ChangeChromaticAberration(float value, float time)
    {
        float t = 0f;
        volume.profile.TryGet(out chromaticAberration); //not sure if i have to do this every time, i'll check later
        float startValue = chromaticAberration.intensity.value;
        while (t < time)
        {
            t += Time.deltaTime;
            chromaticAberration.intensity.value = Mathf.Lerp(startValue, value, t / time);
            yield return null;
        }
    }
    
}

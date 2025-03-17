using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    public static PostProcessManager Instance { get; private set; }
    [SerializeField] private Volume volume;
    private ChromaticAberration chromaticAberration;
    
    private bool chromaticAberrationOn;
     
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
        chromaticAberrationOn = PlayerPrefs.GetInt("ChromaticAberration", 1) == 1;
    }
     
    public void SetChromaticAberration(float value, float time)
    {
        if (!chromaticAberrationOn) return;
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

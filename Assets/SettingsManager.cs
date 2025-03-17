using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle ChromaticAberrationToggle;
    [SerializeField] private Toggle ScreenShakeToggle;
    
    private void Start()
    {
        ChromaticAberrationToggle.isOn = PlayerPrefs.GetInt("ChromaticAberration", 1) == 1;
        ScreenShakeToggle.isOn = PlayerPrefs.GetInt("ScreenShake", 1) == 1;
    }
    
    public void SetChromaticAberration(bool value)
    {
        PlayerPrefs.SetInt("ChromaticAberration", value ? 1 : 0);
    }
    
    public void SetScreenShake(bool value)
    {
        PlayerPrefs.SetInt("ScreenShake", value ? 1 : 0);
    }
    
}

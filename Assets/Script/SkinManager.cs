using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [SerializeField] private MeshFilter carMeshfilter;
    [SerializeField] private Mesh[] carMeshes;
    [SerializeField] private GameObject skinPanel;
    
    private void Start()
    {
        int carSkin = PlayerPrefs.GetInt("CarSkin", 0);
        carMeshfilter.mesh = carMeshes[carSkin];
    }
    
    public void ChangeCarSkin(int index)
    {
        carMeshfilter.mesh = carMeshes[index];
        PlayerPrefs.SetInt("CarSkin", index);
        
        if(skinPanel)skinPanel.SetActive(false); //done so we can use the same script for both scenes
    }
}

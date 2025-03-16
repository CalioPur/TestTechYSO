using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class GameStartHandler : MonoBehaviour
{
    [SerializeField] private Transform enemiesHolder;
    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] private TextMeshProUGUI countDownText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        countDownText.text = "3";
        yield return new WaitForSeconds(1f);
        countDownText.text = "2";
        yield return new WaitForSeconds(1f);
        countDownText.text = "1";
        yield return new WaitForSeconds(1f);
        countDownText.text = "GO!";
        playerInputHandler.startPlayerInput();
        foreach (Transform ai in enemiesHolder)
        {
            ai.GetComponent<AIInputHandler>().StartCarsInput(); //expensive but done on start and only once
        }

        //fade the "go"
        float t = 0;
        while (t < 0.5)
        {
            t += Time.deltaTime;
            countDownText.color = new Color(countDownText.color.r, countDownText.color.g, countDownText.color.b,
                1 - t * 2);
            yield return null;
        }
        //we disable the text and reset the color alpha
        countDownText.gameObject.SetActive(false);
        countDownText.color = new Color(countDownText.color.r, countDownText.color.g, countDownText.color.b, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using naichilab.EasySoundPlayer.Scripts;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] Canvas canvasCountDown;
    [SerializeField] Image countDownImage;
    [SerializeField] TextMeshProUGUI countDownText;
    [SerializeField] GameObject gameManager;
    [SerializeField] GameObject boxManager;
    private int countDownCount;
    //?o???????B
    private float countDownElapsedTime;
    //?J?E???g?_?E?????????B??????????3?b?B
    private float countDownDuration = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        canvasCountDown.gameObject.SetActive(true);
        StartCoroutine("CountDown");
        SePlayer.Instance.Play(1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CountDown()
    {
        countDownCount = 0;
        countDownElapsedTime = 0;

        //?????A?????I????GameObject?????Q?????????????????????????X?????????v?????????B
        canvasCountDown.gameObject.SetActive(true);

        //?e?L?X?g???X?V?B
        countDownText.text = System.String.Format("{0}", Mathf.FloorToInt(countDownDuration));

        while (true)
        {
            countDownElapsedTime += Time.deltaTime;

            //?~?`?X???C?_?[???X?V?BfillAmount??0?`1.0f???????w???????B?o?????????????_???????l?????????????B
            countDownImage.fillAmount = countDownElapsedTime % 1.0f;

            if (countDownCount < Mathf.FloorToInt(countDownElapsedTime))
            {
                //1?b???????J?E???g?B
                countDownCount++;
                //?e?L?X?g???X?V?B
                countDownText.text = System.String.Format("{0}", Mathf.FloorToInt(countDownDuration - countDownCount));
            }

            if (countDownDuration <= countDownElapsedTime)
            {
                //?J?E???g?_?E???I??
                countDownImage.fillAmount = 0f;
                countDownText.text = "Start!";
                yield return new WaitForSeconds(1.0f);
                canvasCountDown.gameObject.SetActive(false);
                GameStart();
                yield break;
            }

            yield return null;
        }
    }

    public void GameStart()
    {
        gameManager.GetComponent<gameManager>().timeFlag = true;
        boxManager.GetComponent<boxManager>().MakeBoxrows();
    }
}
    

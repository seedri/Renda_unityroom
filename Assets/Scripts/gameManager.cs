using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using static System.TimeZoneInfo;
using naichilab.EasySoundPlayer.Scripts;
using unityroom.Api;

public class gameManager : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject boxManager;
    [SerializeField] GameObject gameOverObj;
    [SerializeField] GameObject gameClearObj;
    [SerializeField] Canvas canvasCountDown;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI timeClear;

    //?^?C???A?^?b?N?p????
    private float _time = 0f;
    public bool timeFlag = false;
    //表示時間の初期位置
    private float shakeTime = 0.5f;
    private Vector3 timePos;
    private Tweener shakeTweener;
    //フリック用の開始地点と終了地点
    private Vector3 tapStartPos;
    private Vector3 tapEndPos;
    //フリックの判定距離
    private const float FlickDistance = 200.0f;
    //スワイプ用の現在地点
    private Vector3 tapNowPos;
    //カメラマネージャスクリプト
    cameraManager _cameraManager;
    //ミスした時のペナルティ秒
    private const int MissTime = 2;


    public float time
    {
        get{ return this._time; }

        set { this._time = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        canvasCountDown.gameObject.SetActive(true);

        //開発用にカウントダウンを飛ばした処理
        //canvasCountDown.GetComponent<GameStartManager>().GameStart();

        //時間表示の初期位置を保持
        timePos = timeText.transform.localPosition;

        //スワイプ用の初期取得
        _cameraManager = mainCamera.GetComponent<cameraManager>();
    }

    // Update is called once per frame
    void Update()
    {

        //時間計測と表示
        if (timeFlag)
        {
            _time += Time.deltaTime;
            timeText.text = _time.ToString("F2");
        }

        //ボタンクリックの時は以下の処理を行わない
        if (EventSystem.current.IsPointerOverGameObject()){
            return;
        }
        //実機対応の場合はこっち
        /*
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            return;
        }
        */

        //フリックの判定
        if (Input.GetMouseButtonDown(0))
        {
            tapStartPos = Input.mousePosition;
        }
        //スワイプの判定
        else if (Input.GetMouseButton(0))
        {
            tapNowPos = Input.mousePosition;
            MakeSwipe();
        }
        //指を離した時にだけフリック動作かの判定および処理を行う
        if (Input.GetMouseButtonUp(0))
        {
            tapEndPos = Input.mousePosition;
            //mainCamera.transform.position = tapStartPos;
            IsFlicked();
        }
    }

    public void GameClear()
    {
        timeFlag = false;
        timeClear.text = "タイム:" + timeText.text;
        gameClearObj.SetActive(true);
        int thisScene = 1;
        if (SceneManager.GetActiveScene().name == "GameScene2") thisScene = 2; 
            UnityroomApiClient.Instance.SendScore(thisScene,_time,ScoreboardWriteMode.HighScoreAsc);
        SePlayer.Instance.Play(4);
    }

    //「次へ」ボタンを押した時の処理
    public void PushNextButton()
    {
        if (!boxManager.GetComponent<boxManager>().IsrowNull()) AddError();
        else if (boxManager.GetComponent<boxManager>().CanClear()) GameClear();

        else
        {
            _cameraManager.cameraMove();
            boxManager.GetComponent<boxManager>().goNextrow();
        }    
    }

    //フリックの判定および処理
    private void IsFlicked()
    {
        Vector3 dif = tapEndPos - tapStartPos;
        float dif_x = dif.x * -1;
        if (dif_x < 0) return;
        else
        {
            //指定した閾値より大きく、かつ次の列に行って良い場合、フリック判定
            if (dif_x > FlickDistance)
            {
                if (boxManager.GetComponent<boxManager>().IsrowNull())
                {
                    _cameraManager.cameraMove();
                    boxManager.GetComponent<boxManager>().goNextrow();
                }
                //フリックしたのに列が空じゃない時、エラーして戻す
                else
                {
                    AddError();
                    _cameraManager.CameraReturn();
                }
                //ゲームクリアの時
                if (boxManager.GetComponent<boxManager>().CanClear())
                {
                    GameClear();
                }
            }
            //フリックじゃなければ戻す
            else
            {
                _cameraManager.CameraReturn();
            }
        }
    }

    //スワイプの判定および処理
    private void MakeSwipe()
    {
        Vector3 dif = tapNowPos - tapStartPos;
        float dif_x = -1*dif.x;
        if (dif_x < 0) return;
        else
        {
            //スワイプした距離に応じて移動させる
            _cameraManager.MakeSwipeMove(dif_x);
        }

    }

    //時間表示のテキストを揺らす
    public void ShakeTime()
    {
        //前回の連打が残っていれば停止して初期位置に戻す
        if (shakeTweener != null)
        {
            shakeTweener.Kill();
            timeText.transform.localPosition = timePos;
        }
        shakeTweener = timeText.transform.DOShakePosition(shakeTime,30f);

        //色の処理
        StartCoroutine(TimeColorChange());
    }

    //表示時間の色を変えて戻す
    private IEnumerator TimeColorChange()
    {
        float elapsedTime = 0.0f;
        float colorChangetime = shakeTime / 2;
        Color currentColor;

        while (elapsedTime < shakeTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / colorChangetime;
            if (elapsedTime < colorChangetime)
            {
                currentColor = Color.Lerp(Color.white, Color.red, t);
            }
            else
            {
                currentColor = Color.Lerp(Color.red, Color.white, t-1);
            }
           timeText.color = currentColor;
           yield return null;
        }
    }

    //ペナルティを行う処理
    public void AddError()
    {
        ShakeTime();
        this._time += MissTime;
        SePlayer.Instance.Play(2);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class boxManager : MonoBehaviour
{
    [SerializeField] private GameObject[] boxPrefabs;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private int totalBox = 60;
    [SerializeField] private int maxBox = 10;
    [SerializeField] private int minBox = 1;
    [SerializeField] private AudioClip tapSound;
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] TextMeshProUGUI boxRemainText;
    private int boxNum;
    private int boxRemain;
    private int[] boxRemainRow;
    private GameObject[,] boxes;
    //現在の行
    private int lastCube = 0;
    //現在の列
    private int row = 0;
    private int lastrow=100;
    private int num;

    //列の横幅
    private float _boxX = 3.5f;
    //積み上げる箱の生成する縦の幅
    private float _boxY = 0.8f;

    private gameManager _gameManager;

    public float boxX
    {
        get { return this._boxX; }
        private set { this._boxX = value; }
    }

    public float boxY
    {
        get { return this._boxY; }
        private set { this._boxY = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = gameManager.GetComponent<gameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //下ボタンが連打ボタンに対応
        if (Input.GetKeyDown(KeyCode.DownArrow)){
            PushRendaRed();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _gameManager.PushNextButton();
        }
    }

    //最初に行う箱の列の生成
    public void MakeBoxrows()
    {

        float x = 0;
        float y = boxY;

        //最大の列数を計算
        int maximumrow = (int)Mathf.Ceil((float)(totalBox - maxBox) / minBox + 1);

        //箱を管理する二次元配列を用意      
        boxes = new GameObject[maxBox + 1, maximumrow];

        boxRemainRow = new int[maximumrow];
        while (totalBox > 0)
        {
            //その列における箱の数を決める
            boxNum = Mathf.Min(Random.Range(minBox, maxBox), totalBox);

            //実際に詰んだ箱の列を生成
            for (int j = 0; j < boxNum; j++)
            {
                num = Random.Range(0, boxPrefabs.Length);
                GameObject cube = Instantiate(boxPrefabs[num], new Vector3(x, y, 0), Quaternion.identity);
                y += 1.3f;
                boxes[j, row] = cube;
            }
            x += boxX;
            y = boxY;
            //どの列に何箱あるかを記録
            boxRemainRow[row] = boxNum;
            row++;
            totalBox -= boxNum;
            boxRemain += boxNum;
        }
        //今回の列数を記録してから列の情報を初期化
        lastrow = row - 1;
        row = 0;

        BoxRemainDisp();

    }

    //赤いボタンを押した時の処理
    //現在の使用はボタンの色は赤のみなので、こちらを改変
    public void PushRendaRed()
    {
        //連打を必要以上にしたらゲームオーバー
        if (IsrowNull())
        {
            gameManager.GetComponent<gameManager>().AddError();
        }
        //箱をどかして消去する処理
        else
        {
            if (boxes[lastCube, row].tag == "BoxRed")
            {
                //boxes[lastCube, row].transform.DOMove(new Vector3(-4f, 0, 0), 0.15f).SetRelative(true).SetEase(Ease.OutQuint);
                audioSource.PlayOneShot(tapSound);
                Destroy(boxes[lastCube, row]);
                lastCube++;
                boxRemain--;
                boxRemainRow[row]--;
                BoxRemainDisp();

            }
            //違う色のボタンを押した時
            else
            {
                gameManager.GetComponent<gameManager>().AddError();
            }
        }
    }




    //黄色いボタンを押した時の処理
    //public void PushRendaYellow()
    //{
    //    //連打を必要以上にしたらゲームオーバー
    //    if (IsrowNull())
    //    {
    //        gameManager.GetComponent<gameManager>().AddError();
    //    }
    //    //箱をどかして消去する処理
    //    else
    //    {
    //        if (boxes[lastCube, row].tag == "BoxYellow")
    //        {
    //            boxes[lastCube, row].transform.DOMove(new Vector3(-4f, 0, 0), 0.15f).SetRelative(true).SetEase(Ease.OutQuint);
    //            audioSource.PlayOneShot(tapSound);
    //            Destroy(boxes[lastCube, row], 0.2f);
    //            lastCube++;
    //            boxRemain--;
    //            boxRemainRow[row]--;
    //        }
    //        //違う色のボタンを押した時
    //        else
    //        {
    //            gameManager.GetComponent<gameManager>().AddError();
    //        }
    //    }
    //}


    //残りの箱の個数表示を更新
    private void BoxRemainDisp()
    {
        boxRemainText.text = "残り:" + boxRemain+"個";
    }


    public void goNextrow()
    {
        row++;
        lastCube = 0;
        audioSource.PlayOneShot(moveSound);
    }

    //現在の列を終えて次の列に行くことができるかどうか判定
    public bool IsrowNull()
    {
        if (boxRemainRow[row]==0) return true;
        else return false;
    }


    //クリア判定
    public bool CanClear()
    {
        //残りの箱がないなら、クリア
        if (boxRemain == 0)
        {
            return true;
        }
        else return false;
    }

}

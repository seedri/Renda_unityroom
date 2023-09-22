using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class cameraManager : MonoBehaviour
{
    [SerializeField] private boxManager boxManager;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip flickSound;
    //現在のカメラ位置
    private Vector3 _cameraPos;
    //スワイプのスピード
    private const float SwipeSpped = 0.01f;

    public Vector3 cameraPos
    {
        get { return this._cameraPos; }

        set { this._cameraPos = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _cameraPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //カメラを次の列に移動させる
    public void cameraMove()
    {
        this.transform.DOMove(new Vector3(_cameraPos.x + boxManager.boxX, 0f, -10f), 0.3f).SetRelative(false).SetEase(Ease.OutBack);
        //カメラの現在位置を更新
        _cameraPos.x += boxManager.boxX;
        audioSource.PlayOneShot(flickSound);
    }

    public void PushNextButton()
    {
        cameraMove();
    }

    //毎フレーム実行するスワイプ量に応じてカメラを移動させる処理
    public void MakeSwipeMove(float dif_x)
    {
        this.transform.position = _cameraPos + new Vector3(SwipeSpped * dif_x, 0, 0);
    }

    //カメラを元の位置に戻す処理
    public void CameraReturn()
    {
        this.transform.DOMove(_cameraPos, 0.3f).SetRelative(false).SetEase(Ease.OutBack);
    }
}

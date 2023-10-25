using naichilab.EasySoundPlayer.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadStage1()
    {
        SceneManager.LoadScene("GameScene1");
        //BgmPlayer.Instance.Stop();
        BgmPlayer.Instance.Play(1);
    }

    public void LoadStage2()
    {
        SceneManager.LoadScene("GameScene2");
        //BgmPlayer.Instance.Stop();
        BgmPlayer.Instance.Play(1);
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene("TitleScene");
        //BgmPlayer.Instance.Stop();
        BgmPlayer.Instance.Play(0);
    }
}

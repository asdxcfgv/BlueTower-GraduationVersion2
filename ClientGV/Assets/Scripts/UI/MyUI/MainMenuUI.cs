using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    #region Header OBJECT REFERENCES
    [Space(10)]
    [Header("UI组件引用")]
    #endregion Header OBJECT REFERENCES
    #region Tooltip
    [Tooltip("开始按钮")]
    #endregion Tooltip
    [SerializeField] private Button Btn_Start;
    #region Tooltip
    [Tooltip("结束按钮")]
    #endregion
    [SerializeField] private Button Btn_Exit;

    void Awake()
    {
        Btn_Start.onClick.AddListener(StartGame);
        Btn_Exit.onClick.AddListener(ExitGame);
    }

    private void Start()
    {
        // Play Music
        MusicManager.Instance.PlayMusic(GameResources.Instance.mainMenuMusic, 0f, 2f);
    }


    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

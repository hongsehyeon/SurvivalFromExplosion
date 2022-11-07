using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Michsky.UI.ModernUIPack;

public class StartScene : MonoBehaviour
{
    public GameObject Title;
    public GameObject NamePanel;
    public CustomInputField NameInputField;

    private void Start()
    {
        if (PlayerPrefs.GetString("PlayerName") == "")
            StartCoroutine(ShowNamePanel());
        else
            StartCoroutine(ManagerInit(4));
    }

    public void OnClickStartButton()
    {
        string name = NameInputField.inputText.text;
        if (SetPlayerName() == false)
        {
            // TODO : 닉네임 등록 실패 UI
            Debug.Log("닉네임 등록 실패");
            return;
        }

        StartCoroutine(ManagerInit(0));
    }

    private IEnumerator ManagerInit(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (Managers.Network == null) { }
    }

    private IEnumerator ShowNamePanel()
    {
        yield return new WaitForSeconds(4);
        Title.SetActive(false);
        NamePanel.SetActive(true);
    }

    private bool SetPlayerName()
    {
        // TODO : 닉네임 유효성 검사

        PlayerPrefs.SetString("PlayerName", name);
        return true;
    }
}

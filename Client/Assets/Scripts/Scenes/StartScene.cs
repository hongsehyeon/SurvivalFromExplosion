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
            StartCoroutine(ManagerInit());
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

        StartCoroutine(ManagerInit());
    }

    private IEnumerator ManagerInit()
    {
        if (Managers.Network == null) { }
        yield return null;
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

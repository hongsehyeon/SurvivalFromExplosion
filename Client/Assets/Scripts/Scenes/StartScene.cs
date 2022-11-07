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
            // TODO : �г��� ��� ���� UI
            Debug.Log("�г��� ��� ����");
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
        // TODO : �г��� ��ȿ�� �˻�

        PlayerPrefs.SetString("PlayerName", name);
        return true;
    }
}

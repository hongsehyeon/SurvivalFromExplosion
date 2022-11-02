using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public GameObject Title;
    public GameObject NamePanel;

    private void Start()
    {
        StartCoroutine(ShowNamePanel());
    }

    public void ManagerInit()
    {
        if (Managers.Network == null) { }
        SceneManager.LoadScene("Game");
    }

    private IEnumerator ShowNamePanel()
    {
        yield return new WaitForSeconds(5);
        Title.SetActive(false);
        NamePanel.SetActive(true);
    }
}

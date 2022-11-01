using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(ManagerInit());
    }

    private IEnumerator ManagerInit()
    {
        yield return new WaitForSeconds(5f);
        if (Managers.Network == null) { }
        SceneManager.LoadScene("Game");
    }
}

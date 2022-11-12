using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RankScene : MonoBehaviour
{
    public GameObject ScrollContent;
    public GameObject RankItemPrefab;

    private void Start()
    {
        
    }

    public void OnClickRefreshRankList()
    {

    }

    public void OnClickBack()
    {
        SceneManager.LoadScene("Lobby");
    }
}

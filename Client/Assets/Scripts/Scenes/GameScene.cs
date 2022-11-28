using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public Transform InfoContent;
    public GameObject PlayerInfoPrefab;
    public List<PlayerInfoItem> playerInfoList = new List<PlayerInfoItem>();

    private void Start()
    {
        
    }

    public void GameStart()
    {

    }

    public void AddPlayerInfo(string name)
    {
        GameObject go = Instantiate(PlayerInfoPrefab, InfoContent);
        PlayerInfoItem playerInfoItem = go.GetComponent<PlayerInfoItem>();
        playerInfoItem.Name = name;
        playerInfoList.Add(playerInfoItem);
    }

    public void UpdateUI()
    {
        
    }
}

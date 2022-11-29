using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public Transform InfoContent;
    public GameObject PlayerInfoPrefab;
    public Dictionary<int, PlayerInfoItem> PlayerInfos = new Dictionary<int, PlayerInfoItem>();

    private void Start()
    {
        
    }

    public void GameStart()
    {
        RegisterPlayers();
    }

    public void RegisterPlayers()
    {
        Dictionary<int, GameObject> players = Managers.Object.Objects;

        foreach (GameObject player in players.Values)
        {
            if (player.TryGetComponent(out PlayerController pc))
            {
                PlayerInfoItem item = Instantiate(PlayerInfoPrefab, InfoContent).GetComponent<PlayerInfoItem>();
                item.Name = pc.Name;
                item.Score = 0;
            }
        }
    }

    public void UpdateUI()
    {

    }
}

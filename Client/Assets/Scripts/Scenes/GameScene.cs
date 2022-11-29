using Google.Protobuf.Collections;
using Google.Protobuf.Protocol;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public Transform InfoContent;
    public GameObject PlayerInfoPrefab;
    public Dictionary<int, PlayerInfoItem> PlayerInfos = new Dictionary<int, PlayerInfoItem>();
    public GameObject StartButton;

    private void Start()
    {
        if (Managers.Object.Objects.Count <= 1)
            StartButton.SetActive(true);
    }

    public void OnClickGameStart()
    {
        C_StartGame startGamePacket = new C_StartGame();
        Managers.Network.Send(startGamePacket);
    }

    public void GameStart()
    {
        StartButton.SetActive(false);
        RegisterPlayers();
    }

    public void RegisterPlayers()
    {
        Dictionary<int, GameObject> players = Managers.Object.Objects;

        foreach (GameObject player in players.Values)
        {
            if (player.TryGetComponent(out PlayerController pc))
            {
                GameObject go = Instantiate(PlayerInfoPrefab, InfoContent);
                if (go.TryGetComponent(out PlayerInfoItem item))
                {
                    item.Name = pc.Name;
                    item.Score = 0;
                    item.InfoText.text = $"{item.Name} : {item.Score}";
                    PlayerInfos.Add(pc.Id, item);
                }
            }
        }
    }

    public void UpdateUI(RepeatedField<int> playerIds)
    {
        foreach (int playerId in playerIds)
        {
            if (PlayerInfos.TryGetValue(playerId, out PlayerInfoItem item))
                item.Score++;
        }
    }
}

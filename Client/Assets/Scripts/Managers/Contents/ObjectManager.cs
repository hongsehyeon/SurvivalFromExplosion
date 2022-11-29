using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    public MyPlayerController MyPlayer { get; set; }
    public Exploder Exploder { get; set; }
    public Dictionary<int, GameObject> Objects { get; private set; } = new Dictionary<int, GameObject>();

    public void Add(ObjectInfo info, bool myPlayer = false)
    {
        if (myPlayer)
        {
            GameObject go = Managers.Resource.Instantiate("MyPlayer");
            go.name = info.Name;
            Objects.Add(info.ObjectId, go);

            MyPlayer = go.GetComponent<MyPlayerController>();

            MyPlayer.Id = info.ObjectId;
            MyPlayer.Name = info.Name;
            MyPlayer.PosInfo = info.PosInfo;

            UnityEngine.Object.DontDestroyOnLoad(go);
        }
        else
        {
            GameObject go = Managers.Resource.Instantiate("Player");
            go.name = info.Name;
            Objects.Add(info.ObjectId, go);

            PlayerController pc = go.GetComponent<PlayerController>();
            pc.Id = info.ObjectId;
            pc.Name = info.Name;
            pc.PosInfo = info.PosInfo;
            pc.SyncPos(new Vector2(info.PosInfo.PosX, info.PosInfo.PosY));

            UnityEngine.Object.DontDestroyOnLoad(go);
        }
    }

    public void Remove(int id)
    {
        GameObject go = FindById(id);
        if (go == null)
            return;

        Objects.Remove(id);
        Managers.Resource.Destroy(go);
    }

    public GameObject FindById(int id)
    {
        GameObject go = null;
        Objects.TryGetValue(id, out go);
        return go;
    }

    public GameObject Find(Func<GameObject, bool> condition)
    {
        foreach (GameObject obj in Objects.Values)
        {
            if (condition.Invoke(obj))
                return obj;
        }

        return null;
    }

    public void Clear()
    {
        foreach (GameObject obj in Objects.Values)
            Managers.Resource.Destroy(obj);
        Objects.Clear();
        MyPlayer = null;
    }
}

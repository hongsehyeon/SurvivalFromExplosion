using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    public MyPlayerController MyPlayer { get; set; }
    public Exploder Exploder { get; set; }
    Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();

    public GameObject Add(ObjectInfo info, bool myPlayer = false)
    {
        if (myPlayer)
        {
            GameObject go = Managers.Resource.Instantiate("MyPlayer");
            go.name = info.Name;
            _objects.Add(info.ObjectId, go);

            MyPlayer = go.GetComponent<MyPlayerController>();
            MyPlayer.Id = info.ObjectId;
            MyPlayer.PosInfo = info.PosInfo;

            UnityEngine.Object.DontDestroyOnLoad(go);

            return go;
        }
        else
        {
            GameObject go = Managers.Resource.Instantiate("Player");
            go.name = info.Name;
            _objects.Add(info.ObjectId, go);

            PlayerController pc = go.GetComponent<PlayerController>();
            pc.Id = info.ObjectId;
            pc.PosInfo = info.PosInfo;
            pc.SyncPos(new Vector2(info.PosInfo.PosX, info.PosInfo.PosY));

            UnityEngine.Object.DontDestroyOnLoad(go);

            return go;
        }
    }

    public void Remove(int id)
    {
        GameObject go = FindById(id);
        if (go == null)
            return;

        _objects.Remove(id);
        Managers.Resource.Destroy(go);
    }

    public GameObject FindById(int id)
    {
        GameObject go = null;
        _objects.TryGetValue(id, out go);
        return go;
    }

    public GameObject Find(Func<GameObject, bool> condition)
    {
        foreach (GameObject obj in _objects.Values)
        {
            if (condition.Invoke(obj))
                return obj;
        }

        return null;
    }

    public void Clear()
    {
        foreach (GameObject obj in _objects.Values)
            Managers.Resource.Destroy(obj);
        _objects.Clear();
        MyPlayer = null;
    }
}

using Google.Protobuf.Protocol;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int Id;
    public string Name;

    PositionInfo _posInfo;
    public PositionInfo PosInfo
    {
        get { return _posInfo; }
        set
        {
            MovePos = new Vector2(value.PosX, value.PosY);
            _posInfo = value;
        }
    }

    public float speed = 10f;
    public Vector2 MovePos = Vector2.zero;

    protected virtual void Start()
    {
        // Temp
        SyncPos(new Vector2(3.8f, 0));
    }

    protected virtual void Update()
    {
        MoveToMousePos();
    }

    public void SyncPos(Vector2 pos)
    {
        transform.position = pos;
        PosInfo.PosX = pos.x;
        PosInfo.PosY = pos.y;
        MovePos = pos;
    }

    public virtual void MoveToMousePos()
    {
        transform.position = Vector2.MoveTowards(transform.position, MovePos, speed * Time.deltaTime);
    }

    public virtual void OnDead()
    {
        // TODO : Æø¹ß ÀÌÆåÆ® »ý¼º
        gameObject.SetActive(false);
    }
}

using Google.Protobuf.Protocol;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int Id;

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

    protected virtual void Update()
    {
        MoveToMousePos();
    }

    public virtual void MoveToMousePos()
    {
        transform.position = Vector2.MoveTowards(transform.position, MovePos, speed * Time.deltaTime);
    }

    public virtual void OnDead()
    {

    }
}

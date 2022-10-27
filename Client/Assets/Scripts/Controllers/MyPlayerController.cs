using Google.Protobuf.Protocol;
using UnityEngine;

public class MyPlayerController : PlayerController
{
    Vector2 MousePos;

    void LateUpdate()
    {
        //Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    protected override void Update()
    {
        if (Input.GetMouseButtonDown(1))
            Move();

        base.Update();
    }

    private void Move()
    {
        MousePos = Input.mousePosition;
        Vector2 movePos = Camera.main.ScreenToWorldPoint(MousePos);
        MovePos = movePos;

        PosInfo.PosX = movePos.x;
        PosInfo.PosY = movePos.y;

        C_Move movePacket = new C_Move();
        movePacket.PosInfo = PosInfo;
        Managers.Network.Send(movePacket);
    }
}

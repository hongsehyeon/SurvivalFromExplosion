using Google.Protobuf.Protocol;
using UnityEngine;

public class MyPlayerController : PlayerController
{
    Vector2 MousePos;

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
        movePos = new Vector2(Mathf.Clamp(movePos.x, -0.27f, 7.87f), Mathf.Clamp(movePos.y, -4.07f, 4.07f));
        MovePos = movePos;

        PosInfo.PosX = movePos.x;
        PosInfo.PosY = movePos.y;

        C_Move movePacket = new C_Move();
        movePacket.PosInfo = PosInfo;
        Managers.Network.Send(movePacket);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion"))
        {
            C_Die diePacket = new C_Die();
            Managers.Network.Send(diePacket);
        }
    }
}

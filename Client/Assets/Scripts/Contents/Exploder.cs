using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    private void Start()
    {
        Managers.Object.Exploder = this;
    }

    public void Explode(int patternId)
    {
        Debug.Log("펑퍼퍼퍼ㅓ퍼퍼퍼퍼퍼퍼펑");

        // TODO : 패턴에 따른 폭발 구현
    }
}

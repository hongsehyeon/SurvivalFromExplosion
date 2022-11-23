using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    public float NextExplosionTime = 1f;
    public List<Explosion> Explosions = new List<Explosion>();

    private byte[,] explosionPattern =
    {
        {
            0, 0, 1, 0, 0,
            0, 0, 1, 0, 0,
            1, 1, 1, 1, 1,
            0, 0, 1, 0, 0,
            0, 0, 1, 0, 0
        },
        {
            0, 1, 0, 1, 0,
            0, 1, 0, 1, 0,
            0, 1, 0, 1, 0,
            0, 1, 0, 1, 0,
            0, 1, 0, 1, 0
        },
        {
            1, 0, 1, 0, 1,
            1, 0, 1, 0, 1,
            1, 0, 1, 0, 1,
            1, 0, 1, 0, 1,
            1, 0, 1, 0, 1
        },
        {
            1, 1, 1, 1, 1,
            0, 0, 0, 0, 0,
            1, 1, 1, 1, 1,
            0, 0, 0, 0, 0,
            1, 1, 1, 1, 1
        },
        {
            0, 0, 0, 0, 0,
            1, 1, 1, 1, 1,
            0, 0, 0, 0, 0,
            1, 1, 1, 1, 1,
            0, 0, 0, 0, 0
        },
        {
            1, 1, 1, 1, 1,
            1, 0, 0, 0, 1,
            1, 0, 1, 0, 1,
            1, 0, 0, 0, 1,
            1, 1, 1, 1, 1
        },
        {
            0, 1, 0, 1, 0,
            1, 0, 1, 0, 1,
            0, 1, 0, 1, 0,
            1, 0, 1, 0, 1,
            0, 1, 0, 1, 0
        },
    };
    private WaitForSeconds nextExplosionTime;

    private void Start()
    {
        Managers.Object.Exploder = this;
        nextExplosionTime = new WaitForSeconds(NextExplosionTime);
    }

    public void Explode(RepeatedField<int> patternIds)
    {
        StartCoroutine(Exploding(patternIds));
    }

    private IEnumerator Exploding(RepeatedField<int> patternIds)
    {
        for (int i = 0; i < patternIds.Count; i++)
        {
            for (int j = 0; j < Explosions.Count; j++)
            {
                if (explosionPattern[i, j] == 1)
                {
                    Explosions[j].gameObject.SetActive(true);
                }
            }

            yield return nextExplosionTime;
        }
    }
}

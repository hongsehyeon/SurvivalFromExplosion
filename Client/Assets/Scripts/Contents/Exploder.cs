using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    public GameObject PreviewCountDownUI;
    public GameObject ExplosionCountDownUI;

    public float NextExplosionTime = 1f;
    public List<GameObject> Previews = new List<GameObject>();
    public List<GameObject> Explosions = new List<GameObject>();

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
        StartCoroutine(Exploding(patternIds, true));
    }

    private IEnumerator Exploding(RepeatedField<int> patternIds, bool isPreview)
    {
        if (isPreview)
        {
            PreviewCountDownUI.SetActive(true);
            yield return new WaitForSeconds(1);

            for (int i = 0; i < patternIds.Count; i++)
            {
                for (int j = 0; j < Previews.Count; j++)
                    if (explosionPattern[patternIds[i], j] == 1)
                        Previews[j].SetActive(true);

                yield return nextExplosionTime;
            }

            ExplosionCountDownUI.SetActive(true);
            yield return new WaitForSeconds(3);
            StartCoroutine(Exploding(patternIds, false));
        }
        else
        {
            for (int i = 0; i < patternIds.Count; i++)
            {
                for (int j = 0; j < Explosions.Count; j++)
                    if (explosionPattern[patternIds[i], j] == 1)
                        Explosions[j].SetActive(true);

                yield return nextExplosionTime;
            }
        }
    }
}

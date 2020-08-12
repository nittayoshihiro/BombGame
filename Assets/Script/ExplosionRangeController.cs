using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionRangeController : MonoBehaviour
{
    //爆発範囲が消滅するまでの時間
    [SerializeField] float finishExplosionTime = 1;
    //時間
    float m_Time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_Time += Time.deltaTime;
        if (m_Time > finishExplosionTime)
        {
            Destroy(this.gameObject);
        }
    }
}

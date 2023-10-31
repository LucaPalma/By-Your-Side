using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityFader : MonoBehaviour
{
    private Transform player;
    public float distToP;
    public float currentAlpha;
    public Renderer mr;

    private void Awake()
	{
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}

    private void Start()
    {
        mr = GetComponent<Renderer>();
    }

    void Update()
    {
        distToP = Vector3.Distance(this.transform.position, player.position);

        var col = mr.material.color;
        col.a = 6.8f - distToP;
        if (col.a < 0) col.a = 0;
        mr.material.color = col;

        if (distToP > 35) Destroy(this.gameObject);

    }
}

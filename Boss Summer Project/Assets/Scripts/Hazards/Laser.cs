using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class Laser : MonoBehaviour
{
    private EdgeCollider2D edgeCollider;
    private LineRenderer laser;
    private bool isBlockedByShield;
    public bool IsBlockedByShield {
        get { return isBlockedByShield; }
        set { isBlockedByShield = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        laser = GetComponent<LineRenderer>();
        gameObject.SetActive(false);
    }

    void OnEnable() {
        isBlockedByShield = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Shield") {
            isBlockedByShield = true;
        }
    }

    public void SetEdgeCollider() {
        Vector2 start = laser.GetPosition(0);
        Vector2 end = laser.GetPosition(1);

        List<Vector2> edges = new List<Vector2>();
        edges.Add(start);
        edges.Add(end);

        edgeCollider.SetPoints(edges);
    }
}

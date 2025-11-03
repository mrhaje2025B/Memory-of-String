using UnityEngine;

public class Camera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform player;
    private Transform trans;
    void Awake()
    {
        trans = transform;
    }

    // Update is called once per frame
    void Update()
    {
        trans.position = new Vector3(player.position.x, player.position.y, trans.position.z);
    }
}

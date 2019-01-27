using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillzoneController : MonoBehaviour
{
    public float moveSpeed = 0.35f;

    public int killColumnStart = -1;

    public int killColumn = -1;

    private float killPosition = -1f;

    private void Start()
    {
        killPosition = killColumnStart;
        killColumn = killColumnStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.x < 220 && !GameController.playerDied) {
            this.transform.position = new Vector3(this.transform.position.x + (moveSpeed*Time.deltaTime), this.transform.position.y, this.transform.position.z);
            killPosition += moveSpeed * Time.deltaTime;
            killColumn = Mathf.FloorToInt(killPosition);
            //Debug.Log(killColumn);
        }
    }
}

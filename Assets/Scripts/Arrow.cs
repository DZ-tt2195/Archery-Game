using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    bool moving = true;
    [SerializeField] float bulletSpeed;

    private void Update()
    {
        if (moving)
        {
            this.transform.Translate(bulletSpeed * Time.deltaTime * Vector3.up);
        }
    }

    int CalculateScore()
    {
        Vector3 currentPosition = transform.localPosition;
        currentPosition.z = 0;
        return 8-(int)(Vector3.Distance(currentPosition, Vector3.zero)/0.65f);
    }

    private void OnTriggerEnter(Collider other)
    {
        moving = false;
        this.transform.SetParent(other.gameObject.transform);
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, -0.65f);
        Debug.Log(CalculateScore());
    }
}

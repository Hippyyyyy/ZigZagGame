using SCN.BinaryData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody rb;
    bool movingLeft = true;
    bool firstInput = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (GameManager.Ins.GameStarted)
        {
            MoveCar();
            CheckInput();
        }
        if (transform.position.y <= -2)
        {
            GameManager.Ins.GameOver();
        }
    }

    void MoveCar()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    void CheckInput()
    {
        if (firstInput)
        {
            firstInput = false;
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        if (movingLeft)
        {
            movingLeft = false;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            movingLeft = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Diamond")
        {
            GameManager.Ins.IncrementScore();
            DataManagerSample.Instance.LocalData.AddPointDiamond();
            other.gameObject.SetActive(false);
            AudioManager.Ins.Play_Collect_Diamond();
            GameManager.Ins.Parrticle(other.transform.parent);
        }
    }
}

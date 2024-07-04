using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] List<Color> listColor;
    [SerializeField] Diamond diamondPrefab;
    BoxCollider boxCol;
    ObjectPool platformPool;
    bool isFall;
    public BoxCollider BoxCol { get => boxCol; set => boxCol = value; }

    private void Awake()
    {
        BoxCol = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        /*mat = GetComponent<Material>();
        ChangeColor();*/
    }

    private void OnEnable()
    {
        isFall = false;
        SpawnDiamond();
    }

    public void SetUp()
    {
        isFall = false;
        BoxCol.enabled = true;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    void ChangeColor()
    {
        var colorRandom = Random.Range(0, listColor.Count);
        mat.color = listColor[colorRandom];
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isFall = true;
            Fall();
            Invoke("Fall", 0.2f);
        }
    }

    void Fall()
    {
        if (!isFall) return;

        GetComponent<Rigidbody>().isKinematic = false;
        Invoke("RemoveObj", 1f);
    }

    void SpawnDiamond()
    {
        int randDiamond = Random.Range(0, 5);
        Vector3 diamondPos = transform.position;
        diamondPos.y += 1f;
        if (randDiamond < 1)
        {
            var diamond = Instantiate(diamondPrefab, diamondPos, diamondPrefab.transform.rotation);
            diamond.transform.SetParent(transform);
            diamond.transform.rotation = Quaternion.Euler(-90,0,0);
        }
    }
    public void SetPool(ObjectPool pool)
    {
        this.platformPool = pool;
    }
    public void RemoveObj()
    {
        if (!isFall) return;

        BoxCol.enabled = false;
        gameObject.SetActive(false);
        if (platformPool != null)
        {
            platformPool.RemoveObj(gameObject);
        }
       
    }
}

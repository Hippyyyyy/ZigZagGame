using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] List<Color> listColor;
    [SerializeField] Diamond diamondPrefab;
    private void Start()
    {
        SpawnDiamond();
        /*mat = GetComponent<Material>();
        ChangeColor();*/
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
            Fall();
            Invoke("Fall", 0.2f);
        }
    }

    void Fall()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        Destroy(gameObject, 1f);
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
}

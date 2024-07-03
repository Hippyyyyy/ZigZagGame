using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangerBackgroundColor : MonoBehaviour
{
    [SerializeField] List<Color> listColor;

    private void Start()
    {
        Camera.main.backgroundColor = listColor[0];
    }

    public void StartChange()
    {
        StartCoroutine(ChangeColor());
    }
    public void StopChange()
    {
        StopCoroutine(ChangeColor());
        Camera.main.backgroundColor = listColor[0];
    }

    IEnumerator ChangeColor()
    {
        while (true)
        {
            var randomColor = Random.Range(0, listColor.Count);
            Camera.main.backgroundColor = listColor[randomColor];
            yield return new WaitForSeconds(10f);
        }
    }
}

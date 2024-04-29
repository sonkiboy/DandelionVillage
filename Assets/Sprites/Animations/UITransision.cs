using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UITransision : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;

    float time = 1;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }



    public IEnumerator FadeIn(float time)
    {


        float rate = time/sprites.Length;

        yield return new WaitForSeconds(rate);


        for (int i = 0; i < sprites.Length; i++)
        {
            image.sprite = sprites[i];

            yield return new WaitForSeconds(rate);
        }
    }

    public IEnumerator FadeOut(float time)
    {
        //Debug.Log("Fade out hit");

        float rate = time / sprites.Length;

        for (int i = sprites.Length - 1; i > 0; i--)
        {
            //Debug.Log($"setp {i}: setting image to {sprites[i]}");

            image.sprite = sprites[i];

            yield return new WaitForSeconds(rate);
        }
    }
}

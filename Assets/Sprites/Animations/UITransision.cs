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

    public bool isFading = false;

    private void Awake()
    {
        image = GetComponent<Image>();
    }



    public IEnumerator FadeIn(float time)
    {
        StopCoroutine(FadeOut(1));

        isFading = true;


        PlayerController player = GameObject.FindObjectOfType<PlayerController>();

        player.CurrentMoveRestrict = PlayerController.MovementRestrictions.NoMovement;

        float rate = time/sprites.Length;

        yield return new WaitForSeconds(rate);


        for (int i = 0; i < sprites.Length; i++)
        {
            if (!image.IsDestroyed())
            {
                image.sprite = sprites[i];
            }
            else
            {
                break;

            }


            yield return new WaitForSeconds(rate);
        }

        player.CurrentMoveRestrict = PlayerController.MovementRestrictions.NoRestriction;

        isFading = false;

    }

    public IEnumerator FadeOut(float time)
    {
        StopCoroutine(FadeIn(1));


        isFading = true;

        PlayerController player = GameObject.FindObjectOfType<PlayerController>();

        player.CurrentMoveRestrict = PlayerController.MovementRestrictions.NoMovement;


        //Debug.Log("Fade out hit");

        float rate = time / sprites.Length;

        for (int i = sprites.Length - 1; i > 0; i--)
        {
            //Debug.Log($"setp {i}: setting image to {sprites[i]}");

            if (!image.IsDestroyed())
            {
                image.sprite = sprites[i];
            }
            else
            {
                image = GetComponent<Image>();
            }



            yield return new WaitForSeconds(rate);
        }


        isFading = false;

    }
}

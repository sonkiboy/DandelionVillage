using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject Title;
    public GameObject StartButton;
    public GameObject QuitButton;
    public GameObject Instructions;

    public float InitialDelay;
    public float ScrollSpeed;
    public int ScrollDistance;
    public float TitleDelay;
    public float TitleSpeed;
    public float ButtonDelay;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TitleStart());
    }

    // Update is called once per frame
    void Update()
    {
        


    }

    private IEnumerator TitleStart()
    {
        yield return new WaitForSeconds(InitialDelay/2);

        Instructions.SetActive(false);

        yield return new WaitForSeconds(InitialDelay / 2);


        while (this.transform.position.y < ScrollDistance)
        {
            this.transform.position += Vector3.up * ScrollSpeed;

            yield return new WaitForSeconds(ScrollSpeed);
        }

        yield return new WaitForSeconds(TitleDelay);

        Title.SetActive(true);

        yield return new WaitForSeconds(ButtonDelay);

        StartButton.SetActive(true);
        QuitButton.SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("VillageAndForest");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Transform[] m_TitleLetters;

    public Button m_PlayButton;
    public Button m_ExitButton;

    private AudioSource m_StartGameSound;

    public bool m_IsOnMainMenu = true;

    void Start()
    {
        Cursor.visible = false;
        m_StartGameSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (m_IsOnMainMenu)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_StartGameSound.Play();
                StartCoroutine(DropTitleLetters());
                StartCoroutine(LoadAfterAudio(1));
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    IEnumerator DropTitleLetters()
    {
        List<int> lettersUsed = new List<int>();
        while (lettersUsed.Count < m_TitleLetters.Length)
        {
            int randomNumber = Random.Range(0, m_TitleLetters.Length);
            if (lettersUsed.Contains(randomNumber)) continue;
            lettersUsed.Add(randomNumber);
            DropTitleLetter(randomNumber);
            yield return new WaitForSeconds(0.5f);
        }

        m_PlayButton.GetComponent<Animator>().SetTrigger("SpacePressed");
        m_ExitButton.GetComponent<Animator>().SetTrigger("SpacePressed");
    }

    void DropTitleLetter(int _iD)
    {
        Rigidbody2D rb = m_TitleLetters[_iD].GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

	public void LoadScene(int _iID)
    {
       m_StartGameSound.Play();
       StartCoroutine(LoadAfterAudio(_iID));
    }

    IEnumerator LoadAfterAudio(int _iID)
    {
        yield return new WaitForSeconds(0.5f * m_TitleLetters.Length + 1.3f);
        SceneManager.LoadScene(_iID);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

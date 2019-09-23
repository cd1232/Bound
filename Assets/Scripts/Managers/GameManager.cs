using DG.Tweening;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Level[] m_Levels;
    private Transform[] m_Obstacles;

    public GameObject m_SquarePrefab;
    public GameObject m_LShapePrefab;
    public GameObject m_LilEdgePrefab;
    public GameObject m_BigEdgePrefab;
    public GameObject m_SquareBoxPrefab;
    public GameObject m_PortalPrefab;
    public GameObject m_GameOver;
    public GameObject m_PauseMenu;

    public EndGoalController m_EndGoalController;
    public TextMesh m_ScoreText;

    public Transform m_Ball;
    public Transform m_EndGoal;
    public Transform m_Gradient;
    public Transform m_Gradient2;

    private AudioSource m_CorrectSound;
    private AudioSource m_ErrorSound;

    private GameObject m_PrefabToInstantiate;

    public float m_GradientSpeed;
    public int m_StartLevel;
    private int m_Score = 0;
    private int m_TotalScore = 0;
    private int m_CurrentLevel = 1;

    private bool m_bIsPaused = false;
    private bool m_bIsComplete = false;

    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        m_CorrectSound = sources[0];
        m_ErrorSound = sources[1];
        Cursor.visible = false;
        LoadLevel(m_StartLevel);
    }

    public void LoadLevel(int _iLevel)
    {
        m_TotalScore += m_Score;
        ResetScore();

        if (_iLevel == 6)
        {
            // game completed
            m_bIsComplete = true;
            m_GameOver.SetActive(true);
            Save();
            CanvasGroup GameOver = m_GameOver.GetComponentInChildren<CanvasGroup>();

            GameOver.DOFade(1.0f, 0.5f);
        }

        if (_iLevel - 1 < m_Levels.Length)
        {            
            m_CurrentLevel = _iLevel;
            m_EndGoalController.m_ObjectsToActivate.Clear();
            m_Ball.transform.position = m_Levels[_iLevel - 1].ballStartPosition;
            m_EndGoal.transform.position = m_Levels[_iLevel - 1].goalPosition;

            for (int i = 0; i < m_Levels[_iLevel - 1].obstacles.Length; ++i)
            {
                Level.Obstacle currentObstacle = m_Levels[_iLevel - 1].obstacles[i];
                if (currentObstacle.type == Level.ObstacleType.Square)
                {
                    m_PrefabToInstantiate = m_SquarePrefab;
                }
                else if (currentObstacle.type == Level.ObstacleType.LShape)
                {
                    m_PrefabToInstantiate = m_LShapePrefab;             
                }
                else if (currentObstacle.type == Level.ObstacleType.LilEdge)
                {
                    m_PrefabToInstantiate = m_LilEdgePrefab;
                }
                else if (currentObstacle.type == Level.ObstacleType.BigEdge)
                {
                    m_PrefabToInstantiate = m_BigEdgePrefab;
                }
                else if (currentObstacle.type == Level.ObstacleType.SquareBox)
                {
                    m_PrefabToInstantiate = m_SquareBoxPrefab;
                }

                GameObject temp = Instantiate(m_PrefabToInstantiate, currentObstacle.pos, 
                    Quaternion.Euler(currentObstacle.rot.x, currentObstacle.rot.y, currentObstacle.rot.z), transform.parent);

                temp.transform.localScale = new Vector3(currentObstacle.scale.x, currentObstacle.scale.y, 1.0f);
                m_EndGoalController.m_ObjectsToActivate.Add(temp.transform);
            }

            List<GameObject> portals = new List<GameObject>();

            for (int i = 0; i < m_Levels[_iLevel - 1].portals.Length; ++i)
            {
                Level.Portal currentPortal = m_Levels[_iLevel - 1].portals[i];
                GameObject temp = Instantiate(m_PortalPrefab, currentPortal.pos, Quaternion.Euler(currentPortal.rot.x, 
                    currentPortal.rot.y, currentPortal.rot.z), transform.parent);

                temp.transform.localScale = new Vector3(currentPortal.scale.x, currentPortal.scale.y, 1.0f);
                m_EndGoalController.m_ObjectsToActivate.Add(temp.transform);
                portals.Add(temp);
            }

            for (int i = 0; i < portals.Count; ++i)
            {
                if (i % 2 == 0 || i == 0)
                {
                    portals[i].GetComponent<PortalController>().m_ConnectedPortal = portals[i + 1].transform;
                }
                else
                {
                    portals[i].GetComponent<PortalController>().m_ConnectedPortal = portals[i - 1].transform;
                }
            }

            m_Gradient.GetComponent<SpriteRenderer>().sprite = m_Levels[_iLevel - 1].backgroundGradient;
            m_Gradient2.GetComponent<SpriteRenderer>().sprite = m_Levels[_iLevel - 1].backgroundGradient;
        }
    }

    void Update()
    {
        ScrollGradient();

        // check for esc
        if (Input.GetKeyUp(KeyCode.Escape) && !m_bIsComplete)
        {
            m_bIsPaused = !m_bIsPaused;

            if (!m_bIsPaused)
            {
                m_PauseMenu.SetActive(false);
                Time.timeScale = 1.0f;
            }
            else
            {
                m_PauseMenu.SetActive(true);
                Time.timeScale = 0.0f;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (m_bIsPaused)
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(0);
            }

            if (m_bIsComplete)
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(0);
            }
        }
     }
    
    void ScrollGradient()
    {
        m_Gradient.transform.position = 
            new Vector3(m_Gradient.transform.position.x, m_Gradient.transform.position.y + (m_GradientSpeed * Time.smoothDeltaTime), 
            m_Gradient.transform.position.z);
        m_Gradient2.transform.position =
            new Vector3(m_Gradient2.transform.position.x, m_Gradient2.transform.position.y + (m_GradientSpeed * Time.smoothDeltaTime), 
            m_Gradient2.transform.position.z);

        if (m_Gradient.transform.position.y >= 340.0f)
        {
            m_Gradient.transform.position = new Vector3(m_Gradient.transform.position.x, m_Gradient2.transform.position.y - 324.0f, 
                m_Gradient.transform.position.z);
        }

        if (m_Gradient2.transform.position.y >= 340.0f)
        {
            m_Gradient2.transform.position = new Vector3(m_Gradient2.transform.position.x, m_Gradient.transform.position.y - 324.0f, 
                m_Gradient2.transform.position.z);
        }
    }

    public void Save()
    {
        Text[] textComponents = m_GameOver.GetComponentsInChildren<Text>();
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            int iOldScore = (int)bf.Deserialize(file);
            file.Close();

            if (m_TotalScore < iOldScore)
            {           
                textComponents[1].text = "New Best Score: " + m_TotalScore;

                FileStream file2 = File.Create(Application.persistentDataPath + "/savedGames.gd");
                bf.Serialize(file2, m_TotalScore);
                file2.Close();
            }
            else
            {
                textComponents[1].text = "Best Score: " + iOldScore;
                textComponents[2].text = "Your Score: " + m_TotalScore;
            }
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();

            textComponents[1].text = "New Best Score: " + m_TotalScore;
            FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
            bf.Serialize(file, m_TotalScore);
            file.Close();
        }
    }

    public void AddToScore()
    {
        m_Score++;
        m_ScoreText.text = m_Score.ToString();

        Animator scoreAnim = m_ScoreText.GetComponent<Animator>();
        scoreAnim.SetTrigger("ScoreIncreased");
    }

    void ResetScore()
    {
        m_Score = 0;
        m_ScoreText.text = m_Score.ToString();
    }

    public int GetCurrentLevel()
    {
        return m_CurrentLevel;
    }

    public bool GetIsPaused()
    {
        return m_bIsPaused;
    }

    Vector3 SetPositionPointOnCircle(Vector3 center, float radius, int _iObjectNum)
    {
        float ang = 30 * _iObjectNum;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }
}

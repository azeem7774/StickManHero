using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public enum GameState
{
    START,INPUT,GROWING,NONE
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private int m_lives;
    [SerializeField]
    private Vector3 startPos;

    [SerializeField]
    private Vector2 minMaxRange, spawnRange;

    [SerializeField]
    private GameObject pillarPrefab, centerPointPrefab, stickPrefab, orangePrefab, applePrefab , currentCamera;
    [SerializeField] private GameObject[] playerPrefab;

    [SerializeField]
    private Transform rotateTransform, endRotateTransform;

    [SerializeField]
    private GameObject scorePanel, startPanel, endPanel, m_GamePlayScreen, m_StickTip;

    [SerializeField]
    private TMP_Text scoreText, scoreEndText, orangeText, appleText, highScoreText;

    private GameObject currentPillar, nextPillar, currentStick, player;
    public GameObject Player => player;

    private int score, oranges, apples, highScore;

    private float cameraOffsetX;

    private GameState currentState;

    [SerializeField]
    private float stickIncreaseSpeed, maxStickSize;

    public static GameManager instance;

    private int m_PlayerIndex;
    private Vector3 m_CurrentPlatform;
    [SerializeField] private float yOffset;
    [SerializeField] private Canvas m_GameplayCanvas;
    private Rigidbody2D m_PlayerRb;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        RandomBackground();
        m_PlayerIndex = PrefManager.PlayerIndex;
        
        currentState = GameState.START;

        endPanel.SetActive(false);
        scorePanel.SetActive(false);
        startPanel.SetActive(true);
        score = 0;
        oranges = PlayerPrefs.HasKey("Diamonds") ? PlayerPrefs.GetInt("Diamonds") : 0;
        highScore = PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetInt("HighScore") : 0;
        apples = m_lives;

        scoreText.text = score.ToString();
        orangeText.text = oranges.ToString();
        appleText.text = apples.ToString();
        highScoreText.text = highScore.ToString();

        CreateStartObjects();
        cameraOffsetX = currentCamera.transform.position.x - player.transform.position.x;

        GameStart();
        if(StateManager.instance.hasSceneStarted)
        {
        }
    }

    private void Update()
    {
        //if (!IsTouchOnUI())
        {
            if (currentState == GameState.INPUT)
            {
                if (Input.GetMouseButton(0))
                {
                    currentState = GameState.GROWING;
                    ScaleStick();
                }
            }

            if (currentState == GameState.GROWING)
            {
                if (Input.GetMouseButton(0))
                {
                    ScaleStick();
                }
                else
                {
                    StartCoroutine(FallStick());
                }
            }
        }

    }

    void ScaleStick()
    {
        Vector3 tempScale = currentStick.transform.localScale;
        tempScale.y += Time.deltaTime * stickIncreaseSpeed;
        if (tempScale.y > maxStickSize)
            tempScale.y = maxStickSize;
        currentStick.transform.localScale = tempScale;
    }

    IEnumerator FallStick()
    {
        currentState = GameState.NONE;
        var x = Rotate(currentStick.transform, rotateTransform, 0.4f); 
        yield return x;
        m_StickTip = currentStick.transform.GetChild(0).gameObject;
        Vector2 pillarCenter = nextPillar.GetComponent<Collider2D>().bounds.center;
        float distanceToCeneter = Mathf.Abs(pillarCenter.x);
        if (m_StickTip.transform.position.x-0.2f <= distanceToCeneter && m_StickTip.transform.position.x + 0.2f >= distanceToCeneter)
        {
            if (true)
            {
                print("Perfect");
            }
        }
        Vector3 movePosition = currentStick.transform.position + new Vector3(currentStick.transform.localScale.y,0,0);
        movePosition.y = player.transform.position.y;
        x = Move(player.transform,movePosition,0.5f);
        yield return x;

        var results = Physics2D.RaycastAll(player.transform.position,Vector2.down);
        var result = Physics2D.Raycast(player.transform.position, Vector2.down);
        foreach (var temp in results)
        {
            if(temp.collider.CompareTag("Platform"))
            {
                result = temp;
            }
        }

        if(!result || !result.collider.CompareTag("Platform"))
        {
            player.GetComponent<Rigidbody2D>().gravityScale = 1f;
            x = Rotate(currentStick.transform, endRotateTransform, 0.5f);
            yield return x;
            if (m_lives > 1)
            {
                m_lives--;
                appleText.text = m_lives.ToString();
                player.GetComponent<Rigidbody2D>().gravityScale = 0;
                player.transform.position = m_CurrentPlatform;
                m_PlayerRb.constraints = RigidbodyConstraints2D.FreezePositionY;
                m_PlayerRb.constraints = RigidbodyConstraints2D.None;
                
                GameStart();
                movePosition = currentCamera.transform.position;
                movePosition.x = player.transform.position.x + cameraOffsetX;
                x = Move(currentCamera.transform, movePosition, 0.5f);
                yield return x;
                Vector3 stickPosition = currentPillar.transform.position;
                stickPosition.x += currentPillar.transform.localScale.x * 0.5f - 0.05f;
                stickPosition.y = currentStick.transform.position.y;
                stickPosition.z = currentStick.transform.position.z;
                currentStick = Instantiate(stickPrefab, stickPosition, Quaternion.identity);
            }
            else
            {
                GameOver();
            }
            
        }
        else
        {
            UpdateScore();

            movePosition = player.transform.position;
            movePosition.x = nextPillar.transform.position.x + nextPillar.transform.localScale.x * 0.5f - 0.35f;
            x = Move(player.transform, movePosition, 0.2f);
            yield return x;

            movePosition = currentCamera.transform.position;
            movePosition.x = player.transform.position.x + cameraOffsetX;
            x = Move(currentCamera.transform, movePosition, 0.5f);
            Vector2 BGPos =  m_Forground.rectTransform.position;
            Move(m_Forground.transform, BGPos, 0.5f);
            yield return x;

            CreatePlatform();
            SetRandomSize(nextPillar);
            currentState = GameState.INPUT;
            Vector3 stickPosition = currentPillar.transform.position;
            stickPosition.x += currentPillar.transform.localScale.x * 0.5f - 0.05f;
            stickPosition.y = currentStick.transform.position.y;
            stickPosition.z = currentStick.transform.position.z;
            currentStick = Instantiate(stickPrefab, stickPosition, Quaternion.identity);
        }
    }


    void CreateStartObjects()
    {
        CreatePlatform();

        Vector3 playerPos = playerPrefab[m_PlayerIndex].transform.position;
        playerPos.x += (currentPillar.transform.localScale.x * 0.5f - 0.35f);
        player = Instantiate(playerPrefab[m_PlayerIndex],playerPos,Quaternion.identity);
        player.name = "Player";
        m_PlayerRb = player.GetComponent<Rigidbody2D>();

        Vector3 stickPos = stickPrefab.transform.position;
        stickPos.x += (currentPillar.transform.localScale.x*0.5f - 0.05f);
        currentStick = Instantiate(stickPrefab, stickPos, Quaternion.identity);
    }

    void CreatePlatform()
    {
        GameObject currentPlatform = Instantiate(pillarPrefab);
        centerPointPrefab = currentPlatform.transform.GetChild(0).gameObject;// get red sprite as a perfect point
        currentPillar = nextPillar == null ? currentPlatform : nextPillar;
        nextPillar = currentPlatform;
        currentPlatform.transform.position = pillarPrefab.transform.position + startPos;
        Vector3 tempDistance = new Vector3(UnityEngine.Random.Range(spawnRange.x,spawnRange.y) + currentPillar.transform.localScale.x*0.5f,0,0);
        startPos += tempDistance;
        m_CurrentPlatform = currentPlatform.transform.position+ new Vector3(transform.position.x, transform.position.y+ yOffset, 0);
        if(UnityEngine.Random.Range(0,1) == 0)
        {
            int randomPrefabe = UnityEngine.Random.Range(0, 2);
            GameObject pickable;
            if (randomPrefabe == 0)
                pickable = Instantiate(applePrefab);
            
            else
                pickable = Instantiate(orangePrefab);
            Vector3 tempPos = currentPlatform.transform.position;
            tempPos.y = orangePrefab.transform.position.y;
            pickable.transform.position = tempPos;
        }
        m_Platforms.Add(currentPlatform);
    }

    void SetRandomSize(GameObject pillar)
    {
        var newScale = pillar.transform.localScale;
        var allowedScale = nextPillar.transform.position.x - currentPillar.transform.position.x
            - currentPillar.transform.localScale.x * 0.5f - 0.4f;
        newScale.x = Mathf.Max(minMaxRange.x, UnityEngine.Random.Range(minMaxRange.x,Mathf.Min(allowedScale,minMaxRange.y)));
        pillar.transform.localScale = newScale;
    }

    void UpdateScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    void GameOver()
    {
        foreach (var item in m_Platforms)
            item.SetActive(false);

        
        m_GameplayCanvas.sortingOrder = 10;
        endPanel.SetActive(true);
        m_GamePlayScreen.SetActive(false);
        scorePanel.SetActive(false);

        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        scoreEndText.text = score.ToString();
        highScoreText.text = highScore.ToString();
    }

    public void UpdateOranges()
    {
        oranges++;
        PlayerPrefs.SetInt("Diamonds", oranges);
        orangeText.text = oranges.ToString();
    }
    public void UpdateApple()
    {
        m_lives++;
        appleText.text = m_lives.ToString();
    }
    public void GameStart()
    {
        m_GameplayCanvas.sortingOrder = 0;
        startPanel.SetActive(false);
        scorePanel.SetActive(true);

        CreatePlatform();
        SetRandomSize(nextPillar);
        currentState = GameState.INPUT;
        
    }

    public void GameRestart()
    {
        StateManager.instance.hasSceneStarted = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void SceneRestart()
    {
        StateManager.instance.hasSceneStarted = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    //Helper Functions
    IEnumerator Move(Transform currentTransform,Vector3 target,float time)
    {
        var passed = 0f;
        var init = currentTransform.transform.position;
        while(passed < time)
        {
            passed += Time.deltaTime;
            var normalized = passed / time;
            var current = Vector3.Lerp(init, target, normalized);
            currentTransform.position = current;
            yield return null;
        }
    }

    IEnumerator Rotate(Transform currentTransform, Transform target, float time)
    {
        var passed = 0f;
        var init = currentTransform.transform.rotation;
        while (passed < time)
        {
            passed += Time.deltaTime;
            var normalized = passed / time;
            var current = Quaternion.Slerp(init, target.rotation, normalized);
            currentTransform.rotation = current;
            yield return null;
        }
    }

    private bool IsTouchOnUI()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                Debug.Log("Touch on UI ture");
                return true;
            }
        }

        Debug.Log("Touch on UI false");
        return false;
    }
    #region MyCodeSupprt
    private List<GameObject> m_Platforms = new List<GameObject>(); 
    #region Visuals
    [SerializeField] private Background[] m_GameplayBGs;
    [SerializeField] private Image m_Background;
    [SerializeField] private Image m_Forground;
    [SerializeField] private ParallaxEffect m_Canvas;
    private int m_BGIndex;
    private void RandomBackground()
    {
        int random = UnityEngine.Random.Range(0, m_GameplayBGs.Length);
        m_Background.sprite = m_GameplayBGs[random].m_BackgroundImage;
        m_Forground.sprite = m_GameplayBGs[random].m_ForgroundImage;
        m_BGIndex = random;

    }
    

    
    private void GetSpriteSize()
    {
        SpriteRenderer redSprite = centerPointPrefab.GetComponent<SpriteRenderer>();
        Vector2 spriteSize = redSprite.bounds.size;
    }
    private void OnDrawGizmos()
    {
        SpriteRenderer redSprite = centerPointPrefab.GetComponent<SpriteRenderer>();
        Vector2 spriteSize = redSprite.bounds.size;
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(centerPointPrefab.transform.position, new Vector3(spriteSize.x,spriteSize.y,1));
    }
    #endregion
    #endregion
}
[Serializable]
public struct Background
{
    public Sprite m_BackgroundImage;
    public Sprite m_ForgroundImage;
}
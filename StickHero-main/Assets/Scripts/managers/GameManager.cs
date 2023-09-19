using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

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
    private GameObject pillarPrefab, centerPointPrefab, stickPrefab, orangePrefab, applePrefab, currentCamera, ColliderObject;
    [SerializeField] private GameObject[] playerPrefab;

    [SerializeField]
    private Transform rotateTransform, endRotateTransform;

    [SerializeField]
    private GameObject scorePanel, startPanel, endPanel, m_GamePlayScreen, m_StickTip;

    [SerializeField]
    private TMP_Text  scoreEndText, orangeText, appleText, highScoreText;
    public Text scoreText;

    private GameObject currentPillar, nextPillar, currentStick, player;
    public GameObject Player => player;

    private int score, oranges, apples, highScore;

    public float cameraOffsetX;

    private GameState currentState;

    [SerializeField]
    private float stickIncreaseSpeed, maxStickSize;

    public static GameManager instance;

    private int m_PlayerIndex;
    private Vector3 m_CurrentPlatform;
    [SerializeField] private float yOffset;
    [SerializeField] private Canvas m_GameplayCanvas;
    private Rigidbody2D m_PlayerRb;


    //for Jump
    public bool isMoving = false;



    //for inverted player
    private bool tapStarted = false; // Flag to track if a tap has started
    private float tapStartTime = 0f; // Time when the tap started
    private float tapThreshold = 0.2f; // Maximum time allowed for a single tap in seconds
    public GameObject cPillar, nPillar;



    //for perfect stick
    public float StickLenght;
    public Vector2 StartPoint;
    public Vector2 EndPoint;
    public GameObject tmpObjP;
    public GameObject PerfectlineObj;


    //for hiding Stick
    public GameObject cStick;
    public bool isPerfecScore = false;


    //for odd number of platforms
    public int NumberofPlatforms = 0;
    public GameObject RedPointer;
    GameObject redDot = null;
    public List<GameObject> sticks;
    public List<GameObject> redDots;


    //for camera setup
    public float CamVal;


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

        //setting camera offset
        //commented for testing
        //cameraOffsetX = currentCamera.transform.position.x - player.transform.position.x;

        GameStart();
        if(StateManager.instance.hasSceneStarted)
        {
        }
    }

    private void Update()
    {
        //if (!IsTouchOnUI())
        if (UIManager._instance.isGamestarted)
        {
            cStick = currentStick;
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

    }



    #region Player invert Region
    public bool canTap = false;
    ////IEnumerator CheckTap()
    //{
    //    //if (Input.touchCount > 0 && canTap == true)
    //    //{
    //    //    Touch touch = Input.GetTouch(0); // Get the first touch (assuming only one finger tap)

    //    //    if (touch.phase == TouchPhase.Began)
    //    //    {
    //    //        // The finger touched the screen, mark the start time
    //    //        tapStarted = true;
    //    //        tapStartTime = Time.time;
    //    //    }
    //    //    else if (touch.phase == TouchPhase.Ended && tapStarted)
    //    //    {
    //    //        // The finger was lifted, check if it's a single tap
    //    //        if (Time.time - tapStartTime <= tapThreshold)
    //    //        {
    //    //            // Handle the single tap
    //    //            Debug.Log("Tapped"+touch.position);
    //    //        }

    //    //        // Reset tap flag
    //    //        tapStarted = false;
    //    //    }
    //    //}

   
    //}


    #endregion



    void ScaleStick()
    {

        //enabling the stick
        cStick.SetActive(true);

        Vector3 tempScale = currentStick.transform.localScale;
        tempScale.y += Time.deltaTime * stickIncreaseSpeed;
        if (tempScale.y > maxStickSize)
            tempScale.y = maxStickSize;
        currentStick.transform.localScale = tempScale;

        StickLenght = tempScale.y;
        StartPoint = currentStick.transform.position;



        //Temp Code
        //Vector3 tempScale = currentStick.GetComponent<DistancedDestructor>().ObjectPoint.localScale;
        //tempScale.y += Time.deltaTime * stickIncreaseSpeed;
        //if (tempScale.y > maxStickSize)
        //    tempScale.y = maxStickSize;
        //currentStick.transform.localScale = tempScale;


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
                //print("Perfect");
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
                EndPoint = temp.collider.gameObject.GetComponent<DistancedDestructor>().ObjectPoint.position;
                tmpObjP = temp.collider.gameObject.GetComponent<DistancedDestructor>().ObjectPoint.gameObject;
                float perfectDistannce= Vector2.Distance(StartPoint, EndPoint);
                //Debug.Log("Distance" + perfectDistannce);
                float maxPerfect = perfectDistannce + 0.25f;
                float minPerfect = perfectDistannce - 0.25f;

                if(StickLenght==perfectDistannce || StickLenght==maxPerfect || StickLenght==minPerfect)
                {
                    PerfectlineObj.SetActive(true);
                    UpdateScore();
                    Debug.Log("perfect Line");
                }


                result = temp;
            }
        }





        if (!result || !result.collider.CompareTag("Platform"))
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
                sticks.Add(currentStick);
                cStick = currentStick;
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
            sticks.Add(currentStick);




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
        UIManager._instance.currentPlayer = player;

        Vector3 stickPos = stickPrefab.transform.position;
        stickPos.x += (currentPillar.transform.localScale.x*0.5f - 0.05f);
        currentStick = Instantiate(stickPrefab, stickPos, Quaternion.identity);
        sticks.Add(currentStick);
        cStick = currentStick;

    }

    void CreatePlatform()
    {
        GameObject currentPlatform = Instantiate(pillarPrefab);
        centerPointPrefab = currentPlatform.transform.GetChild(0).gameObject;// get red sprite as a perfect point
        currentPillar = nextPillar == null ? currentPlatform : nextPillar;
        //cPillar = currentPlatform;
        //nPillar = nextPillar;



        nextPillar = currentPlatform;
        currentPlatform.transform.position = pillarPrefab.transform.position + startPos;
        Vector3 tempDistance = new Vector3(UnityEngine.Random.Range(spawnRange.x,spawnRange.y) + currentPillar.transform.localScale.x*0.5f,0,0);
        startPos += tempDistance;
        m_CurrentPlatform = currentPlatform.transform.position+ new Vector3(transform.position.x, transform.position.y+ yOffset, 0);
       
 

        if (UnityEngine.Random.Range(0,1) == 0)
        {
            int randomPrefabe = UnityEngine.Random.Range(0, 2);
            //if (randomPrefabe == 0)
            //{
            //    pickable = Instantiate(applePrefab);
            //}
            //else
            //{
            //    pickable = Instantiate(orangePrefab);
            //}
            //find the distance between pillars
           
            
        }
        m_Platforms.Add(currentPlatform);


        //for pickables
        if(m_Platforms.Count>2)
        {
            if (m_Platforms[m_Platforms.Count - 1] != null && m_Platforms[m_Platforms.Count-2] != null)
            {
                float distanceBetween;
                distanceBetween = Vector3.Distance(m_Platforms[m_Platforms.Count - 2].transform.position, m_Platforms[m_Platforms.Count-1].transform.position);

                // Debug.Log("Distance between is " + Mathf.Round(distanceBetween * Mathf.Pow(10, 2)) / Mathf.Pow(10, 2));
                //Debug.Log("Distance between is without " + distanceBetween);
                //Debug.Log("Distance stick " + CurrenStick.transform.localScale.y);

                GameObject pickable;

                if (distanceBetween >= 4f)
                {
                    pickable = Instantiate(orangePrefab);
                    Vector3 tempPos = m_CurrentPlatform;
                    tempPos.y = orangePrefab.transform.position.y - 1f;
                    pickable.transform.position = tempPos;
                    pickable.transform.position = new Vector3(player.transform.position.x + 1.5f, tempPos.y, tempPos.z);
                }


            }
        }




        // adding the number of platforms 
        NumberofPlatforms++;
        GameObject CurrenStick = currentStick;
        if (NumberofPlatforms != 0 && NumberofPlatforms % 2 != 0 && NumberofPlatforms != 1)
        {
            //spawn red dot
            redDot = Instantiate(RedPointer);
            redDot.transform.position = new Vector3(currentPlatform.transform.position.x, -3.1f, 0f);
            redDots.Add(redDot);

            Debug.Log("Spawn Red Prefab");

        }

      


    }


    public void destroyRedDots()
    {
        for(int i=0;i<redDots.Count;i++)
        {
            redDots[i].SetActive(false);
        }
    }


    int indexNum = 0;

    void findingDistance()
    {
        Debug.Log("Distance work");

        cPillar = m_Platforms[indexNum];
        nPillar = m_Platforms[indexNum+1];


        float distanceBetween;
        if (cPillar != null && nPillar != null && redDot!=null)
        {
            // distanceBetween = Vector3.Distance(CurrenStick.transform.position, redDot.transform.position);
            // float Vel = distanceBetween / cStick.transform.localScale.y;

             distanceBetween = Vector3.Distance(sticks[indexNum].transform.position, redDot.transform.position);
            float roundDistance = Mathf.Round(distanceBetween * Mathf.Pow(10, 1)) / Mathf.Pow(10, 1);
            float roundStickDistance = Mathf.Round(sticks[indexNum].transform.localScale.y * Mathf.Pow(10, 1)) / Mathf.Pow(10, 1);

            Debug.Log("Distance between is " + roundDistance);
            //Debug.Log("Distance between is without " + distanceBetween);
            Debug.Log("Distance stick " + sticks[indexNum].transform.localScale.y);
            Debug.Log("Distance between stick with round " + roundStickDistance);



            float tmpMax = roundDistance + 0.1f;
            float tmpMin = roundDistance - 0.1f;

            if(roundStickDistance>=tmpMin && roundStickDistance<=tmpMax)
            {
                Debug.LogError("Perfect stick");
                PerfectlineObj.SetActive(true);
                score++;
                scoreText.text = score.ToString();

            }



            //if (roundStickDistance==roundDistance)
            //{
            //    Debug.LogError("Perfect stick");
            //}


            //GameObject pickable;

            //if (distanceBetween >= 2.2f)
            //{
            //    pickable = Instantiate(orangePrefab);
            //    Vector3 tempPos = m_CurrentPlatform;
            //    tempPos.y = orangePrefab.transform.position.y - 1f;
            //    pickable.transform.position = tempPos;
            //    pickable.transform.position = new Vector3(tempPos.x - 1.5f, tempPos.y, tempPos.z);
            //}
        }


        indexNum++;


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
        findingDistance();
    }

    public void GameOver()
    {
       // foreach (var item in m_Platforms)
            //item.SetActive(false);

        
        m_GameplayCanvas.sortingOrder = 10;
        endPanel.SetActive(true);
        m_GamePlayScreen.SetActive(false);
        scorePanel.SetActive(false);
        destroyRedDots();

        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        scoreEndText.text = score.ToString();
        highScoreText.text = highScore.ToString();
        
    }

    //own Custom functions
     void instantiateObject()
     {
        Transform pos =stickPrefab.GetComponent<DistancedDestructor>().ObjectPoint;
        Instantiate(ColliderObject,pos.position,Quaternion.identity);
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
            passed +=  Time.deltaTime;
            var normalized = passed / time;
            Debug.Log("Normal Value" + normalized);
            var current = Vector3.Lerp(init, target,normalized);
            //var current = Vector3.Lerp(init, target, 0.2f);

            currentTransform.position = current;
            isMoving = true;
            UIManager._instance.invertbtn.SetActive(true);
            Invoke("waitForMoving",1.5f);
            yield return null;
        }
    }

    void waitForMoving()
    {
        UIManager._instance.invertbtn.SetActive(false);
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
    [SerializeField]
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
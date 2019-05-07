using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
    using System.Collections.Generic;       
    using UnityEngine.UI;                  
using Completed;

public class GameManager : MonoBehaviour
    {
        public float levelStartDelay = 2f;                      
        public float turnDelay = 0.1f;                          
        public int playerFoodPoints = 100;                      
        public static GameManager instance = null;              
        [HideInInspector] public bool playersTurn = true;       

        private Text levelText;                                 
        private GameObject levelImage;                          
        private BoardManager boardScript;                       
        private int level = 1;                                  
        private List<Enemy> enemies;                            
        private bool enemiesMoving;                             
        private bool doingSetup = true;                         

        //Awake is always called before any Start functions
        void Awake()
        {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);

            //Assign enemies to a new List of Enemy objects.
            enemies = new List<Enemy>();

            //Get a component reference to the attached BoardManager script
            boardScript = GetComponent<BoardManager>();

            //Call the InitGame function to initialize the first level 
            InitGame();
        }

        //this is called only once, and the paramter tell it to be called only after the scene was loaded
        //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static public void CallbackInitialization()
        {
            //register the callback to be called everytime the scene is loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        //This is called each time a scene is loaded.
        static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            instance.level++;
            instance.InitGame();
        }


        //Initializes the game for each level.
        void InitGame()
        {
            doingSetup = true;

            levelImage = GameObject.Find("LevelImage");

            levelText = GameObject.Find("LevelText").GetComponent<Text>();

            levelText.text = "Day " + level;

            levelImage.SetActive(true);

            Invoke("HideLevelImage", levelStartDelay);

            enemies.Clear();

            boardScript.SetupScene(level);

        }
        void HideLevelImage()
        {
            levelImage.SetActive(false);

            doingSetup = false;
        }

        void Update()
        {
            if (playersTurn || enemiesMoving || doingSetup)
                return;

        }

        public void AddEnemyToList(Enemy script)
        {
            enemies.Add(script);
        }

        public void GameOver()
        {
            levelText.text = "After " + level + " days, you starved.";

            levelImage.SetActive(true);

            enabled = false;
        }

        IEnumerator MoveEnemies()
        {
            enemiesMoving = true;

            yield return new WaitForSeconds(turnDelay);

            if (enemies.Count == 0)
            {
                yield return new WaitForSeconds(turnDelay);
            }

            for (int i = 0; i<enemies.Count; i++)
            {
                enemies[i].MoveEnemy();

        yield return new WaitForSeconds(enemies[i].moveTime);
    }
    playersTurn = true;
            enemiesMoving = false;
        }
    }


//using UnityEngine;
//using System.Collections;
//using UnityEngine.SceneManagement;
//using System.Collections;
//using Completed;

//public class GameManager : MonoBehaviour
//{
//    public static GameManager instance = null;
//    public BoardManager boardScript;
//    private int level = 3;

//    private void Awake()
//    {
//            boardScript = GetComponent<BoardManager>();
//            InitGame();
//    }
//    void InitGame()
//    {
//        boardScript.SetupScene(level);
//    }

//    private void Update()
//    {
//    }
//}
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
            //While doingSetup is true the player can't move, prevent player from moving while title card is up.
            doingSetup = true;

            //Get a reference to our image LevelImage by finding it by name.
            levelImage = GameObject.Find("LevelImage");

            //Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
            levelText = GameObject.Find("LevelText").GetComponent<Text>();

            //Set the text of levelText to the string "Day" and append the current level number.
            levelText.text = "Day " + level;

            //Set levelImage to active blocking player's view of the game board during setup.
            levelImage.SetActive(true);

            //Call the HideLevelImage function with a delay in seconds of levelStartDelay.
            Invoke("HideLevelImage", levelStartDelay);

            //Clear any Enemy objects in our List to prepare for next level.
            enemies.Clear();

            //Call the SetupScene function of the BoardManager script, pass it current level number.
            boardScript.SetupScene(level);

        }


        //Hides black image used between levels
        void HideLevelImage()
        {
            //Disable the levelImage gameObject.
            levelImage.SetActive(false);

            //Set doingSetup to false allowing player to move again.
            doingSetup = false;
        }

        //Update is called every frame.
        void Update()
        {
            //Check that playersTurn or enemiesMoving or doingSetup are not currently true.
            if (playersTurn || enemiesMoving || doingSetup)

                //If any of these are true, return and do not start MoveEnemies.
                return;

            //Start moving enemies.
          //  StartCoroutine(MoveEnemies());
        }

        //Call this to add the passed in Enemy to the List of Enemy objects.
        public void AddEnemyToList(Enemy script)
        {
            //Add Enemy to List enemies.
            enemies.Add(script);
        }


        //GameOver is called when the player reaches 0 food points
        public void GameOver()
        {
            //Set levelText to display number of levels passed and game over message
            levelText.text = "After " + level + " days, you starved.";

            //Enable black background image gameObject.
            levelImage.SetActive(true);

            //Disable this GameManager.
            enabled = false;
        }

       // Coroutine to move enemies in sequence.
        IEnumerator MoveEnemies()
        {
            //While enemiesMoving is true player is unable to move.
            enemiesMoving = true;

            //Wait for turnDelay seconds, defaults to .1 (100 ms).
            yield return new WaitForSeconds(turnDelay);

            //If there are no enemies spawned (IE in first level):
            if (enemies.Count == 0)
            {
                //Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
                yield return new WaitForSeconds(turnDelay);
            }

            //Loop through List of Enemy objects.
            for (int i = 0; i<enemies.Count; i++)
            {
                //Call the MoveEnemy function of Enemy at index i in the enemies List.
                enemies[i].MoveEnemy();

        //Wait for Enemy's moveTime before moving next Enemy, 
        yield return new WaitForSeconds(enemies[i].moveTime);
    }
    //Once Enemies are done moving, set playersTurn to true so player can move.
    playersTurn = true;

            //Enemies are done moving, set enemiesMoving to false.
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
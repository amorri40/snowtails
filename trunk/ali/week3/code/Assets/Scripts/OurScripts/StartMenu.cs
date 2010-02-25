using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour
    {

    public const int SINGLEPLAYER = 1, SPLITSCREEN2 = 2;
    public int mode = 0;
    public Transform playerPrefab;
    private Transform player1, player2, player3, player4;

    // Use this for initialization
    void Start()
        {
        Time.timeScale = 0;
        }

    // Update is called once per frame
    void Update()
        {

        }

    void startSinglePlayer() {
    mode = SINGLEPLAYER;
    Time.timeScale = 1;
    Transform player = (Transform)Instantiate(playerPrefab, new Vector3(56, 14, -22), Quaternion.identity);
    player1 = player.Find("PlayerSled");
    ((Movement)player1.GetComponent(typeof(Movement))).playerid=1;
    enabled = false;
        }

    void startSplitScreen2Player()
        {
        mode = SPLITSCREEN2;
        Time.timeScale = 1;
        Transform player = (Transform)Instantiate(playerPrefab, new Vector3(56, 14, -22), Quaternion.identity);
        player1 = player.Find("PlayerSled");
        player1.Find("FirstPersonCamera").camera.rect = new Rect(0.0f, 0.5f, 1.0f, 0.5f);
        player1.Find("ThirdPersonCamera").camera.rect = new Rect(0.0f, 0.5f, 1.0f, 0.5f);
        ((Movement)player1.GetComponent(typeof(Movement))).playerid = 1;

        player = (Transform)Instantiate(playerPrefab, new Vector3(56, 14, -22), Quaternion.identity);
        player2 = player.Find("PlayerSled");
        player2.Find("FirstPersonCamera").camera.rect = new Rect(0.0f, 0.0f, 1.0f, 0.5f);
        player2.Find("ThirdPersonCamera").camera.rect = new Rect(0.0f, 0.0f, 1.0f, 0.5f);
        ((Movement)player2.GetComponent(typeof(Movement))).playerid = 2;
        enabled = false;
        }

    void OnGUI()
        {
        // Make a group on the center of the screen
        GUI.BeginGroup(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 400));
        // All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.

        // We'll make a box so you can see where the group is on-screen.
        GUI.Box(new Rect(0, 0, 400, 400), "Start Menu");
        //GUI.Button(new Rect(5, 370, 80, 20), "Back to menu");
        if (GUILayout.Button("Single Player"))
            {
            startSinglePlayer();
            }
        if (GUILayout.Button("2 Player split screen"))
            {
            startSplitScreen2Player();
            }
        // End the group we started above. This is very important to remember!
        GUI.EndGroup();

        }
    }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
public class NecromancerController : MonoBehaviour
{
    
    private PhotonView pv;

    public GameObject canvas;
    public GameObject pauseMenu;
    public Slider manaSlider;
    public float cameraSpeed;
    public float mana;
    public float maxMana;
    public float manaRecoveredPerSecond;

    public float skeletonCost;
    public float goblinCost;
    public float flyingEyeCost;

    private float startTime;

    private Camera camera;
    private Vector2 velocity;
    private Vector2 spawnPosition;
    public GameObject spawnMarker;
    

    public string[] minionPrefabNames;
    public GameObject[] minionButtons;
    private float[] minionCosts;
    private int minionSelected = 0;


    void Start()
    {
        /*Load these into an array for easier use*/
        minionCosts = new float[]{skeletonCost, goblinCost, flyingEyeCost};

        pv = GetComponent<PhotonView>();
        startTime = Time.time;

        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        canvas.SetActive(true);
        canvas.GetComponent<Canvas>().worldCamera = camera;

        SetMaxMana();
        markSpawnLocations();
    }

    void Update()
    {
        if (pv.IsMine)
        {
            Vector2 input = new Vector2(0,0);

            if (!pauseMenu.activeSelf)
            {
                //check for horizontal movement
                if (KeyBindingManager.GetKey(KeyAction.right)) input.x++;
                if (KeyBindingManager.GetKey(KeyAction.left)) input.x--;

                //check for vertical movement    
                if (KeyBindingManager.GetKey(KeyAction.up)) input.y++;
                if (KeyBindingManager.GetKey(KeyAction.down)) input.y--;

                velocity = input.normalized * cameraSpeed;

                if (KeyBindingManager.GetKeyDown(KeyAction.fire1))
                {
                    spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.zero);

                    if (hit.collider != null && hit.collider.tag == "Spawn Point") SpawnMinion(spawnPosition);
                }
            }

            if (KeyBindingManager.GetKeyDown(KeyAction.pause)) pauseMenu.SetActive(!pauseMenu.activeSelf);

            RecoverMana();
        }

    }
    private void FixedUpdate()
    {
        camera.transform.Translate(velocity * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Adds a small particle effect for the necromancer to mark where spawn locations are
    /// </summary>
    private void markSpawnLocations()
    {
        GameObject[] spawnLocations = GameObject.FindGameObjectsWithTag("Spawn Point");
        foreach (GameObject spawn in spawnLocations)
        {
            Instantiate(spawnMarker, spawn.transform);
        }
    }

    /// <summary>
    /// Spawns the currently selected minion at the given position.
    /// </summary>
    /// <param name="spawnPosition">A Vector2 position for the minion to be spawned.</param>
    private void SpawnMinion(Vector2 spawnPosition)
    {
        if (mana >= minionCosts[minionSelected])
        {
            mana -= minionCosts[minionSelected];
            manaSlider.value = mana;
            startTime = Time.time;
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", minionPrefabNames[minionSelected]), spawnPosition, Quaternion.identity);
        }
    }

    /// <summary>
    /// Increase mana based off of time passed.
    /// </summary>
    public void RecoverMana()
    {
        if (mana < maxMana) mana += manaRecoveredPerSecond * (Time.time - startTime);
        if (mana > maxMana) mana = maxMana;

        startTime = Time.time;
        manaSlider.value = mana;
    }

    /// <summary>
    /// Set the max value of the mana bar.
    /// </summary>
    public void SetMaxMana()
    {
        manaSlider.maxValue = maxMana;
        manaSlider.value = mana;
    }

    /// <summary>
    /// Changes the minionSelected to the given selection.
    /// </summary>
    /// <param name="selection">Index of the selected minion in the minionPrefabNames array.</param>
    public void SelectMinion(int selection)
    {
        minionSelected = selection;
    }
}

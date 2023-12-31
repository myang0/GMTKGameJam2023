using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Taskbar : MonoBehaviour
{
    public RectTransform taskbarPanelTransform;

    public TaskbarGhost greenTaskbarGhost;
    public TaskbarGhost redTaskbarGhost;
    public TaskbarGhost blueTaskbarGhost;

    GhostSettings ghostSettings;

    public List<GameObject> ghosts;

    public bool hidden = false;
    public bool draggingGhost = false;

    public GameObject currentGhost = null;
    public TaskbarGhost currentTaskbarGhost = null;

    public GameObject smokeParticles;

    Tilemap _tilemap;
    Camera _camera;

    void Start()
    {
        ghosts = new List<GameObject>();

        _tilemap = GameObject.FindWithTag("DroppableTilemap").GetComponent<Tilemap>();
        _camera = Camera.main;

        GridMovementManager.Instance.onGridMovementStart += HandleMovementStart;
        GridMovementManager.Instance.onGridReset += HandleGridReset;

        ghostSettings = GridMovementManager.Instance.ghostSettings;

        greenTaskbarGhost.Initialize(ghostSettings.greenGhosts);
        redTaskbarGhost.Initialize(ghostSettings.redGhosts);
        blueTaskbarGhost.Initialize(ghostSettings.blueGhosts);
    }

    void Update()
    {
        if (hidden)
            return;

        InputLogic();

        if (draggingGhost)
        {
            Vector3 mousePositionInWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
            if (currentGhost != null)
            {
                Vector3 newGhostPosition = new Vector3(
                    mousePositionInWorld.x,
                    mousePositionInWorld.y,
                    0
                );
                currentGhost.transform.position = newGhostPosition;
            }
        }
    }

    void HandleMovementStart()
    {
        gameObject.SetActive(false);
        hidden = true;
    }

    void HandleGridReset()
    {
        gameObject.SetActive(true);
        hidden = false;
    }

    public void ClearGhosts()
    {
        if (ghosts.Count > 0)
        {
            SoundManager.Instance.PlaySound(Sound.GhostReset, volumeScaling: 0.3f);
        }

        foreach (GameObject ghost in ghosts)
        {
            Destroy(ghost);
        }

        ghosts = new List<GameObject>();

        greenTaskbarGhost.Initialize(ghostSettings.greenGhosts);
        redTaskbarGhost.Initialize(ghostSettings.redGhosts);
        blueTaskbarGhost.Initialize(ghostSettings.blueGhosts);
    }

    void InputLogic()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickLogic();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseLogic();
        }
    }

    void ClickLogic()
    {
        if (greenTaskbarGhost.hoveredOver && greenTaskbarGhost.remainingGhosts > 0)
        {
            draggingGhost = true;
            SoundManager.Instance.PlaySound(Sound.Click);

            currentGhost = greenTaskbarGhost.HandleDrag();
            currentTaskbarGhost = greenTaskbarGhost;

            return;
        }

        if (redTaskbarGhost.hoveredOver && redTaskbarGhost.remainingGhosts > 0)
        {
            draggingGhost = true;
            SoundManager.Instance.PlaySound(Sound.Click);

            currentGhost = redTaskbarGhost.HandleDrag();
            currentTaskbarGhost = redTaskbarGhost;

            return;
        }

        if (blueTaskbarGhost.hoveredOver && blueTaskbarGhost.remainingGhosts > 0)
        {
            draggingGhost = true;
            SoundManager.Instance.PlaySound(Sound.Click);

            currentGhost = blueTaskbarGhost.HandleDrag();
            currentTaskbarGhost = blueTaskbarGhost;

            return;
        }
    }

    void ReleaseLogic()
    {
        if (!draggingGhost)
            return;
        
        Vector3 mousePositionInWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosOnMouse = _tilemap.WorldToCell(mousePositionInWorld);
        TileBase tileOnMouse = _tilemap.GetTile(cellPosOnMouse);
        
        if (tileOnMouse == null || tileOnMouse.name == "Textures-16_51")
        {
            Destroy(currentGhost);
            currentTaskbarGhost.ReleaseGhost();
        }
        else
        {
            Vector3 positionInMaze = new Vector3(cellPosOnMouse.x + 0.5f, cellPosOnMouse.y + 0.5f, 0);

            currentGhost.transform.position = positionInMaze;
            currentGhost.GetComponent<GridMovementController>().SetNewOriginalPosition(positionInMaze);

            ghosts.Add(currentGhost);
            SoundManager.Instance.PlaySound(Sound.Spawn, volumeScaling: 0.75f);
            Instantiate(smokeParticles, new Vector3(positionInMaze.x, positionInMaze.y - 0.5f, 0), Quaternion.identity);
        }

        currentGhost = null;
        currentTaskbarGhost = null;

        draggingGhost = false;
    }
}

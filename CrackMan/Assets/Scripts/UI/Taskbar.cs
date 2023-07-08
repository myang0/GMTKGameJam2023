using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Taskbar : MonoBehaviour
{
    public RectTransform taskbarPanelTransform;
    public List<TaskbarGhost> taskbarGhosts;
    public List<GameObject> ghosts;

    public bool hidden = false;
    public bool draggingGhost = false;

    public GameObject currentGhost = null;
    public TaskbarGhost currentTaskbarGhost = null;

    public Tilemap tilemap;

    Camera _camera;

    void Start()
    {
        ghosts = new List<GameObject>();
        _camera = Camera.main;

        GridMovementManager.Instance.onGridMovementStart += HandleMovementStart;
        GridMovementManager.Instance.onGridReset += HandleGridReset;

        foreach (TaskbarGhost tg in taskbarGhosts)
        {
            tg.Initialize(2);
        }
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
        foreach (GameObject ghost in ghosts)
        {
            Destroy(ghost);
        }

        ghosts = new List<GameObject>();

        foreach (TaskbarGhost taskbarGhost in taskbarGhosts)
        {
            taskbarGhost.AddToGhostCount(2);
        }
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
        foreach (TaskbarGhost taskbarGhost in taskbarGhosts)
        {
            if (taskbarGhost.hoveredOver && taskbarGhost.remainingGhosts > 0)
            {
                draggingGhost = true;

                currentGhost = taskbarGhost.HandleDrag();
                currentTaskbarGhost = taskbarGhost;
            }
        }
    }

    void ReleaseLogic()
    {
        if (!draggingGhost)
            return;
        
        Vector3 mousePositionInWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosOnMouse = tilemap.WorldToCell(mousePositionInWorld);
        TileBase tileOnMouse = tilemap.GetTile(cellPosOnMouse);
        
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
        }

        currentGhost = null;
        currentTaskbarGhost = null;

        draggingGhost = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GhostDragger : MonoBehaviour
{
    public Vector3 originalGhostPosition;
    public GameObject currentGhost;

    public GameObject smokeParticles;

    Tilemap _tilemap;
    Camera _camera;

    void Start()
    {
        _tilemap = GameObject.FindWithTag("DroppableTilemap").GetComponent<Tilemap>();
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickLogic();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseLogic();
        }

        if (currentGhost != null)
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

    void ClickLogic()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, LayerMask.GetMask("Ghost"));

        if (hit.collider != null)
        {
            originalGhostPosition = hit.collider.gameObject.transform.position;
            currentGhost = hit.collider.gameObject;
        }
    }

    void ReleaseLogic()
    {
        if (currentGhost == null)
            return;

        Vector3 mousePositionInWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosOnMouse = _tilemap.WorldToCell(mousePositionInWorld);
        TileBase tileOnMouse = _tilemap.GetTile(cellPosOnMouse);
        
        if (tileOnMouse == null || tileOnMouse.name == "Textures-16_51")
        {
            currentGhost.transform.position = originalGhostPosition;
        }
        else
        {
            Vector3 positionInMaze = new Vector3(cellPosOnMouse.x + 0.5f, cellPosOnMouse.y + 0.5f, 0);

            currentGhost.transform.position = positionInMaze;
            currentGhost.GetComponent<GridMovementController>().SetNewOriginalPosition(positionInMaze);

            SoundManager.Instance.PlaySound(Sound.Spawn, volumeScaling: 0.75f);
            Instantiate(smokeParticles, new Vector3(positionInMaze.x, positionInMaze.y - 0.5f, 0), Quaternion.identity);
        }

        currentGhost = null;
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InfiniteTiledParallaxWrapped : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector2 parallaxEffect = new Vector2(0.5f, 0.5f);
    public int tileCountX = 3;
    public int tileCountY = 3;

    private GameObject[,] tiles;
    private float tileWidth, tileHeight;
    private Vector3 lastCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null)
        {
            return;
        }

        tileWidth = sr.bounds.size.x;
        tileHeight = sr.bounds.size.y;

        tiles = new GameObject[tileCountX, tileCountY];
        Vector3 origin = transform.position;

        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                Vector3 pos = origin + new Vector3((x - (tileCountX - 1) / 2f) * tileWidth, (y - (tileCountY - 1) / 2f) * tileHeight, 0);
                GameObject tile = Instantiate(gameObject, pos, Quaternion.identity, transform.parent);
                DestroyImmediate(tile.GetComponent<InfiniteTiledParallaxWrapped>());
                tiles[x, y] = tile;
            }
        }
    }

    void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - lastCameraPosition;

        foreach (GameObject tile in tiles)
        {
            tile.transform.position += new Vector3(delta.x * parallaxEffect.x, delta.y * parallaxEffect.y, 0f);
        }

        lastCameraPosition = cameraTransform.position;
        RepositionTilesIfNeeded();
    }

    void RepositionTilesIfNeeded()
    {
        float camX = cameraTransform.position.x;
        float camY = cameraTransform.position.y;

        float halfWidth = tileWidth * tileCountX / 2f;
        float halfHeight = tileHeight * tileCountY / 2f;

        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                GameObject tile = tiles[x, y];
                Vector3 tilePos = tile.transform.position;

                float deltaX = camX - tilePos.x;
                float deltaY = camY - tilePos.y;

                if (Mathf.Abs(deltaX) > halfWidth)
                {
                    float offsetX = tileWidth * tileCountX * Mathf.Sign(deltaX);
                    tile.transform.position += new Vector3(offsetX, 0f, 0f);
                }

                if (Mathf.Abs(deltaY) > halfHeight)
                {
                    float offsetY = tileHeight * tileCountY * Mathf.Sign(deltaY);
                    tile.transform.position += new Vector3(0f, offsetY, 0f);
                }
            }
        }
    }
}
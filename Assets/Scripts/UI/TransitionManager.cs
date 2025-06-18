using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum TransitionType
{
    Enter, // Block rơi vào để che màn
    Exit   // Block từ từ rơi ra để mở màn
}

public class TransitionManager : MonoBehaviour
{
    [Header("Prefab & Cài đặt")]
    public GameObject blockPrefab;
    public Vector2Int gridSize = new Vector2Int(10, 6);
    public float delayBetweenBlocks = 0.05f;
    public float delayBeforeFallOut = 2f;

    private List<GameObject> spawnedBlocks = new();

    public void Play(TransitionType type, Action onComplete = null)
    {
        switch (type)
        {
            case TransitionType.Enter:
                StartCoroutine(PlayEnterTransition(onComplete));
                break;
            case TransitionType.Exit:
                StartCoroutine(PlayExitTransition());
                break;
        }
    }

    private IEnumerator PlayEnterTransition(Action onComplete)
    {
        StopCameraFollow();

        if (PlayerInputHandler.Instance != null)
        {
            PlayerInputHandler.Instance.DisableInput();
        }

        UIUtils.SetAllButtonsInteractableAllScenes(false);

        List<Vector2Int> dropOrder = GetCustomDropOrder(gridSize);
        spawnedBlocks.Clear();

        foreach (var cell in dropOrder)
        {
            GameObject block = Instantiate(blockPrefab, GetWorldSpawnPosition(cell), Quaternion.identity, transform);
            spawnedBlocks.Add(block);
            Vector3 target = GetWorldTargetPosition(cell);
            StartCoroutine(MoveBlock(block.transform, target, 0.3f));
            yield return new WaitForSeconds(delayBetweenBlocks);
        }

        yield return new WaitForSeconds(delayBeforeFallOut);

        onComplete?.Invoke();
    }

    private IEnumerator PlayExitTransition()
    {
        StopCameraFollow();

        yield return new WaitForSeconds(0.1f);

        if (spawnedBlocks.Count == 0)
            SpawnBlocksCoverScreen();

        foreach (var block in spawnedBlocks)
        {
            Vector3 target = block.transform.position + Vector3.down * (Camera.main.orthographicSize * 2f);
            StartCoroutine(MoveAndDestroyBlock(block.transform, target, 0.5f));
            yield return new WaitForSeconds(delayBetweenBlocks);
        }

        spawnedBlocks.Clear();

        if (PlayerInputHandler.Instance != null)
        {
            PlayerInputHandler.Instance.EnableInput();
        }

        RestoreCameraFollow();
        UIUtils.SetAllButtonsInteractableAllScenes(true);
    }

    private IEnumerator MoveAndDestroyBlock(Transform block, Vector3 targetPos, float duration)
    {
        float elapsed = 0f;
        Vector3 start = block.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            block.position = Vector3.Lerp(start, targetPos, t);
            yield return null;
        }

        block.position = targetPos;
        Destroy(block.gameObject); 
    }

    public void SpawnBlocksCoverScreen()
    {
        List<Vector2Int> dropOrder = GetCustomDropOrder(gridSize);
        spawnedBlocks.Clear();

        foreach (var cell in dropOrder)
        {
            Vector3 pos = GetWorldTargetPosition(cell);
            GameObject block = Instantiate(blockPrefab, pos, Quaternion.identity, transform);
            spawnedBlocks.Add(block);
        }
    }

    private IEnumerator MoveBlock(Transform block, Vector3 targetPos, float duration)
    {
        float elapsed = 0f;
        Vector3 start = block.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            block.position = Vector3.Lerp(start, targetPos, t);
            yield return null;
        }

        block.position = targetPos;
    }

    private Vector3 GetWorldSpawnPosition(Vector2Int cell)
    {
        Vector3 topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        float x = Mathf.Floor(topLeft.x) + cell.x;
        float y = Mathf.Ceil(topLeft.y) + 1f; // spawn cao hơn 1 block
        return new Vector3(x, y, 0);
    }

    private Vector3 GetWorldTargetPosition(Vector2Int cell)
    {
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(Vector3.zero);
        float x = Mathf.Floor(bottomLeft.x) + cell.x;
        float y = Mathf.Floor(bottomLeft.y) + cell.y;
        return new Vector3(x, y, 0);
    }

    private List<Vector2Int> GetCustomDropOrder(Vector2Int size)
    {
        List<Vector2Int> order = new();

        for (int y = size.y - 1; y >= 0; y--)
        {
            for (int x = 0; x < size.x; x++)
            {
                order.Add(new Vector2Int(x, y));
            }
        }

        return order;
    }

    private void StopCameraFollow()
    {
        CinemachineCamera cam = FindFirstObjectByType<CinemachineCamera>();
        if (cam != null)
        {
            cam.Follow = null;
            cam.LookAt = null;
            Debug.Log("[Camera] ✅ Dừng Follow/LookAt");
        }
    }

    private void RestoreCameraFollow()
    {
        CinemachineCamera cam = FindFirstObjectByType<CinemachineCamera>();
        if (cam == null)
        {
            Debug.LogWarning("[Camera] ⚠ Không tìm thấy CinemachineCamera để khôi phục Follow");
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("[Camera] ⚠ Không tìm thấy Player để gán lại Follow/LookAt");
            return;
        }

        cam.Follow = player.transform;
        cam.LookAt = player.transform;
        Debug.Log("[Camera] ✅ Đã khôi phục Follow/LookAt về Player");
    }
}

using System.Collections.Generic;
using UnityEngine;

public class PiecePreviewDisplay : MonoBehaviour
{
    [System.Serializable]
    public struct PreviewEntry
    {
        public Tetromino tetromino;
        public Piece prefab;
		  public Vector3 localOffset;
    }

    [SerializeField] private PreviewEntry[] prefabs;
    [SerializeField] private Transform[] slots;
    [SerializeField] private Vector3 previewScale = Vector3.one * 0.6f;

    private readonly List<GameObject> spawned = new List<GameObject>();
    private Dictionary<Tetromino, Piece> prefabLookup;

    private void Awake()
    {
        prefabLookup = new Dictionary<Tetromino, Piece>();
        foreach (var entry in prefabs)
        {
            if (entry.prefab != null)
                prefabLookup[entry.tetromino] = entry.prefab;
        }
    }

    public void SetQueue(Tetromino[] queue)
    {
        ClearSpawned();

        int count = Mathf.Min(slots.Length, queue.Length);
        for (int i = 0; i < count; i++)
        {
            SpawnPreview(queue[i], slots[i]);
        }
    }

    public void SetPiece(Tetromino? tetromino)
    {
        ClearSpawned();

        if (tetromino.HasValue && slots.Length > 0)
        {
            SpawnPreview(tetromino.Value, slots[0]);
        }
    }

    private void SpawnPreview(Tetromino tetromino, Transform slot)
    {
        if (!prefabLookup.TryGetValue(tetromino, out Piece prefab) || prefab == null)
            return;

        Piece instance = Instantiate(prefab, slot.position, Quaternion.identity, slot);
        instance.enabled = false;

        Collider[] colliders = instance.GetComponentsInChildren<Collider>(true);
        foreach (var c in colliders) c.enabled = false;

        Collider2D[] colliders2D = instance.GetComponentsInChildren<Collider2D>(true);
        foreach (var c in colliders2D) c.enabled = false;

        instance.transform.localScale = previewScale;
        instance.transform.localPosition = Vector3.zero;

        spawned.Add(instance.gameObject);
    }

    private void ClearSpawned()
    {
        for (int i = 0; i < spawned.Count; i++)
        {
            if (spawned[i] != null)
                Destroy(spawned[i]);
        }

        spawned.Clear();
    }
}

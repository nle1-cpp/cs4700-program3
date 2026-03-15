using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] Tetrominoes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NewTetromino();
    }

    public void NewTetromino()
    {
        Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position, Quaternion.identity);
    }
}

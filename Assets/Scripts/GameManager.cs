using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject[] Tetrominos;
    public GameObject[] TetrominosPrediction;
    public GameObject gameover;
    public GameObject scoretxt;
    public GameObject endscore;
    public Transform SpawnPoint;
    public Transform Next;
    private bool Lost = false;
    public static int score = 0;
    public static int level = 1;
    private int next;
    public GameObject NextTetromino;
    public GameObject OldTetromino;
    void Start()
    {
        NewTetromino();
        StartCoroutine(AddScore());
    }
    public void NewTetromino()
    {
        if (!Lost)
        {
            if(NextTetromino == null)
            {
                int id = Random.Range(0, Tetrominos.Length);
                next = Random.Range(0, Tetrominos.Length);
                OldTetromino = Instantiate(Tetrominos[id], SpawnPoint.position, Quaternion.identity);
                OldTetromino.tag = "active";
                Instantiate(TetrominosPrediction[id], SpawnPoint.position, Quaternion.identity);
                NextTetromino = Instantiate(Tetrominos[next], Next.position, Quaternion.identity);
                NextTetromino.GetComponent<Tetromino>().enabled = false;
            }
            else
            {
                OldTetromino.tag = "notactive";
                Destroy(NextTetromino);
                OldTetromino = Instantiate(Tetrominos[next], SpawnPoint.position, Quaternion.identity);
                OldTetromino.tag = "active";
                Instantiate(TetrominosPrediction[next], SpawnPoint.position, Quaternion.identity);
                next = Random.Range(0, Tetrominos.Length);
                NextTetromino = Instantiate(Tetrominos[next], Next.position, Quaternion.identity);
                NextTetromino.GetComponent<Tetromino>().enabled = false;
            }
        }
    }
    public void GameOver()
    {
        endscore.GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + score;
        Time.timeScale = 0;
        Lost = true;
        gameover.SetActive(true);
    }
    public void StartOver()
    {
        gameover.SetActive(false);
        SceneManager.LoadScene(0);
        Lost = false;
        Time.timeScale = 1;
        score = 0;
        level = 1;
        UpdateSpeed();
    }
    private IEnumerator AddScore()
    {
        while (!Lost)
        {
            score += 1 * level;
            UpdateScore();
            yield return new WaitForSeconds(1);
        }
    }
    public void UpdateScore()
    {
        if(!Lost)
            scoretxt.GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + score;
        if (score > Mathf.RoundToInt(level * (100 * level) * 3f) && level < 7)
        {
            level++;
            UpdateSpeed();
        }
    }
    public void UpdateSpeed()
    {
        Tetromino.fallTime = 0.8f - (0.1f * (level));
    }
}

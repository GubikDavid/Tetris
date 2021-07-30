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
    public Transform Next1;
    public Transform Next2;
    public Transform Next3;
    private bool Lost = false;
    public static int score = 0;
    public static int level = 1;
    private int first;
    private int second;
    private int third;
    public GameObject FirstTetromino;
    public GameObject SecondTetromino;
    public GameObject ThirdTetromino;
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
            if(FirstTetromino == null)
            {
                int id = Random.Range(0, Tetrominos.Length);
                first = Random.Range(0, Tetrominos.Length);
                second = Random.Range(0, Tetrominos.Length);
                third = Random.Range(0, Tetrominos.Length);
                OldTetromino = Instantiate(Tetrominos[id], SpawnPoint.position, Quaternion.identity);
                OldTetromino.tag = "active";
                Instantiate(TetrominosPrediction[id], SpawnPoint.position, Quaternion.identity);
                FirstTetromino = Instantiate(Tetrominos[first], Next1.position, Quaternion.identity);
                FirstTetromino.GetComponent<Tetromino>().enabled = false;
                SecondTetromino = Instantiate(Tetrominos[second], Next2.position, Quaternion.identity);
                SecondTetromino.GetComponent<Tetromino>().enabled = false;
                ThirdTetromino = Instantiate(Tetrominos[third], Next3.position, Quaternion.identity);
                ThirdTetromino.GetComponent<Tetromino>().enabled = false;
            }
            else
            {
                OldTetromino.tag = "notactive";
                Destroy(FirstTetromino);
                Destroy(SecondTetromino);
                Destroy(ThirdTetromino);
                OldTetromino = Instantiate(Tetrominos[first], SpawnPoint.position, Quaternion.identity);
                OldTetromino.tag = "active";
                Instantiate(TetrominosPrediction[first], SpawnPoint.position, Quaternion.identity);
                first = second;
                second = third;
                third = Random.Range(0, Tetrominos.Length);
                FirstTetromino = Instantiate(Tetrominos[first], Next1.position, Quaternion.identity);
                FirstTetromino.GetComponent<Tetromino>().enabled = false;
                SecondTetromino = Instantiate(Tetrominos[second], Next2.position, Quaternion.identity);
                SecondTetromino.GetComponent<Tetromino>().enabled = false;
                ThirdTetromino = Instantiate(Tetrominos[third], Next3.position, Quaternion.identity);
                ThirdTetromino.GetComponent<Tetromino>().enabled = false;
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

using UnityEngine;
public class Tetromino : MonoBehaviour
{
    private float previousTime, mvPrevTime;
    public static float fallTime = .8f;
    public static int width = 10;
    public static int height = 20;
    public static Transform[,] grid = new Transform[width, height];
    public static bool isset = false;
    private void Start()
    {
        isset = false;
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow) && Time.time - mvPrevTime > .15f)
        {
            MoveTetromino(-1);
            mvPrevTime = Time.time;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && Time.time - mvPrevTime > .15f)
        {
            MoveTetromino(1); 
            mvPrevTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateTetromino();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            string x = ValidMove();
            while (x == "Success")
            {
                transform.position += new Vector3(0, -1, 0);
                x = ValidMove();
            }
            SetInPlace();
        }
        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / (10 - GameManager.level) : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            string x = ValidMove();
            if(x != "Success")
                SetInPlace();
            previousTime = Time.time;
        }
    }
    private void RotateTetromino()
    {
        if (transform.name.Contains("O Tetromino")) return;
        transform.RotateAround(transform.GetChild(0).position, Vector3.forward, -90);
        string x = ValidMove();
        if (x == "xgrid")
        {
            transform.position += new Vector3(transform.position.x < 4 ? transform.name.Contains("I Tetromino") ? 2 : 1 : transform.name.Contains("I Tetromino") ? -2 : -1, 0, 0);
            FindObjectOfType<TetrominoPrediction>().RotateTetromino();
        }
        else if (x == "object" || x == "ygrid")
            transform.RotateAround(transform.GetChild(0).position, Vector3.forward, 90);
        else FindObjectOfType<TetrominoPrediction>().RotateTetromino();
    }
    private void SetInPlace()
    {
        if (!isset)
        {
            transform.position -= new Vector3(0, -1, 0);
            AddToGrid();
            enabled = false;
            FindObjectOfType<TetrominoPrediction>().DestroyPrediction();
            FindObjectOfType<GameManager>().NewTetromino();
            isset = true;
        }
        
    }
    private void MoveTetromino(int i)
    {
        transform.position += new Vector3(i, 0, 0);
        string x = ValidMove();
        if (x == "xgrid" || x == "object")
            transform.position += new Vector3(-i, 0, 0);
        else 
            FindObjectOfType<TetrominoPrediction>().MoveTetromino(i);
    }
    private void AddToGrid()
    {
        foreach(Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.transform.position.x);
            int y = Mathf.RoundToInt(child.transform.position.y);
            if (y >= height)
                FindObjectOfType<GameManager>().GameOver();
            else
                grid[x, y] = child;
        }
        CheckPops();
        GameManager.score += 20;
        FindObjectOfType<GameManager>().UpdateScore();
    }
    private string ValidMove()
    {
        foreach(Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.transform.position.x);
            int y = Mathf.RoundToInt(child.transform.position.y);

            if (x < 0 || x > width - 1)
                return "xgrid";
            else if (y < 0)
                return "ygrid";
            else if (x < width && y < height && grid[x, y] != null)
                return "object";
        }
        return "Success";
    }
    private void CheckPops()
    {
        for (int y = 0; y < height; y++)
        {
            if (CheckLine(y))
            {
                DeleteLine(y);
                MoveDown(y);
                y--;
            }
        }
    }
    private bool CheckLine(int i)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, i] == null)
                return false;
        }
        return true;
    }
    private void DeleteLine(int i)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, i].gameObject);
            grid[x, i] = null;
        }
        GameManager.score += 50;
    }
    private void MoveDown(int i)
    {
        for(int y = i; y < height - 1; y++)
        {
            for(int x = 0; x < width; x++)
            {
                if(grid[x, y + 1] != null)
                {
                    grid[x, y] = grid[x, y + 1];
                    grid[x, y].transform.position -= Vector3.up;
                    grid[x, y + 1] = null;
                }
            }
        }
    }
}

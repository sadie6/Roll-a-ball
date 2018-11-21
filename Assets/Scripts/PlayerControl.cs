using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {
    private Rigidbody rb;
    public float speed;  //小球的速度
    public Text CountText;   //计分板
    private int count;   //吃掉的小球个数
    public GameObject WinGame;   //赢了的画面
    public GameObject restartBt;   //重新开始游戏按钮
    public GameObject GameOverText;  //输了的画面
    public GameObject StartGamePrefab;   //3 2 1开始动画
    private Boolean status = false; 
    public Text TimeText;     //计时器
    private int time = 10;    //倒计时时间
    private float intervalTime = 1;     //倒计时间隔
    private List<int> ranklist = new List<int>(); //存放score
    public GameObject ScorePrefab;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        
        restartBt.SetActive(false);
        TimeText.text = "倒计时:" + string.Format("{0}", time);

        StartCoroutine(StartGame_Animator(StartGamePrefab));   //游戏开始动画
    }

    private void Update()
    {
        if (time > 0 && status)
        {
            intervalTime -= Time.deltaTime;
            if (intervalTime <= 0)
            {
                intervalTime++;
                time--;
                TimeText.text = "倒计时:" + string.Format("{0}", time);
            }
        }
        else
        {
            if (status)
            {
                SetGameOver();
                status = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (status)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            rb.AddForce(movement * speed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    void SetGameOver()
    {
        Record();
        restartBt.SetActive(true);
        GameObject mUICanvas = GameObject.Find("Canvas");
        Instantiate(GameOverText, transform.parent = mUICanvas.transform);
        Time.timeScale = 0;
    }

    public void ReStartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }

    IEnumerator StartGame_Animator(GameObject prefab)   
    {
        GameObject go = Instantiate(prefab);
        yield return new WaitForSeconds(3.5F);
        Destroy(go);
        status = true;
    }

    void SetCountText()    
    {
        CountText.text = "分数:" + count.ToString();
        if (count >= 15)
        {
            Record();
            Time.timeScale = 0;
            restartBt.SetActive(true);
            GameObject mUICanvas = GameObject.Find("Canvas");
            Instantiate(WinGame,transform.parent = mUICanvas.transform);
        }

    }

    void Record()    //记录分数
    {
        if (!File.Exists(Application.dataPath + "/Resources/RankingList.txt"))   //不存在就新建文件
        {
            FileStream fs = new FileStream(Application.dataPath + "/Resources/RankingList.txt", FileMode.Create);
            fs.Close();
        }
        StreamReader sr = new StreamReader(Application.dataPath + "/Resources/RankingList.txt");//声明文本对象读取数据流       
        string nextLine;                                      //声明一个数据接受对象 
        while ((nextLine = sr.ReadLine()) != null)            //将所有存储的分数全部存到ranklist中 
        {
            ranklist.Add(int.Parse(nextLine));
        }
        sr.Close();                                            //关闭文件流 
        ranklist.Add(count);
        ranklist.Sort();
       /* foreach (int s in ranklist)
        {
            print(s);
        }*/
        StreamWriter sw = new StreamWriter(Application.dataPath + "/Resources/RankingList.txt");//声明文本对象写入数据流         
        if (ranklist.Count > 10)
        {
            for (int i = 10; i <= ranklist.Count; i++)
            {
                ranklist.RemoveAt(0);                         //移除超出排行榜之外的分数 
            }
            for (int i = 0; i < ranklist.Count; i++)
            {
                sw.WriteLine(ranklist[i]);//将score写入文本
            }
            ranklist.Clear();                                 //保存完毕清除数组 
            sw.Close();                                        //关闭文件流 
        }
        else
        {
            for (int i = 0; i < ranklist.Count; i++)
            {
                sw.WriteLine(ranklist[i]);//将score写入文本
            }
            ranklist.Clear();                                 //保存完毕清除数组 
            sw.Close();
        }
    }

}

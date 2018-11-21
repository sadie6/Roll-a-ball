using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Rank : MonoBehaviour {
    private List<int> ranklist = new List<int>();
    public GameObject Achievement;

	// Use this for initialization
	void Start () {
        RankDown();
    }

    void RankDown()
    {
        Time.timeScale = 1;
        if (!File.Exists(Application.dataPath + "/Resources/RankingList.txt"))
        {
            FileStream fs = new FileStream(Application.dataPath + "/Resources/RankingList.txt", FileMode.Create);
            fs.Close();
        }
        StreamReader sr = new StreamReader(Application.dataPath + "/Resources/RankingList.txt");
        string nextLine;
        while ((nextLine = sr.ReadLine()) != null)
        {
            ranklist.Add(int.Parse(nextLine));
        }
        sr.Close();

        for (int i = 0; i < ranklist.Count; i++)              //实例化排行榜 
        {
            GameObject item = Instantiate(Achievement);
            GameObject itemplace = GameObject.Find("Grid_Achievements");
            item.SetActive(true);
            item.transform.SetParent(itemplace.transform,false);
            item.transform.Find("Text_AchievementDescription").GetComponent<Text>().text = (i + 1).ToString();
            item.transform.Find("Text_RewardAmount").GetComponent<Text>().text = ranklist[ranklist.Count-1-i].ToString();
        }
        ranklist.Clear();
    }

   
	
}

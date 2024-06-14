using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderLineManager : MonoBehaviour
{
    public LeaderLine leaderLinePrefab;
    public LineRenderer linePrefab;

    public Transform leftContent;
    public Transform rightContent;
    public Transform lineContent;

    //public Transform target;
    public List<Transform> targets;

    private List<LeaderLine> leftLeaders = new List<LeaderLine>();
    private List<LeaderLine> rightLeaders = new List<LeaderLine>();
    
    
    //创建指示线
    public void CreateLeaderLine(Transform target,Transform leaderContent,List<LeaderLine> list)
    {
        //生成标签和线
        var line = Instantiate(linePrefab,lineContent);
        var leaderLine = Instantiate(leaderLinePrefab, leaderContent);
        //leaderLine.rect.anchoredPosition = new Vector2(0, 0);
        //为标签设置目标和线段
        leaderLine.Setup(this, target, line); 
        list.Add(leaderLine);
    }

    //将一边的Leader移动到另一边上
    public bool CheckNeedMoveLeadersToOtherSide(List<LeaderLine> list,List<LeaderLine> otherList, bool checkLeft)
    {
        List<LeaderLine> needMoveList = new List<LeaderLine>();
        foreach (var leader in list)
        {
            if (checkLeft != IsLeftSide(Camera.main, leader.target))
            {
                needMoveList.Add(leader);
            }
        }
        if (needMoveList.Count > 0)
        {
            foreach (var leader in needMoveList)
            {
                list.Remove(leader);
                otherList.Add(leader);
            }
            return true;
        }
        return false;
    }

    //为leaders下的所有leader标签进行排序
    public void SortAndPositionLeaders(List<LeaderLine> leaders,Transform content,float spaceY = -80f , float offsetY = -200f)
    {
        leaders.Sort((a, b) => b.target.position.y.CompareTo(a.target.position.y));
        for(int i =0;i< leaders.Count; i++)
        {
            leaders[i].transform.parent = content;
            leaders[i].rect.anchoredPosition = new Vector2(0, i * spaceY + offsetY);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            for (int i = 0; i < targets.Count; i++)
            {
                var target = targets[i];
                if (IsLeftSide(Camera.main, target))
                    CreateLeaderLine(target, leftContent, leftLeaders);
                else
                    CreateLeaderLine(target, rightContent, rightLeaders);
            }
        }
        bool needUpdateRight = CheckNeedMoveLeadersToOtherSide(leftLeaders, rightLeaders, true);
        bool needUpdateLeft = CheckNeedMoveLeadersToOtherSide(rightLeaders, leftLeaders, false);

        SortAndPositionLeaders(leftLeaders, leftContent);
        SortAndPositionLeaders(rightLeaders, rightContent);

    }



    private bool IsLeftSide(Camera camera, Transform transform)
    {
        return camera.WorldToScreenPoint(transform.position).x < (Screen.width / 2);
    }

}

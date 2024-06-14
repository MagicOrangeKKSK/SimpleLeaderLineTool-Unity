using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace Marisa
{
    public class LeaderLineManager : MonoBehaviour
    {
        public SplitModel splitModel;
        public LeaderLine leaderLinePrefab;
        public Camera uiCamera;

        public  List<LeaderLine> leaderLines; //UI窗口列表


        private void Update()
        {
            UpdateWindowPositions();
        }

        private float timeInterval = 1f / 20f;
        private float time = 1f/ 20f;
        private Vector2[] originPosList;
        private void UpdateWindowPositions()
        {
            time += Time.deltaTime;
            if (time >= timeInterval)
            {
                time -= timeInterval;

                SortLeaderLine();

                if (originPosList == null)
                    originPosList = new Vector2[leaderLines.Count];
                for (int i = 0; i < leaderLines.Count; i++)
                    originPosList[i] = leaderLines[i].CalculateTargetScreenPoint() + new Vector2(0, 100f);

                SimulateRepulsiveForce();
            }
            for (int i = 0; i < leaderLines.Count; i++)
            {
                leaderLines[i].SetAnchoredPos(Vector2.Lerp(leaderLines[i].GetAnchoredPos(), 
                    originPosList[i], 
                    Time.deltaTime * 16f));
            }
        }

        private void SimulateRepulsiveForce()
        {
            Vector2 offsetWeight = new Vector2(260f, 30f).normalized;
            //简单力导向布局
            Vector2 force;
            Vector2 dir;

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < leaderLines.Count; j++)
                {
                    if (leaderLines[j].isHide)
                        continue;
                    force = Vector2.zero;
                    for (int k = 0; k < leaderLines.Count; k++)
                    {
                        if (leaderLines[k].isHide)
                            continue;
                        if (j != k)
                        {
                            dir = leaderLines[j].GetAnchoredPos() - leaderLines[k].GetAnchoredPos();
                            float distance = dir.magnitude;
                            if (distance < 780f)
                            {
                                force += dir.normalized / distance * 20f * offsetWeight;
                            }
                        }
                    }
                    originPosList[j] += force;
                }
            }
        }


        private void SortLeaderLine()
        {
            leaderLines.Sort((a, b) =>
            {
                float aDistance = Vector3.Distance(a.target.position, Camera.main.transform.position);
                float bDistance = Vector3.Distance(b.target.position, Camera.main.transform.position);
                return bDistance.CompareTo(aDistance);
            });

            for (int i = 0; i < leaderLines.Count; i++)
            {
                leaderLines[i].canvas.sortingOrder = i;
            }
        }



#if UNITY_EDITOR
        [Button]
        public void GenerateLeaderLine()
        {
            for(int i = leaderLines.Count - 1;i >= 0;i--)
                DestroyImmediate(leaderLines[i].gameObject);

            leaderLines.Clear();

            for (int i = 0; i < splitModel.Nodes.Length; i++)
            {
                LeaderLine leaderLine = Instantiate(leaderLinePrefab);
                leaderLine.transform.SetParent(transform);
                leaderLine.target = splitModel.Nodes[i];
                leaderLine.canvas.worldCamera = uiCamera;
                leaderLine.canvas.sortingOrder = i;
                leaderLine.UpdateNameTextPosition();
            
                leaderLines.Add(leaderLine);
            }
        }
#endif
    }
}

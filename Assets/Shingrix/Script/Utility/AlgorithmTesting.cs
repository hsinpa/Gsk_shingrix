using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hsinpa.Ranking;
using Hsinpa.Algorithm;

public class AlgorithmTesting : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            int count = 5;
            List<TypeStruct.RankStruct> structs = GenQuickSortData(count).ToList();
            structs = structs.OrderByDescending(x => x.Value).ToList();
            //structs = QuickSort.Sort(structs, 0, count - 1);
            DebugLog(structs);
        }

        public static List<TypeStruct.RankStruct> GenQuickSortData(int count)
        {
            List<TypeStruct.RankStruct> rankStructs = new List<TypeStruct.RankStruct>();

            for (int i = 0; i < count; i++)
            {
                var g_rankStruct = new TypeStruct.RankStruct();
                g_rankStruct.SetIndex(i);
                g_rankStruct.SetValue(Random.Range(0, 100));

                g_rankStruct.name = "Test name " + i;

                rankStructs.Add(g_rankStruct);
            }

            return rankStructs;
        }

        private void DebugLog(List<TypeStruct.RankStruct> rankStructs) {
            int s_count = rankStructs.Count;

            for (int i = 0; i < s_count; i++)
            {
                Debug.Log($"Struct Index {rankStructs[i].Index}, Value {rankStructs[i].Value}");
            }
        }
    }

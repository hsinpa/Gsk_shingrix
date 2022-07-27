using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.Ranking {
    public class RankingView : MonoBehaviour
    {
        [SerializeField]
        private RankingItemView rankingItemViewPrefab;

        private RectTransform _rectTran;

        private List<RankingItemView> items = new List<RankingItemView>();
        private const int max_ranking_view = 10;

        public void PrepareItemView() {
            _rectTran = GetComponent<RectTransform>();

            PrepareMaxRankView(count: max_ranking_view);
        }

        public void SetRankingData(List<TypeStruct.RankStruct> sortedStructs) {

            int s_count = sortedStructs.Count; 

            for (int i = 0; i < max_ranking_view; i++) {

                bool isViewValid = i < s_count;

                items[i].gameObject.SetActive(isViewValid);

                if (!isViewValid)
                    continue;

                sortedStructs[i].SetIndex(i);
                items[i].SetData(sortedStructs[i]);
            }
        }

        //public void Update()
        //{
        //    if (items == null) return;

        //    int item_count = items.Count;

        //    for (int i = 0; i < item_count; i++)
        //    {
        //        items[i].OnUpdate();
        //    }
        //}


        private void PrepareMaxRankView(int count) {
            Utility.UtilityFunc.ClearChildObject(_rectTran);

            for (int i = 0; i < count; i++)
            {
                var itemView = GameObject.Instantiate<RankingItemView>(rankingItemViewPrefab, _rectTran);

                itemView.gameObject.SetActive(false);

                items.Add(itemView);
            }
        }

        public static Color GetColorByIndex(int index) {
            switch (index) {
                case 0:
                    return Color.yellow;
                case 1:
                    return Color.green;
                case 2:
                    return Color.blue;
                default:
                    return Color.gray;
            }
        }



    }
}

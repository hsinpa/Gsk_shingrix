using Hsinpa.Ranking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hsinpa.UI
{
    public class RankPageView : MonoBehaviour
    {
        [SerializeField]
        private RankingItemView rank_1;

        [SerializeField]
        private RankingItemView rank_2;

        [SerializeField]
        private RankingItemView rank_3;

        [SerializeField]
        private Button restart_btn;

        public void SetRestartAction(System.Action action)
        {
            Hsinpa.Utility.UtilityFunc.SetSimpleBtnEvent(restart_btn, action);
        }

        public void SetRanking(List<TypeStruct.RankStruct> rank_list) {
            if (rank_list == null) return;

            if (rank_list.Count > 0) {
                rank_1.SetName(rank_list[0].name);
                rank_1.SetScore(rank_list[0].Value.ToString());
            }

            if (rank_list.Count > 1)
            {
                rank_2.SetName(rank_list[1].name);
                rank_2.SetScore(rank_list[1].Value.ToString());
            }

            if (rank_list.Count > 2)
            {
                rank_3.SetName(rank_list[2].name);
                rank_3.SetScore(rank_list[2].Value.ToString());
            }
        }

    }
}
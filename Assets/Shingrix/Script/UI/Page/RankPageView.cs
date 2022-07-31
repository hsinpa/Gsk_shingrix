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

            try
            {
                rank_1.gameObject.SetActive(false);
                rank_2.gameObject.SetActive(false);
                rank_3.gameObject.SetActive(false);

                TypeStruct.RankStruct fallbackStruct = default(TypeStruct.RankStruct);

                AssignRanking(rank_1, (rank_list.Count > 0) ? rank_list[0] : fallbackStruct);
                AssignRanking(rank_2, (rank_list.Count > 1) ? rank_list[1] : fallbackStruct);
                AssignRanking(rank_3, (rank_list.Count > 2) ? rank_list[2] : fallbackStruct);
            }
            catch (System.Exception e) {
                Debug.Log(e.Message);
            }
        }

        private void AssignRanking(RankingItemView rankingItemView, TypeStruct.RankStruct rankStruct) {
            rankingItemView.gameObject.SetActive(rankStruct.id != null);
            if (rankStruct.id == null) {
                return;
            }

            rankingItemView.SetName(rankStruct.name);
            rankingItemView.SetScore(rankStruct.Value.ToString());
        }

    }
}
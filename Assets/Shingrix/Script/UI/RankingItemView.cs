using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Hsinpa.Ranking
{
    public class RankingItemView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI title;

        [SerializeField]
        private TextMeshProUGUI score;

        [SerializeField]
        private GameObject score_object;

        [SerializeField]
        private Image background;

        [SerializeField]
        private RectTransform rectTransform;

        [SerializeField]
        private LayoutElement layoutElement;
        private float defaultHeight;
        private float defaultWidth;

        private Color _targetColor;
        private float _targetWidth;
        private float _targetHeight;
        private Vector2 _targetPosition = new Vector2();
        private Vector2 _cacheSize = new Vector2();

        private Ranking.TypeStruct.RankStruct _rankStruct;


        public void SetName(string name) {
            title.text = name;
        }


        public void SetScore(string value)
        {
            score.text = value;
        }

        public void SetData(Ranking.TypeStruct.RankStruct rankStruct) {

            if (defaultHeight == 0)
                defaultHeight = layoutElement.preferredHeight;

            if (defaultWidth == 0)
                defaultWidth = layoutElement.preferredWidth;


            this._rankStruct = rankStruct;

            Color color = RankingView.GetColorByIndex(this._rankStruct.Index);

            background.color = color;

            SetName(rankStruct.name);
            SetScore(rankStruct.Value.ToString());

            if (rankStruct.Index >= 3) {
                layoutElement.preferredHeight = defaultHeight * 0.95f;
                layoutElement.preferredWidth = defaultWidth * 0.85f;
            }
        }
    }
}
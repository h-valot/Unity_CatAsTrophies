using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Console
{
    public class Console : MonoBehaviour
    {
        [Header("REFERENCES")]
        public RSE_DebugLog rseDebugLog;
        public Transform graphicsParent;
        public TextMeshProUGUI messageTM;

        [Header("TWEAKING")]
        public float duration;
        public float offset;
        
        private Vector3 _basePos;
        
        private void OnEnable()
        {
            _basePos = graphicsParent.localPosition;
            rseDebugLog.action += PrintMessage;
        }

        private void OnDisable()
        {
            rseDebugLog.action -= PrintMessage;
        }

        private void PrintMessage(string message, Color color) => AnimateMessage(message, color);

        private async Task AnimateMessage(string message, Color color)
        {
            graphicsParent.DOLocalMoveY(_basePos.y, 0);
            messageTM.DOColor(new Color(color.r, color.g, color.b, 1), 0);
            
            messageTM.color = color;
            messageTM.text = message;

            graphicsParent.DOLocalMoveY(_basePos.y + offset, duration);
            messageTM.DOColor(new Color(color.r, color.g, color.b, 0), duration).SetEase(Ease.InCubic);
        }
    }
}
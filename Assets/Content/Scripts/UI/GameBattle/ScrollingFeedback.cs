using UnityEngine;

namespace UI.GameBattle
{
    public class ScrollingFeedback : MonoBehaviour
    {
        [Header("REFERENCES")] 
        public BattlePawn battlePawn;
        public ScrollingFeedbackElement scrollingFeedbackElementPrefab;
    
        private Entity _entityRef;
    
        public void OnEnable() => battlePawn.OnEntityUpdated += UpdateEntityRef;
        public void OnDisable() => battlePawn.OnEntityUpdated -= UpdateEntityRef;

        private void UpdateEntityRef()
        {
            if (battlePawn.entityIdLinked == "")
            {
                _entityRef.onStatusReceived -= CreateScrollingFeedbackElement;
                _entityRef = null;
            }
            else if(_entityRef == null)
            {
                _entityRef = Misc.IdManager.GetEntityById(battlePawn.entityIdLinked);
                _entityRef.onStatusReceived += CreateScrollingFeedbackElement;
            }
            else
            {
                _entityRef.onStatusReceived -= CreateScrollingFeedbackElement;
                _entityRef = Misc.IdManager.GetEntityById(battlePawn.entityIdLinked);
                _entityRef.onStatusReceived += CreateScrollingFeedbackElement;
            }
        }

        private void CreateScrollingFeedbackElement (string text, Color textColor, bool isEffect)
        {
            float startPositionX = UnityEngine.Random.Range(-0.4f, 0.4f);
            float startPositionY = UnityEngine.Random.Range(-0.3f, 0.5f);
            float horizontalVelocity = UnityEngine.Random.Range(0.2f, 1.0f) * (UnityEngine.Random.Range(0, 2) * 2 - 1); //random between -1 and -0.2 or 0.2 and 1

            float fontSize = isEffect ? Registry.gameSettings.effectFontSize : Registry.gameSettings.defaultFontSize;

            var newEffectDisplay = Instantiate(scrollingFeedbackElementPrefab, gameObject.transform);
            newEffectDisplay.Initialize(text, startPositionX, startPositionY, horizontalVelocity, textColor, fontSize);
        }
    }
}
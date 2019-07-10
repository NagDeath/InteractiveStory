using UnityEngine;
using System.Linq;

public class CommonCharacterScript : MonoBehaviour, ICharacter
{
    [SerializeField]
    private UILabel phraseLabel;
    [SerializeField]
    private UISprite faceSprite, emotionCircleSprite;
    [SerializeField]
    private TweenPosition characterTP;

    private void OnEnable()
    {
        MainLogic.setupCharDelegate += SetupCharacter;
        MainLogic.destroyCharDelegate += DestroyCharacter;
        MainLogic.playTPDelegate += characterTP.PlayForward;
    }

    private void OnDisable()
    {
        MainLogic.setupCharDelegate -= SetupCharacter;
        MainLogic.destroyCharDelegate -= DestroyCharacter;
        MainLogic.playTPDelegate -= characterTP.PlayForward;
    }

    public void SetupCharacter(Passage block)
    {
        var index = block.text.IndexOf("[[");
        var phraseTextSubstring = block.text.Substring(0, index);
        phraseLabel.text = phraseTextSubstring;

        var emotion = block.tags.FirstOrDefault(t => t.Contains("Emotion_")).Replace("Emotion_", "");

        faceSprite.spriteName = "Face_" + emotion;
        emotionCircleSprite.spriteName = "EmotionCircle_" + emotion;
    }

    public void DestroyCharacter()
    {
        Destroy(gameObject);
    }
}

using UnityEngine;
using System.Linq;

public class NarratorScript : MonoBehaviour, ICharacter
{
    [SerializeField]
    private UILabel phraseLabel;
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
    }

    public void DestroyCharacter()
    {
        Destroy(gameObject);
    }
}

using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using System;

public class MainLogic : MonoBehaviour
{
    [SerializeField]
    private UIGrid choicesGreed;
    [SerializeField]
    private TweenColor gameOverTC;
    [SerializeField]
    private AudioSource musicPlayer;
    [SerializeField]
    private UITexture background;

    private List<Passage> blocks = new List<Passage>();
    private List<string[]> buttons = new List<string[]>();

    private string currentCharName;

    public GameObject spawnpoint;

    public static Action<Passage> setupCharDelegate;
    public static Action destroyCharDelegate;
    public static Action playTPDelegate;

    private void Start()
    {
        blocks = JsonConvert.DeserializeObject<RootObject>(LauncherScript.Json.ToString()).passages;
        SceneSettings(blocks[0]);
    }

    private void SceneSettings(Passage currentBlock) //Setup scene on each stage
    {
        //Character spawn
        var charName = currentBlock.tags.FirstOrDefault(t => t.Contains("CharName_")).Replace("CharName_", "");
        currentCharName = charName;
        spawnpoint.AddChild(LauncherScript.Characters.FirstOrDefault(c => c.name.Contains(charName)));
        playTPDelegate?.Invoke();
        setupCharDelegate?.Invoke(currentBlock);

        SetupBackground(currentBlock);
        SetupMusic(currentBlock);
        SetupChoicesGrid(currentBlock);
    }

    public void ChoiceButtonAction(string buttonText) //Begin next stage when choice button is clicked
    {
        var nextBlock = blocks.FirstOrDefault(b => b.name == buttons.FirstOrDefault(btn => btn[0] == buttonText)[1]);
        if (nextBlock.links != null)
        {
            if (currentCharName == nextBlock.tags.FirstOrDefault(t => t.Contains("CharName_")).Replace("CharName_", ""))
            {
                setupCharDelegate?.Invoke(nextBlock);
                SetupChoicesGrid(nextBlock);
            }
            else
            {
                destroyCharDelegate?.Invoke();
                SceneSettings(nextBlock);
            }
        }
        else
            GameOver();
    }

    private void SetupChoicesGrid(Passage currentBlock) //Choice buttons setup and activation
    {
        var choiceButtons = choicesGreed.GetChildList();
        choiceButtons.ForEach(b => b.gameObject.SetActive(false));
        buttons.Clear();
        for (int i = 0; i < currentBlock.links.Count; i++)
        {
            choiceButtons[i].gameObject.SetActive(true);
            string[] buttonNames = null;
            if (currentBlock.links[i].name.Contains("|"))
                buttonNames = currentBlock.links[i].name.Split('|');
            else
                buttonNames = new string[] { currentBlock.links[i].name, currentBlock.links[i].name };
            choiceButtons[i].GetComponentInChildren<UILabel>().text = buttonNames[0];
            buttons.Add(buttonNames);
        }
    }

    private void GameOver() //When game ends delete character, disable buttons and set black screen
    {
        destroyCharDelegate?.Invoke();
        choicesGreed.GetChildList().ForEach(b => b.gameObject.SetActive(false));
        gameOverTC.PlayForward();
    }

    private void SetupMusic(Passage currentBlock) //Find and play right music
    {
        var rightMusic = currentBlock.tags.FirstOrDefault(t => t.Contains("Sound_")).Replace("Sound_", "");
        if (musicPlayer.clip.name != rightMusic)
        {
            musicPlayer.clip = LauncherScript.Sounds.FirstOrDefault(m => m.name.Contains(rightMusic));
            musicPlayer.loop = true;
            musicPlayer.Play();
        }
    }

    private void SetupBackground(Passage currentBlock) //Find and set right background
    {
        var rightBackgroundName = currentBlock.tags.FirstOrDefault(t => t.Contains("BG_")).Replace("BG_", "");
        print(LauncherScript.Backgrounds.Count);
        background.mainTexture = LauncherScript.Backgrounds.FirstOrDefault(b => b.name == rightBackgroundName);
    }
}

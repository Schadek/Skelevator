using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum Skulls
{
    None,
    Jacques,
    Dominique
}

public class GameInstance : MonoBehaviour {

    //This class holds information about all the important components in the scene
    [System.Serializable]
    public struct UI
    {
    public CanvasGroup inventory;
    public CanvasGroup dialogue;
    public RectTransform dialogueRect;
    public Image backgroundImage;
    public Image faceset;
    public Text dialogueBox;
    public Image fadeTexture;
    }

    [System.Serializable]
    public struct Player
    {
        public Transform human;
        public Transform dog;
        public Transform invokingPosition;
        public Transform invokingPositionDog;
    }

    public UI UI_Components;
    public Player Player_Components;
    [Space(15)]
    public bool debugMode;

    private static GameInstance instance;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }

    public static GameInstance GetInstance()
    {
        return instance;
    }
}

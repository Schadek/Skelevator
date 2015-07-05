using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//NOTE: This is a singleton. Be aware of that if you use it
public class MessageSystem : MonoBehaviour
{
    public GameInstance gameInstance;

    private static MessageSystem instance;
    private GameInstance.UI uiComponents;

    private float timeBetweenCharacters = 0.01f;
    private float timeBeforeWindowClose = 1f;

    private int actualTextLength;
    private int characterPointer;
    private int tempCommandLength;

    private Vector2 originalRectPosition;

    void Awake()
    {
        instance = this;
        uiComponents = gameInstance.UI_Components;
        originalRectPosition = uiComponents.dialogueRect.anchoredPosition;
        uiComponents.dialogue.alpha = 0;
    }

    public static MessageSystem GetInstance()
    {
        return instance;
    }

    /************************************************************/

    public void StartDialogue(string content)
    {
        StopAllCoroutines();
        StartCoroutine(fadeIn());
        StartCoroutine(textBlending(content));
    }

    private IEnumerator fadeIn()
    {
        gameInstance.UI_Components.dialogue.interactable = true;
        gameInstance.UI_Components.dialogue.blocksRaycasts = true;

        while (gameInstance.UI_Components.dialogue.alpha < 1)
        {
            gameInstance.UI_Components.dialogue.alpha += Time.deltaTime;

            if (uiComponents.dialogue.alpha > 1)
            {
                uiComponents.dialogue.alpha = 1;
            }
            yield return null;
        }
    }

    private IEnumerator fadeOut()
    {
        uiComponents.dialogue.interactable = false;
        uiComponents.dialogue.blocksRaycasts = false;

        while (uiComponents.dialogue.alpha > 0)
        {
            uiComponents.dialogue.alpha -= Time.deltaTime;

            if (uiComponents.dialogue.alpha < 0)
            {
                uiComponents.dialogue.alpha = 0;
            }

            yield return null;
        }
    }

    private IEnumerator textBlending(string text)
    {
        characterPointer = 0;
        //The actual text length is the text length - length of inline commands
        actualTextLength = text.Length;

        uiComponents.dialogueBox.text = "";

        while (uiComponents.dialogueBox.text.Length != actualTextLength)
        {
            if (text[characterPointer] != '\\')
            {
                uiComponents.dialogueBox.text += text[characterPointer];
                characterPointer++;
                yield return new WaitForSeconds(timeBetweenCharacters);
            }
            else
            {
                yield return StartCoroutine(ExecInlineCommand(text, characterPointer));
                characterPointer += tempCommandLength;
                actualTextLength -= tempCommandLength;
                tempCommandLength = 0;
                yield return null;
            }
        }
        yield return new WaitForSeconds(timeBeforeWindowClose);
        StartCoroutine(fadeOut());
        uiComponents.dialogueBox.text = "";
    }

    private IEnumerator ExecInlineCommand(string text, int index)
    {
        string excerpt = "";
        index++;

        while (text[index] != ';' && text[index] != '[')
        {
            excerpt += text[index];
            index++;
        }

        //Substitute the '\' character
        tempCommandLength = 1;
        switch (excerpt)
        {
            case "w":
                tempCommandLength += 1;
                yield return new WaitForSeconds(RetrieveValueAsFloat(text, index));
                break;
            case "wt":
                //Compensate the semicolon
                tempCommandLength += 3;
                uiComponents.dialogueRect.anchoredPosition = (originalRectPosition * -1) + new Vector2(0, 100);
                break;
            case "wm":
                tempCommandLength += 3;
                //Compensate the semicolon
                uiComponents.dialogueRect.anchoredPosition = new Vector2(0, 50);
                break;
            case "wb":
                tempCommandLength += 3;
                //Compensate the semicolon
                uiComponents.dialogueRect.anchoredPosition = originalRectPosition;
                break;
            case "sh":
                break;
            case "f":
                tempCommandLength += 1;
                uiComponents.faceset.sprite = Resources.Load<Sprite>("Facesets/" + RetrieveValueAsString(text, index));
                break;
            case "sp":
                tempCommandLength += 2;
                timeBetweenCharacters = RetrieveValueAsFloat(text, index);
                break;
            case "bi":
                tempCommandLength += 2;
                string value = RetrieveValueAsString(text, index);
                if (value != "null")
                    uiComponents.backgroundImage.sprite = Resources.Load<Sprite>("Facesets/" + RetrieveValueAsString(text, index));
                else
                    uiComponents.backgroundImage.sprite = null;
                break;
            case "bc":
                tempCommandLength += 2;
                uiComponents.backgroundImage.color = RetrieveValueAsColor(text, index);
                break;
            case "bt":
                tempCommandLength += 2;
                Color colorBT = uiComponents.backgroundImage.color;
                colorBT.a = RetrieveValueAsFloat(text, index);
                uiComponents.backgroundImage.color = colorBT;
                break;
            case "cl":
                tempCommandLength += 2;
                timeBeforeWindowClose = RetrieveValueAsFloat(text, index);
                break;
            case "sfx":
                break;
            case "tt":
                tempCommandLength += 2;
                Color colorTT = uiComponents.dialogueBox.color;
                colorTT.a = RetrieveValueAsFloat(text, index);
                uiComponents.dialogueBox.color = colorTT;
                break;
        }
    }

    private float RetrieveValueAsFloat(string text, int index)
    {
        string value = text.Remove(0, index + 1);
        int iterator = 0;
        float valueAsFloat = 0f;

        while (value[iterator] != ']')
        {
            iterator++;
        }

        value = value.Remove(iterator);
        valueAsFloat = System.Convert.ToSingle(value);

        //Compensate the two square brackets + lenght of value
        tempCommandLength += value.Length + 2;
        return valueAsFloat;
    }

    private string RetrieveValueAsString(string text, int index)
    {
        string value = text.Remove(0, index + 1);
        int iterator = 0;

        while (value[iterator] != ']')
        {
            iterator++;
        }

        value = value.Remove(iterator);
        //Compensate the two square brackets + lenght of value
        tempCommandLength += value.Length + 2;
        return value;
    }

    private Color RetrieveValueAsColor(string text, int index)
    {
        string value = text.Remove(0, index + 1);
        int iterator = 0;
        Color newColor;

        while (value[iterator] != ']')
        {
            iterator++;
        }

        value = value.Remove(iterator);
        newColor = value.ParseToColor(); 

        //Compensate the two square brackets + lenght of value
        tempCommandLength += value.Length + 2;
        return newColor;
    }
}

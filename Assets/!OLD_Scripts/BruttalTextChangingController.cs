using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BruttalTextChangingController : MonoBehaviour
{
    public List<Transform> texts;

    public Transform pobieracz;
    public Transform oddawacz;
    public Transform windmill;

    public float opacityChangeSpeed;

    private bool needToChange;
    private int activeText;
    private string controller;

    private void Start()
    {
        activeText = 0;
        needToChange = false;
        controller = getControllerType();
        texts.ForEach(text => text.gameObject.SetActive(false));
        texts[activeText].gameObject.SetActive(true);
        var textController = texts[activeText].Find(controller);
        if (textController)
        {
            textController.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        controller = getControllerType();
        if (isPobieraczFullOfEnergy() && !didPobieraczGaveOddawaczEnergyAndCooookies() && !didWindmillGoBruuuum() && activeText == 0) // first change
        {
            needToChange = true;
        }

        if (!isPobieraczFullOfEnergy() && didPobieraczGaveOddawaczEnergyAndCooookies() && !didWindmillGoBruuuum() && activeText == 1) // second change
        {
            needToChange = true;
        }

        if (!isPobieraczFullOfEnergy() && !didPobieraczGaveOddawaczEnergyAndCooookies() && didWindmillGoBruuuum() && activeText == 2) // third change
        {
            needToChange = true;
        }

        if (needToChange)
        {
            if(texts[activeText].GetComponent<Text>().color.a != 0) // didnt dissapear
            {
                Color previous = texts[activeText].GetComponent<Text>().color;
                texts[activeText].GetComponent<Text>().color = new Color(previous.r, previous.g, previous.b, previous.a - opacityChangeSpeed < 0 ? 0 : previous.a - opacityChangeSpeed);

                // This is NOT a proper fucking way of doing this in any way, and is basically a one huge fucking hack to stop the gor forsaken NullReferenceExceptions. I will lose my mind if I see one more code like this ~fmazurek
                var textController = texts[activeText].Find(controller);
                if (textController)
                {
                    previous = textController.GetComponent<Text>().color;
                    textController.GetComponent<Text>().color = new Color(previous.r, previous.g, previous.b, previous.a - opacityChangeSpeed < 0 ? 0 : previous.a - opacityChangeSpeed);
                }
            } 
            else // dissapear
            {
                needToChange = false;
                texts[activeText].gameObject.SetActive(false);
                activeText++;
                if(activeText < texts.Count)
                {
                    texts[activeText].gameObject.SetActive(true);
                    var textController = texts[activeText].Find(controller);
                    if (textController)
                    {
                        textController.gameObject.SetActive(true);
                    }

                    Color previous = texts[activeText].GetComponent<Text>().color;
                    texts[activeText].GetComponent<Text>().color = new Color(previous.r, previous.g, previous.b, 0);

                    if (textController)
                    {
                        previous = textController.GetComponent<Text>().color;
                        textController.GetComponent<Text>().color = new Color(previous.r, previous.g, previous.b, 0);
                    }
                }
            }
        }
        else if(activeText < texts.Count)
        {
            if (texts[activeText].GetComponent<Text>().color.a != 1) // didnt show up with full glory
            {
                Color previous = texts[activeText].GetComponent<Text>().color;
                texts[activeText].GetComponent<Text>().color = new Color(previous.r, previous.g, previous.b, previous.a + opacityChangeSpeed > 1 ? 1 : previous.a + opacityChangeSpeed);

                // I assure you I will lose my mind if I find one more "texts[activeText].Find(controller)" copied around
                var textController = texts[activeText].Find(controller);
                if (textController)
                {
                    previous = textController.GetComponent<Text>().color;
                    textController.GetComponent<Text>().color = new Color(previous.r, previous.g, previous.b, previous.a + opacityChangeSpeed > 1 ? 1 : previous.a + opacityChangeSpeed);
                }
            }
        }
    }

    private bool isPobieraczFullOfEnergy()
    {
        return pobieracz.GetComponent<Energabler>().energy_units == 2;
    }

    private bool didPobieraczGaveOddawaczEnergyAndCooookies()
    {
        return oddawacz.GetComponent<Energabler>().energy_units == 2;
    }

    private bool didWindmillGoBruuuum()
    {
        return windmill.GetComponent<Energabler>().energy_units == 2;
    }

    private string getControllerType()
    {
        string[] joystickNames = Input.GetJoystickNames();
        foreach (string joystickName in joystickNames)
        {
            if (joystickName.ToLower().Contains("xbox"))
            {
                return "xbox";
            }
            else if (joystickName.ToLower().Contains("playstation"))
            {
                return "ps";
            }
            else
            {
                return "pc";
            }
        }
        return "pc";
    }
}

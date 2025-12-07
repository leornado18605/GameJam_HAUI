using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : Singleton<TextManager>
{
    public Text text;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            text.text = "";
            gameObject.SetActive(false);
            if (PlayerAttackController.Instance.win1 && PlayerAttackController.Instance.win2 &&
                PlayerAttackController.Instance.win3)
            {
                PlayerAttackController.Instance.gameObject.GetComponent<PlayerController>().EndGame();
            }
        }
    }
}

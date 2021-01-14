using UnityEngine;
using System.Collections;
using TMPro;

public class TextPoint : MonoBehaviour
{
    private Player player;  // the player script instance
    private GameFlowManager gameFlowManager;
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        // get the player gameObject from the game flow manager
        player = GameObject.Find("GameManager").GetComponent<GameFlowManager>().getPlayer().GetComponent<Player>();
        gameFlowManager = GameObject.Find("GameManager").GetComponent<GameFlowManager>();
        text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Detect whether the player is reaching this teleport point
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (gameObject.name == "FireTextPoint")
                text.text = "Press Space to fire gun";
            else if (gameObject.name == "JumpTextPoint")
                text.text = "Press W to jump";
            else if (gameObject.name == "DashTextPoint")
                text.text = "Press F to dash, evading attack";
            else if (gameObject.name == "AirSkillText")
                text.text = "Press Q in the air to use air skill";
            else if (gameObject.name == "LandSkillText")
                text.text = "Press E on the ground to use ground skill";
            text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            text.gameObject.SetActive(false);
        }
    }

}

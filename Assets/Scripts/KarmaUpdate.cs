using TMPro;
using UnityEngine;

public class KarmaUpdate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI karmaText;
    private int num = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //karmaText = GetComponent<TextMeshPro>();
        // Debug.Log(karmaText.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            num++;
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            num--;
        }
        karmaText.text = "KARMA: " + num.ToString();
        Debug.Log("Karma is now " + num);
    }
}

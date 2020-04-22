using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelWarpScript : MonoBehaviour
{
    public string newLevel;

    public Image warpImage;
    public Animator anim;
    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Fading());
        }
    }

    private IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => warpImage.color.a == 1);
        gm.ChangeLevel(newLevel);

    }

}

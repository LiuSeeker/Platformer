using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignScript : MonoBehaviour
{
    private GameObject texto;
    public GameObject textGO;
    public Canvas canvas;
    public string msg;

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
            texto = Instantiate(textGO, new Vector3(0,0,0), Quaternion.Euler(new Vector3(0, 0, 0)));
            texto.transform.SetParent(canvas.transform, false);
            texto.transform.GetComponent<Text>().text = msg;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(texto);
        }
    }


}

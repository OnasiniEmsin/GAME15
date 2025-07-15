using UnityEngine;
using System.Collections;

public class Animator : MonoBehaviour
{
    float a=1;
    int c=1;
    public bool isForWep;
    public byte typeOfAnimation=0;

    public GameObject rasm;

    // Start is called before the first frame update
    void Start()
    {
        switch(typeOfAnimation){
            case 0:StartCoroutine(aa());
                if(isForWep){
                    PlayerPrefs.SetInt("ekran",1);
                }else{
                    PlayerPrefs.SetInt("ekran",0);
                }break;
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator aa(){
        while(true){
            yield return new WaitForSeconds(.05f);
            
            if(a>1.5f){
                c=-1;
            }
            if(a<=1){
                c=1;
            }
            a+=c*.01f;
            transform.localScale = new Vector3(a, a, 2f);
        }
    }
}

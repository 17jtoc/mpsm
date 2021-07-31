using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHand : MonoBehaviour
{

    public Sprite sel1;
    public Sprite sel2;

    public Sprite weapon1;
    public Sprite trap1;
    public Sprite trap2;
    public Sprite trap3;
    public Sprite trap4;

    public Image trapicon;
    public Image weaponicon;

    public Image backgroundHand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setBack(int setTo)
    {
        if (setTo == 0)
        {
            backgroundHand.sprite = sel1;
        }
        else
        {
            backgroundHand.sprite = sel2;
        }
    }

    public void setTrap(int setTo)
    {
        if(setTo == 0)
        {
            trapicon.sprite = trap1;

        }
        else if (setTo == 1)
        {
            trapicon.sprite = trap2;

        }
        else if (setTo == 2)
        {
            trapicon.sprite = trap3;

        }
        else if (setTo == 3)
        {
            trapicon.sprite = trap4;

        }
        
    }

    public void setWeapon(int setTo)
    {

    }
}

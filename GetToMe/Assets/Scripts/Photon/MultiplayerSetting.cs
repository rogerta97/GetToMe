using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSetting : MonoBehaviour
{
    public int m_MaxPlayers;
    public bool delayStart; 

    public static MultiplayerSetting Instance; 

    private void Awake()
    {

        if (MultiplayerSetting.Instance == null)
        {
            MultiplayerSetting.Instance = this;
        }
        else
        {
            if (MultiplayerSetting.Instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

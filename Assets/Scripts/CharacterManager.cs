using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    public Player _player;
    public Player Player
    {
        get
        {
            return _player;
        }
        set
        {
            _player = value;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //기존에 있는 instance와 지금 넣으려고 하는 내 자신이 다르다면, 현재 것을 파괴해줘라.
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

    }
}



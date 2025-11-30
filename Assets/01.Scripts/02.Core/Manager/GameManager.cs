using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Player _player;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }

        _player = GameObject.FindFirstObjectByType<Player>();

        if (_player == null) ("플레이어를 찾을 수 없음").EditorLog();
    }

    private GameManager() { }

    #region 프로퍼티

    public Player Player { get { return _player; } }

    #endregion

}

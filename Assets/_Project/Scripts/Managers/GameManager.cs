using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

public class GameManager : Singleton<GameManager>
{
    #region Variables Game
    
    [FoldoutGroup("Variables Game", expanded: true)]
    public int InitialLives = 20;
    [FoldoutGroup("Variables Game")]
    [Sirenix.OdinInspector.ReadOnly, SerializeField] private int _currentLives;
    [FoldoutGroup("Variables Game")]
    [SerializeField] private int _currentHorde = 0;
    [SerializeField] private int _enemiesKilled = 0;
    private Spawner _spawner;

    public static Action<int> OnUpdateCoins;
    public static Action<int> OnUpdateEnemiesKilled;
    public static Action<int> OnUpdateCurrentHorde;
    public static Action<int> OnUpdateCurrentLives;
    public static Action OnFinish;
    [Serializable]
    public struct EnemySpawnByHorde
    {
        public TypePool type;
        public int HowMany;
        public int PerHorde;

        public int AmountOfEnemies(int currentHorde) => HowMany * (currentHorde / PerHorde);
    }

    public List<EnemySpawnByHorde> EnemySpawnByHordeList;

    #endregion

    #region Variables Player

    [FoldoutGroup("Variables Player", expanded: true)]
    public int Coins = 0;

    #endregion

    

    public static Action<Vector3> OnPositionCharacter;
    public XROrigin XROrigin;
    public RespawnData[] RespawnLocations = new RespawnData[4];

    [Sirenix.OdinInspector.ReadOnly, SerializeField] private int _currentEnemiesOnScene = 0;
    #region Unity Methods
    private void OnValidate()
    {
        Utils.ValidationUtility.SafeOnValidate(() =>
        {
            if (this == null) return;
            if (XROrigin == null) XROrigin = FindObjectOfType<XROrigin>();
            if (RespawnLocations.Length == 0) RespawnLocations = FindObjectsOfType<RespawnData>();
            if (_spawner == null) _spawner = FindObjectOfType<Spawner>();
        });
    }

    private void Start()
    {
        OnValidate(); //< To check cached variables

        TeleportationProviderCustom.OnTeleport += SendPositionXR;
    }

    private void OnDestroy()
    {
        TeleportationProviderCustom.OnTeleport -= SendPositionXR;
    }

    #endregion

    public void InitGame()
    {
        _currentLives = InitialLives;
        _currentHorde = 0;

        AddCoins(20);
        OnUpdateCurrentHorde?.Invoke(0);
        OnUpdateEnemiesKilled?.Invoke(0);
        OnUpdateCurrentLives?.Invoke(_currentLives);

        NewHorde();
    }

    public void NewHorde()
    {
        ++_currentHorde;
        OnUpdateCurrentHorde?.Invoke(_currentHorde);
        _ = SpawnHorde(_currentHorde);
    }

    async UniTask SpawnHorde(int currentHorde)
    {
        //await UniTask.Yield();
        int length = EnemySpawnByHordeList.Count;
        int lengthRespawn = RespawnLocations.Length;

        // To calculate how many enemies will be on scene at the same time
        for (int i = 0; i < length; i++)
        {
            int amount = EnemySpawnByHordeList[i].AmountOfEnemies(currentHorde);
            _currentEnemiesOnScene += amount;
        }

        for (int i = 0; i < length; i++)
        {
            int amount = EnemySpawnByHordeList[i].AmountOfEnemies(currentHorde);
            int currentRespawn = 0;
            TypePool typePool = EnemySpawnByHordeList[i].type;
            for (int j = 0; j < amount; j++)
            {
                currentRespawn = j % lengthRespawn;
                RespawnData respawn = RespawnLocations[currentRespawn];
                var enemy = _spawner.GetFromPool(typePool);
                enemy.SetActive(true);
                enemy.GetComponentInChildren<EnemyBehaviour>().Init(respawn.Target.position, respawn.transform);
                await UniTask.Delay(1000);   //< time in ms to spawn each enemy spawn
            }
        }
    }

    private void SendPositionXR()
    {
        OnPositionCharacter?.Invoke(XROrigin.transform.position);
    }

    public void TakeDamage()
    {
        --_currentLives;
        OnUpdateCurrentLives?.Invoke(_currentLives);
        // Update Feedback

        if (_currentLives > 0) return;
        else GameOver();
    }

    public void GameOver()
    {
        OnFinish?.Invoke();
        _currentHorde = 0;
    }

    public void AddCoins(int coins)
    {
        Coins += coins;
        OnUpdateCoins?.Invoke(Coins);
    }

    
    public void ReturnEnemyToPool(EnemyBehaviour enemy, bool isKilled = false)
    {
        --_currentEnemiesOnScene;
        if (isKilled)
        {
            ++_enemiesKilled;
            OnUpdateEnemiesKilled?.Invoke(_enemiesKilled);
        }
        _spawner.ReturnToPool(enemy.MyPool, enemy.transform.parent.gameObject);
        
        CheckEndHorde();
    }

    public void ReturnToPool(TypePool type,GameObject obj)
    {
        _spawner.ReturnToPool(type, obj);
    }

    public GameObject GetFromPool(TypePool type)
    {
        return _spawner.GetFromPool(type);
    }

    public void CheckEndHorde()
    {
        if (_currentEnemiesOnScene != 0) return;

        NewHorde();
    }
}

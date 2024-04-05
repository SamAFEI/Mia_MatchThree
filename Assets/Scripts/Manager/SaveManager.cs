using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.Manager
{
    public class SaveManager : MonoBehaviour
    {
        private static SaveManager instance;
        public static SaveManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindAnyObjectByType<SaveManager>();
                }
                if (instance == null)
                {
                    CreateDefault();
                }
                return instance;
            }
        }

        [SerializeField] private string fileName;
        private GameData gameData;
        private List<ISaveManager> saveManagers;
        private FileDataHandler dataHandler;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            Instance.dataHandler = new FileDataHandler(Application.persistentDataPath,fileName);
            Instance.saveManagers = FindAllSaveManagers();
            LoadGame();
        }

        private static void CreateDefault()
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/Manager/SaveManager");
            obj = Instantiate(obj, Vector3.zero, Quaternion.identity);
            instance = obj.GetComponent<SaveManager>();
        }
        public static void NewGame()
        {
            Instance.gameData = new GameData();
            Instance.gameData.Coin = 4000;
        }

        public static void LoadGame()
        {
            return;
            if (Instance.dataHandler == null) { return; }
            Instance.saveManagers = FindAllSaveManagers();
            Instance.gameData = Instance.dataHandler.Load();
            if (Instance.gameData == null)
            {
                NewGame();
            }
            foreach (ISaveManager _saveManager in Instance.saveManagers)
            {
                _saveManager.LoadData(Instance.gameData);
            }
        }
        public static void SaveGame()
        {
            return;
            if (Instance.dataHandler == null) { return; }
            Instance.saveManagers = FindAllSaveManagers();
            foreach (ISaveManager _saveManager in Instance.saveManagers) 
            {
                _saveManager.SaveData(ref Instance.gameData);
            }
            Instance.dataHandler.Save(Instance.gameData);
        }
        private void OnApplicationQuit()
        {
            SaveGame();
        }
        private static List<ISaveManager> FindAllSaveManagers()
        {
            IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
            return new List<ISaveManager>(saveManagers);
        }
    }
}

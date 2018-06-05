using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsoleController : UiPanel<DebugConsoleController>
{
    private string _initPath;
    [SerializeField] private bool _consoleVisible;
    [SerializeField] private bool _useInitDotRwc = true;
    [SerializeField] private InputField _inputField;

    private SpawnController _spawnController;
    private TownController _townController;

    private Queue<string> _commands;
    private float _timeToSleep;

    private void Awake()
    {
        _spawnController = SpawnController.Instance;
        _townController = TownController.Instance;
        _commands = new Queue<string>();

        _inputField.onEndEdit.AddListener(OnEndEdit);
        CheckShowConsole();

        _initPath = Path.Combine(Application.streamingAssetsPath, "init.rwc");
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.C)) return;
        _consoleVisible = !_consoleVisible;

        CheckShowConsole();
    }

    private void CheckShowConsole()
    {
        if (_consoleVisible)
        {
            ShowPanel();
        }
        else
        {
            HidePanel();
        }
    }

    private void Start()
    {
        StartCoroutine(InterpretCommands());
        var res = PlayerPrefs.GetInt(PlayerPrefsKeys.HasActiveGame.ToString());
        if (_useInitDotRwc && res == 0 || !TownController.Instance.DebugUseLoadedGame && _useInitDotRwc)
        {
            FileManager.Instance.ReadText(_initPath, EnqueueInitCommands);
        }

        StartCoroutine(InterpretCommands());
    }

    private void EnqueueInitCommands(string initCommands)
    {
        foreach (var line in initCommands.Split(Environment.NewLine.ToCharArray()))
        {
            _commands.Enqueue(line);
        }
    }

    private IEnumerator InterpretCommands()
    {
        while (true)
        {
            if (_commands.Count <= 0)
            {
                yield return null;
            }
            else
            {
                InterpretCommand(_commands.Dequeue());
                if (_timeToSleep > 0)
                {
                    yield return new WaitForSeconds(_timeToSleep);
                    _timeToSleep = 0;
                }

                yield return null;
            }
        }
    }


    private void InterpretCommand(string command)
    {
        if (string.IsNullOrEmpty(command))
        {
            return;
        }

        var parts = command.Split(' ').ToList();
        var head = GetCommandHead(parts);

        // spawn
        if (head.StartsWith("sp"))
        {
            InterpretSpawnCommand(parts);
            return;
        }

        // sleep
        if (head.StartsWith("sl"))
        {
            _timeToSleep = float.Parse(GetCommandHead(parts));
            return;
        }

        // order
        if (head.StartsWith("o"))
        {
            var secondPart = GetCommandHead(parts);
            if (secondPart.StartsWith("p"))
            {
                var pos = GetPos(parts);
                _townController.PushNewOrder(OrderType.PlantWheat, pos);
                return;
            }

            return;
        }

        // fastforward (ff)
        if (head.StartsWith("f"))
        {
            var secondPart = GetCommandHead(parts);
            var speed = int.Parse(secondPart);
            GameApplication.Instance.ChangeTimeSpeed(speed);
            return;
        }

        // save
        if (head.StartsWith("sav"))
        {
            LoadSaveGame.SaveGameData();
            return;
        }

        // reload
        if (head.StartsWith("rel"))
        {
            LoadSaveGame.Instance.ReloadGame();
            return;
        }

        // reset
        if (head.StartsWith("res"))
        {
            LoadSaveGame.Instance.ResetGame();
            return;
        }

        // erase
        if (head.StartsWith("er"))
        {
            LoadSaveGame.Instance.EraseGame();
            return;
        }

        // add
        if (head.StartsWith("add"))
        {
            var secondPart = GetCommandHead(parts);
            DropItemType type;
            // meat
            if (secondPart.StartsWith("m"))
            {
                type = DropItemType.Meat;
            }
            else if (secondPart.StartsWith("b"))
            {
                type = DropItemType.Bread;
            }
            else
            {
                Debug.LogError($"DropItemType {secondPart} not supported");
                return;
            }

            var ammountString = GetCommandHead(parts);
            var ammount = uint.Parse(ammountString);
            _townController.AddToStorage(type, ammount);
            return;
        }

        Debug.LogError("Unrecognized command " + command);
    }

    private void InterpretSpawnCommand(IList<string> parts)
    {
        var secondPart = GetCommandHead(parts);
        var pos = GetPos(parts);

        if (secondPart.StartsWith("v"))
        {
            _spawnController.SpawnVillager();
            return;
        }

        if (secondPart.StartsWith("wh"))
        {
            _spawnController.SpawnWheatCrop(pos);
            return;
        }

        if (secondPart.StartsWith("wa"))
        {
            _spawnController.SpawnBuilding(BuildingType.Warehouse, pos);
            return;
        }

        if (secondPart.StartsWith("h"))
        {
            _spawnController.SpawnBuilding(BuildingType.House, pos);
            return;
        }

        if (secondPart.StartsWith("wf"))
        {
            _spawnController.SpawnBuilding(BuildingType.WheatFarm, pos);
            return;
        }

        if (secondPart.StartsWith("tfa"))
        {
            _spawnController.SpawnBuilding(BuildingType.TofuFarm, pos);
            return;
        }

        if (secondPart.StartsWith("tfe"))
        {
            _spawnController.SpawnBuilding(BuildingType.TofuFermenter, pos);
            return;
        }

        if (secondPart.StartsWith("p"))
        {
            _spawnController.SpawnBuilding(BuildingType.Pen, pos);
            return;
        }

        if (secondPart.StartsWith("o"))
        {
            _spawnController.SpawnBuilding(BuildingType.Oven, pos);
            return;
        }

        if (secondPart.StartsWith("s"))
        {
            _spawnController.SpawnBuilding(BuildingType.Slaughterhouse, pos);
            return;
        }

        if (secondPart.StartsWith("d"))
        {
            _spawnController.SpawnBuilding(BuildingType.Dock, pos);
            return;
        }
    }

    private Vector2Int GetPos(IList<string> parts)
    {
        if (parts.Count < 2)
        {
            return Vector2Int.zero;
        }

        var cx = int.Parse(GetCommandHead(parts));
        var cy = int.Parse(GetCommandHead(parts));
        return new Vector2Int(cx, cy);
    }

    private string GetCommandHead(IList<string> parts)
    {
        var firstPart = parts.First();
        parts.RemoveAt(0);
        return firstPart;
    }

    private void OnEndEdit(string command)
    {
        _commands.Enqueue(command);
        _inputField.text = "";
    }
}
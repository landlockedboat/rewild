using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class MissionsManager : BitController<MissionsManager>
{
    private Queue<Mission> _missions = new Queue<Mission>();

    private string _missionsPath;

    [HideInInspector] public bool AreMissionsLoaded;

    private bool _reportedCompletion;
    public bool HasMissions => _missions.Count > 0;

    private void Start()
    {
        _missionsPath = Path.Combine(Application.streamingAssetsPath, "missions.json");
//        SaveSampleMissions();
        LoadMissions();
    }

    private void Update()
    {
        if (!AreMissionsLoaded || _reportedCompletion || !_missions.Peek().IsCompleted) return;

        TriggerCallback(NotificationType.OnMissionObjectivesReached);
        _reportedCompletion = true;
    }

    public Mission GetCurrentMission()
    {
        return HasMissions ? _missions.Peek() : null;
    }

    public void StartCurrentMission()
    {
        if (!HasMissions)
        {
            return;
        }

        _missions.Peek().Start();
        _reportedCompletion = false;

        TriggerCallback(NotificationType.OnMissionStarted);
    }

    public void CompleteCurrentMission()
    {
        if (!AreMissionsLoaded)
        {
            return;
        }

        if (!_missions.Peek().IsCompleted)
        {
            return;
        }

        Debug.Log($"Mission {_missions.Dequeue().Title} completed!");
        TriggerCallback(NotificationType.OnMissionCompleted);

        if (_missions.Count > 0)
        {
            TriggerCallback(NotificationType.OnMissionToStart);
            return;
        }

        Debug.Log("You completed all missions!");
        LevelConfiguration.Instance.TriggerVictory();
        AreMissionsLoaded = false;
    }

    private void LoadMissions()
    {
        FileManager.Instance.ReadText(_missionsPath, OnReadText);
    }

    private void OnReadText(string missionsText)
    {
        _missions = JsonConvert.DeserializeObject<Queue<Mission>>(missionsText,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

        Debug.Log($"Missions loaded from {_missionsPath}");
        AreMissionsLoaded = true;
        TriggerCallback(NotificationType.OnMissionsLoaded);
    }

    private void SaveSampleMissions()
    {
        _missions = new Queue<Mission>();

        _missions.Enqueue(
            new InfoMission
            {
                ObjectiveAmmount = 1,
                Text = new Dictionary<Language, string>
                {
                    {
                        Language.English,
                        "Welcome to ReWild! This is a game about resource management and strategy. You'll win once you" +
                        $"complete all missions, but keep in mind that if {TownModel.Instance.MaxVillagersLeaving} villagers" +
                        "get angry at you and leave the island, you'll lose the game! Understood? Let's begin!"
                    },
                    {
                        Language.Spanish,
                        "Bienvenido a ReWild! Esto es un juego sobre estrategia y gestion de recursos. Habras ganado una vez hayas " +
                        $"completado todas las misiones, pero ten en mente que si {TownModel.Instance.MaxVillagersLeaving} aldeanos " +
                        "se enfadan y abandonan la isla habras perdido!"
                    }
                },
                Title = new Dictionary<Language, string>
                {
                    {
                        Language.English,
                        "Welcome!"
                    },
                    {
                        Language.Spanish,
                        "Bienvenido!"
                    }
                }
            });

        _missions.Enqueue(
            new BuildMission
            {
                BuildingType = BuildingType.House,
                ObjectiveAmmount = 1,
                Text = new Dictionary<Language, string>
                {
                    {
                        Language.English,
                        "You need to build a house for your first villager to live in!"
                    },
                    {
                        Language.Spanish,
                        "Necesitas construir una casa para que tu primer aldeano pueda vivr en ella!"
                    }
                },
                Title = new Dictionary<Language, string>
                {
                    {
                        Language.English,
                        "Build a house"
                    },
                    {
                        Language.Spanish,
                        "Construye una casa"
                    }
                }
            });

        _missions.Enqueue(
            new InfoMission
            {
                ObjectiveAmmount = 1,
                Text = new Dictionary<Language, string>
                {
                    {
                        Language.English,
                        $"Well done building that house! {Environment.NewLine}" +
                        "To build more buildings, you'll have to wait a day to collect " +
                        $"the taxes that each villager pays. Each villager pays between {LevelConfiguration.Instance.MinMoneyPerVillager} " +
                        $"and {LevelConfiguration.Instance.MaxMoneyPerVillager} gold."
                    },
                    {
                        Language.Spanish,
                        $"Buen trabajo construyendo esa casa! {Environment.NewLine}" +
                        "Para construir mas, deberas esperarte un dia para recoger " +
                        $"los impuestos de cada aldeano. Cada aldeano paga entre {LevelConfiguration.Instance.MinMoneyPerVillager} " +
                        $"y {LevelConfiguration.Instance.MaxMoneyPerVillager} de oro."
                    }
                },
                Title = new Dictionary<Language, string>
                {
                    {
                        Language.English,
                        "Well done!"
                    },
                    {
                        Language.Spanish,
                        "Buen trabajo!"
                    }
                }
            });

        _missions.Enqueue(
            new BuildMission
            {
                BuildingType = BuildingType.Warehouse,
                ObjectiveAmmount = 1,
                Text = new Dictionary<Language, string>
                {
                    {
                        Language.English,
                        "Build a Warehouse to store the goods the villager will collect!"
                    },
                    {
                        Language.Spanish,
                        "Construye un almacen para que los aldeanos puedan guardar lo que produzcan!"
                    }
                },
                Title = new Dictionary<Language, string>
                {
                    {
                        Language.English,
                        "Build a Warehouse"
                    },
                    {
                        Language.Spanish,
                        "Construye un Almacen"
                    }
                }
            });

        _missions.Enqueue(
            new BuildMission
            {
                BuildingType = BuildingType.Oven,
                ObjectiveAmmount = 1,
                Text = new Dictionary<Language, string>
                {
                    {
                        Language.English,
                        "Well done! Now, build an oven to make bread with wheat."
                    },
                    {
                        Language.Spanish,
                        "Buen trabajo! Ahora, construye un horno para que los aldeanos puedan hornear pan."
                    }
                },
                Title = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Build an Oven"
                    },
                    {
                        Language.Spanish,
                        "Construye un horno"
                    }
                }
            });

        _missions.Enqueue(
            new BuildMission
            {
                BuildingType = BuildingType.WheatFarm,
                ObjectiveAmmount = 1,
                Text = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Build a wheat farm to make wheat!"
                    },
                    {
                        Language.Spanish,
                        "Construye una granja de trigo!"
                    }
                },
                Title = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Build a wheat farm"
                    },
                    {
                        Language.Spanish,
                        "Construye una granja de trigo"
                    }
                }
            });

        _missions.Enqueue(
            new DaysPassedMission
            {
                ObjectiveAmmount = 2,
                Text = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Let's see if you can survive for some time now..."
                    },
                    {
                        Language.Spanish, "Veamos si puedes sobrevivir durante unos dias ahora..."
                    }
                },
                Title = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Survive!"
                    },
                    {
                        Language.Spanish, "Sobrevive!"
                    }
                }
            });

        _missions.Enqueue(
            new DietChangeMission
            {
                BuildingsToUnlock = new[] {BuildingType.Slaughterhouse, BuildingType.Pen},
                NewDiet = Diet.MeatEater,
                ObjectiveAmmount = 1,
                Text = new Dictionary<Language, string>
                {
                    {
                        Language.English,
                        "Villagers now need to eat meat, you need to build another industry to make it happen!"
                    },
                    {
                        Language.Spanish,
                        "Los aldeanos necesitan comer carne ahora, necesitaras construir toda una industria para satisfacerlos!"
                    }
                },
                Title = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Villagers need meat!"
                    },
                    {
                        Language.Spanish, "Los aldeanos comen carne!"
                    }
                }
            });

        _missions.Enqueue(
            new BuildMission
            {
                BuildingType = BuildingType.Slaughterhouse,
                ObjectiveAmmount = 1,
                Text = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Build a slaughterhouse."
                    },
                    {
                        Language.Spanish, "Construye un matadero"
                    }
                },
                Title = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Build a slaughterhouse"
                    },
                    {
                        Language.Spanish, "Construye un matadero"
                    }
                }
            });

        _missions.Enqueue(
            new BuildMission
            {
                BuildingType = BuildingType.Pen,
                ObjectiveAmmount = 1,
                Text = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Build a pen to keep the pigs in there."
                    },
                    {
                        Language.Spanish, "Construye un corral."
                    }
                },
                Title = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Build a pig pen"
                    },
                    {
                        Language.Spanish, "Construye un corral"
                    }
                }
            });

        _missions.Enqueue(
            new DietChangeMission
            {
                BuildingsToLock = new[] {BuildingType.Slaughterhouse, BuildingType.Pen},
                BuildingsToUnlock = new[] {BuildingType.TofuFarm, BuildingType.TofuFermenter},
                NewDiet = Diet.Vegan,
                ObjectiveAmmount = 1,
                Text = new Dictionary<Language, string>
                {
                    {
                        Language.English,
                        "Villagers don't need to eat meat no more!, you can destroy all meat produciong buildings now!"
                    },
                    {
                        Language.Spanish,
                        "Los aldeanos ya no comen mas carne! Puedes destruir todas las construcciones de la industria de la carne!"
                    }
                },
                Title = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Villagers don't eat meat!"
                    },
                    {
                        Language.Spanish, "Los aldeanos no comen carne!"
                    }
                }
            });

        _missions.Enqueue(
            new BulldozeBuildingsMission
            {
                BuildingTypesToBulldoze = new[] {BuildingType.Slaughterhouse, BuildingType.Pen},
                Text = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Destroy all meat produciong buildings now!"
                    },
                    {
                        Language.Spanish, "Destruye las construcciones de la industria carnica!"
                    }
                },
                Title = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Destroy the meat industry!"
                    },
                    {
                        Language.Spanish, "Destruye la industria carnica!"
                    }
                }
            });

        _missions.Enqueue(
            new BuildMission
            {
                BuildingType = BuildingType.TofuFarm,
                ObjectiveAmmount = 1,
                Text = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Build a tofu farm to make tofu!"
                    },
                    {
                        Language.Spanish, "Construye una plantacion de tofu para tener soja!"
                    }
                },
                Title = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Build a tofu farm"
                    },
                    {
                        Language.Spanish, "Construye una plantacion de soja!"
                    }
                }
            });

        _missions.Enqueue(
            new BuildMission
            {
                BuildingType = BuildingType.TofuFermenter,
                ObjectiveAmmount = 1,
                Text = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Build a tofu fermenter to make tofu!"
                    },
                    {
                        Language.Spanish, "Construye un fermentador de tofu!"
                    }
                },
                Title = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Build a tofu fermenter"
                    },
                    {
                        Language.Spanish, "Construye un fermentador de tofu!"
                    }
                }
            });

        _missions.Enqueue(
            new DaysPassedMission
            {
                ObjectiveAmmount = 3,
                Text = new Dictionary<Language, string>
                {
                    {
                        Language.English, "If you survive those last days, you'll win!"
                    },
                    {
                        Language.Spanish, "Si sobrevives estos ultimos dias, habras ganado!"
                    }
                },
                Title = new Dictionary<Language, string>
                {
                    {
                        Language.English, "Win the game!"
                    },
                    {
                        Language.Spanish, "Gana el juego!"
                    }
                }
            });

        var text = JsonConvert.SerializeObject(_missions,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        File.WriteAllText(_missionsPath, text);

        print("Missions saved");
    }
}
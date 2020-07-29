using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static bool gameHasStarted = false;
    private static bool gameFinished = false;
    private static bool inEscapeMenu = false;
    private static float soundVolume = 50f;
    private static int roundSwings = 0;

    public static bool GameHasStarted { get => gameHasStarted; set => gameHasStarted = value; }
    public static bool GameFinished { get => gameFinished; set => gameFinished = value; }
    public static int RoundSwings { get => roundSwings; set => roundSwings = value; }
    public static bool InEscapeMenu { get => inEscapeMenu; set => inEscapeMenu = value; }
    public static float SoundVolume { get => soundVolume; set => soundVolume = value; }
}

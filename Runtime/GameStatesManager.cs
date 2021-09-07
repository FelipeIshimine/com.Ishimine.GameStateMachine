using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Managers/GameState")]
public class GameStatesManager : RuntimeScriptableSingleton<GameStatesManager>
{
#if UNITY_EDITOR
        [MenuItem("GameStateMachine/PrintUpperState")]
        public static void PrintUpperState() => Debug.Log(UpperState);
        [MenuItem("GameStateMachine/PrintStateChain")]
        public static void PrintStateChain()
        {
            string str = string.Empty;
            for (var index = ActiveStates.Count - 1; index >= 0; index--)
            {
                BaseGameState baseGameState = ActiveStates[index];
                str += $"{baseGameState}->";
            }
            Debug.Log(str);
        }
#endif
    public static IReadOnlyList<BaseGameState> ActiveStates => StatesStack.ToArray();
    [ShowInInspector]  public static readonly Stack<BaseGameState> StatesStack = new Stack<BaseGameState>();
    public static  BaseGameState UpperState => StatesStack == null || StatesStack.Count == 0?null:StatesStack.Peek();
    public bool autoInitialize = true;
    public static bool AutoInitialize => Instance.autoInitialize;

    [ShowInInspector] public static BaseGameState Current;

    public static void Push<T>(GameState<T> gameState) where T : GameState<T>
    {
        StatesStack.Push(gameState);
        Current = gameState;
    }
    
    public static BaseGameState Pop()
    {
        BaseGameState oldState = StatesStack.Pop();
        Current = UpperState;
        return oldState;
    }

}
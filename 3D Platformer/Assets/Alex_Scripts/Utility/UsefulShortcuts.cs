using System;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ShortcutManagement;
#endif
using UnityEngine;

#if UNITY_EDITOR
internal static class UsefulShortcuts
{
    /// <summary>
    /// Alt+ C to clear the console.
    /// </summary>
    [Shortcut("Clear Console", KeyCode.C, ShortcutModifiers.Alt)]
    public static void ClearConsole()
    {
        var assembly = Assembly.GetAssembly(typeof(SceneView));
        var type     = assembly.GetType("UnityEditor.LogEntries");
        var method   = type.GetMethod("Clear");
        method?.Invoke(new (), null);
    }
#endif

    /// <summary>
    /// A collection of debugging shortcuts.
    /// Includes keyboard shortcuts tied to the F-keys, as well as context menus.
    /// Note: These methods are local functions, and are only accessible within this method.
    /// </summary>

#if UNITY_EDITOR
    [Shortcut("Damage Player", KeyCode.F1), ContextMenu("Damage Player")]
    static void DamagePlayer()
    {
        // Damage the player by 10.
        //GameManager.Instance.Player.CurrentHealth -= 10;
        Debug.Log("Player damaged.");
    }

    [Shortcut("Heal Player", KeyCode.F2), ContextMenu("Heal Player")]
    static void HealPlayer()
    {
        // Heal the player by 10.
        //GameManager.Instance.Player.CurrentHealth += 10;
        Debug.Log("Player healed.");
    }

    [Shortcut("Kill Player", KeyCode.F3), ContextMenu("Kill Player")]
    static void KillPlayer()
    {
        // Kill the player.
        //GameManager.Instance.Player.CurrentHealth = 0;
        Debug.Log("Player killed.");
    }

    [Shortcut("Reload Scene", KeyCode.F5), ContextMenu("Reload Scene")]
    static void ReloadScene()
    {
        // Reload Scene
        SceneManagerExtended.ReloadScene();
        Debug.Log("Scene reloaded.");
    }
}
#endif

public static class UsefulMethods
{
    /// <summary>
    /// Allows you to call a method after a delay through the use of an asynchronous operation. </summary>
    /// <example> DelayTaskAsync(() => action(), delayInSeconds, debugLog, cancellationToken).AsTask(); </example>
    /// <remarks> To run a method after the task is completed: Task delayTask = delayTask.ContinueWith(_ => action();</remarks>
    /// <param name="action">The action or method to run. Use delegate lambda " () => " to run. </param>
    /// <param name="delayInSeconds">The delay before running the method.</param>
    /// <param name="debugLog">Whether or not to debug the waiting message.</param>
    /// <param name="cancellationToken"> Token for cancelling the currently running task. Not required. </param>
    public static async UniTask DelayTaskAsync(Action action, float delayInSeconds, bool debugLog = false, CancellationToken cancellationToken = default)
    {
        if (debugLog) Debug.Log($"Waiting for {delayInSeconds} seconds...");
        var timeSpan = TimeSpan.FromSeconds(delayInSeconds);
        await UniTask.Delay(timeSpan, cancellationToken: cancellationToken);
        action();
        if (debugLog) Debug.Log("Action completed.");
    }
}
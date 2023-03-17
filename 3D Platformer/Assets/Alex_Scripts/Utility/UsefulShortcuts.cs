using System;
using System.Collections;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

internal static class UsefulShortcuts
{
    static GameObject testBool;

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

    /// <summary>
    /// Allows you to call a method after a delay through the use of delegates.
    /// </summary>
    /// <param name="delayInSeconds">The delay before running the method.</param>
    /// <param name="action">The action or method to run.</param>
    public static async void DoAfterDelay(Action action, float delayInSeconds)
    {
        Debug.Log("Waiting for " + delayInSeconds + " seconds...");
        var timeSpan = TimeSpan.FromSeconds(delayInSeconds);
        await Task.Delay(timeSpan);
        action();
        Debug.Log("Completed action.");
    }
}
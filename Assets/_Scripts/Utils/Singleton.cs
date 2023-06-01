using UnityEngine;

// Credti: TaroDev (https://www.youtube.com/watch?v=tE1qH8OxO2Y&t=662s)

/// <summary>
/// A static instance is similar to aa singleton, but insted of destroying any new
/// instances, it overrides the current instance. This is for resetting the state
/// and saves you doing it manually
/// </summary>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

/// <summary>
/// This transforms the static instance into a basic singleton. This will destroy any new 
/// versions created, leaving the original instance intatct
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

/// <summary>
/// Finally we have a persistent version of the singleton. This wil survive through the scene
/// loads. Perfect for system classes which require stateful, persistent data. or audio sources
/// where music plays through loading screens, etc
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        base.Awake();
    }
}
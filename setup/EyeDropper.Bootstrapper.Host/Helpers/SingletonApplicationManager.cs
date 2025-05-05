using System.Windows;

namespace EyeDropper.Bootstrapper.Host.Helpers;

/// <summary>
/// Manages a single-instance application by using a named mutex and event wait handle.
/// If another instance is already running, this class signals the existing instance
/// to bring its window to the foreground and then shuts down the new instance.
/// </summary>
public class SingletonApplicationManager
{
    #region Fields/Consts

    /// <summary>
    /// The name of the synchronization event used to signal the existing instance.
    /// </summary>
    private readonly string _uniqueEventName;

    /// <summary>
    /// The name of the mutex used to enforce single-instance ownership.
    /// </summary>
    private readonly string _uniqueMutexName;

    /// <summary>
    /// The event wait handle used for inter-process signaling.
    /// </summary>
    private EventWaitHandle? _eventWaitHandle;

    /// <summary>
    /// The mutex used to determine ownership of the application instance.
    /// </summary>
    private Mutex? _mutex;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="SingletonApplicationManager"/> class
    /// with the specified event and mutex names.
    /// </summary>
    /// <param name="uniqueEventName">
    /// The name of the named <see cref="EventWaitHandle"/> for cross-process signaling.
    /// </param>
    /// <param name="uniqueMutexName">
    /// The name of the named <see cref="Mutex"/> for enforcing single-instance ownership.
    /// </param>
    public SingletonApplicationManager(string uniqueEventName, string uniqueMutexName)
    {
        _uniqueEventName = uniqueEventName ?? throw new ArgumentNullException(nameof(uniqueEventName));
        _uniqueMutexName = uniqueMutexName ?? throw new ArgumentNullException(nameof(uniqueMutexName));
    }

    #region Methods

    /// <summary>
    /// Registers the application instance. If this is the first instance, begins listening
    /// for focus requests from subsequent instances. Otherwise, signals the existing instance
    /// to focus and shuts down the current application.
    /// </summary>
    /// <param name="application">
    /// The WPF <see cref="Application"/> to control (shutdown or dispatcher invoke).
    /// </param>
    /// <param name="onRegistered">
    /// An <see cref="Action"/> to invoke if this instance successfully acquires ownership.
    /// </param>
    /// <param name="onInstanceAlreadyRunning">
    /// An <see cref="Action"/> to invoke on the existing instance when a focus request is received.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="application"/> or <paramref name="onRegistered"/> is <c>null</c>.
    /// </exception>
    public void Register(Application application, Action onRegistered, Action onInstanceAlreadyRunning)
    {
        ArgumentNullException.ThrowIfNull(application, nameof(application));
        ArgumentNullException.ThrowIfNull(onRegistered, nameof(onRegistered));

        bool isOwned;
        _mutex = new Mutex(true, _uniqueMutexName, out isOwned);
        _eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, _uniqueEventName);

        // Prevents the mutex from being garbage-collected while in use.
        GC.KeepAlive(_mutex);

        if (isOwned)
        {
            // Start a background thread to listen for focus requests
            var listenerThread = new Thread(() =>
            {
                while (_eventWaitHandle.WaitOne())
                {
                    application.Dispatcher.Invoke(onInstanceAlreadyRunning);
                }
            })
            {
                IsBackground = true
            };

            listenerThread.Start();
            onRegistered();
            return;
        }

        // Signal the existing instance to focus its window
        _eventWaitHandle.Set();

        // Shut down this instance
        application.Shutdown();
    }

    #endregion
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace LEAPGroup.Core
{
    public class ServiceLocator : MonoBehaviour
    {
        // Runtime service registry. Services register themselves in Awake via
        // ServiceLocator.Register(this); consumers resolve them with ServiceLocator.Get<T>().
        // This replaces per-class "public static Instance" singletons.
        static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        static TaskCompletionSource<bool> readySource;
        static bool instanceExists;

        // Completes once every service has registered (in Awake) AND finished its own start-up
        // (in Start). Awake and Start order across GameObjects is undefined, so a consumer that
        // resolves a service during its own Start is a coin flip. Wait on this instead - see
        // WhenReady below, which is the form most callers want.
        public static Task Ready
        {
            get { return readySource.Task; }
        }

        public static bool IsReady
        {
            get { return isReady; }
        }
        static bool isReady;

        // Fast-enter-playmode (domain reload disabled) keeps statics alive between plays, so
        // clear the registry and re-arm the gate at the start of every play session.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetServices()
        {
            services.Clear();
            isReady = false;
            instanceExists = false;
            readySource = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        async void Awake()
        {
            instanceExists = true;

            // Services register in Awake and finish initializing in Start. Waiting for the end of
            // this frame puts us past every Awake and every Start, which is the first moment the
            // registry is both complete and safe to read from.
            try
            {
                await Awaitable.EndOfFrameAsync(destroyCancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Play mode ended, or this object was destroyed, before the gate opened.
                return;
            }

            MarkReady();
        }

        // AfterSceneLoad runs once every Awake in the scene has completed. If no ServiceLocator
        // component exists by then, nothing would ever complete Ready and every waiting consumer
        // would hang forever, so open the gate rather than deadlock.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void GuardAgainstMissingInstance()
        {
            if (instanceExists)
            {
                return;
            }

            Debug.LogWarning("ServiceLocator: no ServiceLocator component in the loaded scene. "
                + "Opening the ready gate immediately so consumers do not hang.");

            MarkReady();
        }

        static void MarkReady()
        {
            if (isReady)
            {
                return;
            }

            isReady = true;
            readySource.TrySetResult(true);
        }

        public static void Register<T>(T service) where T : class
        {
            services[typeof(T)] = service;
        }

        // Runs onReady once the registry is ready, or straight away if it already is. The callback
        // is dropped if owner was destroyed while waiting, so it never touches a dead object.
        public static void WhenReady(MonoBehaviour owner, Action onReady)
        {
            if (owner == null)
            {
                Debug.LogError("ServiceLocator.WhenReady needs an owner MonoBehaviour to guard against destruction.");
                return;
            }

            if (onReady == null)
            {
                Debug.LogError($"{owner.name}: ServiceLocator.WhenReady was given no callback.", owner);
                return;
            }

            if (isReady)
            {
                onReady();
                return;
            }

            RunWhenReady(owner, onReady);
        }

        // Resolves T for you and hands it to onReady. Once the registry is ready a missing service
        // is a genuine wiring error, so this logs through Get<T>() rather than failing silently.
        public static void WhenReady<T>(MonoBehaviour owner, Action<T> onReady) where T : class
        {
            if (onReady == null)
            {
                Debug.LogError("ServiceLocator.WhenReady was given no callback.", owner);
                return;
            }

            WhenReady(owner, () =>
            {
                T service = Get<T>();

                if (service != null)
                {
                    onReady(service);
                }
            });
        }

        static async void RunWhenReady(MonoBehaviour owner, Action onReady)
        {
            await Ready;

            // A destroyed MonoBehaviour is a live C# reference but a Unity "fake null".
            if (owner == null)
            {
                return;
            }

            onReady();
        }

        // Non-logging lookup. Use for optional services / existence guards, e.g.
        // if (ServiceLocator.TryGet(out InventoryManager inventory)) inventory.Foo();
        public static bool TryGet<T>(out T service) where T : class
        {
            if (services.TryGetValue(typeof(T), out var found) && found is T typed && !IsUnityNull(found))
            {
                service = typed;
                return true;
            }
            service = null;
            return false;
        }

        // Logs if the service is missing. Use when the caller assumes the service exists.
        public static T Get<T>() where T : class
        {
            if (TryGet(out T service))
                return service;

            Debug.LogError($"ServiceLocator: no service registered for {typeof(T).Name}");
            return null;
        }

        // A destroyed MonoBehaviour is a live C# reference but a Unity "fake null"; treat it as absent.
        static bool IsUnityNull(object o) => o is UnityEngine.Object unityObject && unityObject == null;
    }
}

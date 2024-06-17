using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace psp_papers_mod.MonoBehaviour;

public class UnityTaskAction {

    public object Param;
    public Action<object> Action;

    public void Invoke() {
        this.Action.Invoke(this.Param);
    }

    public static UnityTaskAction From<T>(Action<T> action, T param) {
        return new UnityTaskAction {
            Action = obj => action.Invoke((T)obj),
            Param = param,
        };
    }

    public static UnityTaskAction From(Action action) {
        return new UnityTaskAction { Action = _ => action.Invoke(), Param = null, };
    }

}

internal class UnityTaskActionQueue : Queue<UnityTaskAction>;

public class UnityThreadInvoker : UnityEngine.MonoBehaviour {

    private static readonly UnityTaskActionQueue actionQueue = new();

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update() {
        lock (actionQueue) {
            while (actionQueue.Count > 0) actionQueue.Dequeue().Invoke();
        }
    }

    public static void Invoke(UnityTaskAction action) {
        lock (actionQueue) {
            actionQueue.Enqueue(action);
        }
    }

    public static void Invoke(Action action) {
        Invoke(UnityTaskAction.From(action));
    }

    public static void Invoke<T>(Action<T> action, T param) {
        Invoke(UnityTaskAction.From(action, param));
    }

}

public static class UnityThreadInvokerTaskExtensions {

    public static Task SuccessWithUnityThread(this Task task, Action action) {
        return task.ContinueWith(t => {
            if (!t.IsCompletedSuccessfully) {
                Console.Out.WriteLine("Async Error: SuccessWithUnityThread");
                return t;
            }

            UnityThreadInvoker.Invoke(action);
            return t;
        });
    }

    public static Task<T> ResultWithUnityThread<T>(this Task<T> task, Action<T> action) {
        return task.ContinueWith(t => {
            if (!t.IsCompletedSuccessfully) {
                Console.Out.WriteLine("Async Error: ResultWithUnityThread");
                return t.Result;
            }

            UnityThreadInvoker.Invoke(action, t.Result);
            return t.Result;
        });
    }

}
using System.Collections.Generic;

public class PauseHandler : IPause {
    private List<IPause> _handlers = new List<IPause>();

    public bool IsPaused { get; private set; }

    public void Add(IPause handler) => _handlers.Add(handler); 
    public void Remove(IPause handler) => _handlers.Remove(handler);

    public void SetPause(bool isPaused) {
        IsPaused = isPaused;

        foreach (var handler in _handlers) {
            handler.SetPause(isPaused);
        }   
    }
}

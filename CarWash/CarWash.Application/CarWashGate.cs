namespace CarWash.Application;

/// <summary>Объединяет проверку связей и запись в одну операцию.</summary>
public sealed class CarWashGate : IDisposable
{
    private readonly SemaphoreSlim _gate = new(1);

    public async Task<TResult> RunAsync<TResult>(Func<Task<TResult>> action, CancellationToken cancellationToken)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            return await action();
        }
        finally
        {
            _gate.Release();
        }
    }

    public void Dispose()
    {
        _gate.Dispose();
    }
}

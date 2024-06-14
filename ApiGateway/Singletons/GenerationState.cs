using MassTransit;
using Shared.Trip.Events;

namespace ApiGateway.Singletons
{
    public class GenerationState
    {
        private System.Timers.Timer _timer;
        private IPublishEndpoint? _publishEndpoint;
        private readonly object _lock = new object();
        public bool IsGenerating { get; private set; }

        public GenerationState()
        {
            _timer = new System.Timers.Timer(5000);
            _timer.Elapsed += (sender, e) => HandleElapsed().Wait();
        }

        public void SetPublishEndpoint(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        private async Task HandleElapsed()
        {
            if (_publishEndpoint != null)
            {
                await _publishEndpoint.Publish(new GenerateChangesEvent{});
            }
        }

        public void Start()
        {
            lock (_lock)
            {
                if (!IsGenerating)
                {
                    _timer.Start();
                    IsGenerating = true;
                }
            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                if (IsGenerating)
                {
                    _timer.Stop();
                    IsGenerating = false;
                }
            }
        }
    }
}
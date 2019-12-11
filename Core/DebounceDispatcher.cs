using System;
using System.Timers;

namespace PortiaHelper.Core
{
	public class DebounceDispatcher
	{
		private Timer _timer;

		public void Debounce(double interval, Action<object> action, object param = null) {
			_timer?.Stop();

			if (_timer != null) {
				_timer?.Dispose();
				_timer = null;
			}

			_timer = new Timer(interval);
			_timer.Elapsed += delegate (Object sender, ElapsedEventArgs e) {
				_timer?.Stop();
				_timer?.Dispose();

				action.Invoke(param);
			};
			_timer.Start();
		}
	}
}

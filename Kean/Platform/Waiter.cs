using System;

namespace Kean.Platform
{
	class Waiter :
		IRunner
	{
		System.Threading.AutoResetEvent wait = new System.Threading.AutoResetEvent(false);

		#region IRunner Members
		public void Run()
		{
			this.wait.WaitOne();
		}

		public bool Close()
		{
			return this.wait.Set();
		}
		#endregion
	}
}

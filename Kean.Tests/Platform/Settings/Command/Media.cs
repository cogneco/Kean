// 
//  Media.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Kean;
using Kean.Extension;

namespace Kean.Platform.Settings.Test.Command
{
	public class Media
	{
        string resource;
		[Property("resource")]
        [Notify("ResourceChanged")]
        public string Resource 
        {
            get{ return this.resource;}
            private set
            {
                if (value != this.resource)
                {
                    this.resource = value;
                    this.ResourceChanged.Call(value);
                }
            }
        }
        public event Action<string> ResourceChanged;

		MediaState state;
		[Property("state")]
		[Notify("StateChanged")]
		public MediaState State 
		{
			get { return this.state; }
			private set
			{
				switch (value)
				{
					case MediaState.Paused:
						this.timer.Stop();
						break;
					case MediaState.Playing:
						this.timer.Start();
						break;
					case MediaState.Closed:
						this.timer.Stop();
						this.Position = TimeSpan.Zero;
						break;
				}
				this.state = value;
				this.StateChanged.Call(this.state);
			}
		}
		public event Action<MediaState> StateChanged;

		System.Timers.Timer timer;
		TimeSpan position;
		[Property("position")]
		[Notify("PositionChanged")]
		public TimeSpan Position 
		{
			get { return this.position; }
			set
			{
				if (this.State != MediaState.Closed)
				{
					this.position = value;
					this.PositionChanged.Call(this.position);
				}
			}
		}
		public event Action<TimeSpan> PositionChanged;

		public Media()
		{
			this.timer = new System.Timers.Timer(40);
			this.timer.Elapsed += (sender, e) => this.Position += new TimeSpan(0, 0, 0, 0, 40);
			this.timer.AutoReset = true;
		}

		[Method("play")]
		public bool Play()
		{
			bool result;
			if (result = this.State == MediaState.Paused)
				this.State = MediaState.Playing;
			return result;
		}
		[Method("pause")]
		public bool Pause()
		{
			bool result;
			if (result = this.State == MediaState.Playing)
				this.State = MediaState.Paused;
			return result;
		}
		[Method("eject")]
		public bool Eject()
		{
			bool result;
            if (result = this.State != MediaState.Closed)
            {
                this.State = MediaState.Closed;
                this.Resource = null;
            }
			return result;
		}
		[Method("open", "Open a resource.", "Opens a resource and pauses.")]
		public bool Open(string resource)
		{
			bool result;
			if (result = this.State == MediaState.Closed)
			{
				this.State = MediaState.Paused;
				this.Resource = resource;
			}
			return result;
		}
		[Method("changeState")]
		public void ChangeState(MediaState state)
		{
			this.State = state;
		}
	}
}

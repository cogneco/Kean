//
//  Module
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2011-2012 Simon Mika
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
using Serialize = Kean.Serialize;
using Error = Kean.Error;
using Argument = Kean.Cli.Argument;

namespace Kean.Platform.Log
{
	public class Module :
		Platform.Module
	{
		Cache cache = new Log.Cache();

		#region Properties
		[Serialize.Parameter]
		public bool SaveLog { get; set; }
		[Serialize.Parameter]
		public Error.Level ThrowThreshold
		{
			get { return Error.Exception.Threshold; }
			set { Error.Exception.Threshold = value; } 
		}
		[Serialize.Parameter]
		public Error.Level LogThreshold
		{
			get { return this.cache.LogThreshold; }
			set { this.cache.LogThreshold = value; } 
		}
		[Serialize.Parameter]
		public Error.Level AllThreshold
		{
			get { return this.cache.AllThreshold; }
			set { this.cache.AllThreshold = value; }
		}
		#endregion

		#region Constructors
		public Module() :
			base("Log")
		{
			Error.Log.OnAppend += this.Append;
			this.ApplicationChanged += application => this.cache.Writers.Add(new Log.Writer.Csv() { Filename = System.IO.Path.Combine(application.ExecutablePath, System.IO.Path.GetFileNameWithoutExtension(application.Executable) + ".log") });
		}
		#endregion

		#region Module Overrides
		protected internal override void AddArguments(Argument.Parser parser)
		{
			base.AddArguments(parser);
			parser.Add('l', "log", () =>
			{
				Error.Log.CatchErrors = true;
				this.SaveLog = true;
				this.LogThreshold = this.AllThreshold = Error.Exception.Threshold = Error.Level.Warning;
			});
		}
		protected internal override void Start()
		{
			this.Append(Error.Level.Message, "Product", this.Application.Product);
			this.Append(Error.Level.Message, "Version", this.Application.Version);
			this.Append(Error.Level.Message, "Company", this.Application.Company);
			this.Append(Error.Level.Message, "Copyright", this.Application.Copyright);
			this.Append(Error.Level.Message, "Description", this.Application.Description);
			this.Append(Error.Level.Message, "Command Line", this.Application.CommandLine.Fold((string accumululated, string element) => element + " " + accumululated, ""));
			this.Append(Error.Level.Message, "OS", System.Environment.OSVersion.VersionString);
			this.Append(Error.Level.Message, "CLR Version", System.Environment.Version.ToString());
			this.Append(Error.Level.Message, "Machine Name", System.Environment.MachineName);
			base.Start();
		}
		#endregion

		#region Public Methods
		public void Append(Error.Level level, string title, string message)
		{
			this.cache.Append(level, title, message);
		}
		public void Append(Error.IError item)
		{
			this.cache.Append(item);
		}
		public void Append(Error.Level level, string title, System.Exception exception)
		{
			this.cache.Append(level, title, exception);
		}
		public void Flush()
		{
			this.cache.Flush();
		}
		#endregion

		protected internal override void Dispose()
		{
			if (this.cache.NotNull())
			{
				if (this.SaveLog)
					this.Flush();
				Error.Log.OnAppend -= this.Append;
				this.cache.Dispose();
				this.cache = null;
			}
			base.Dispose();
		}
	}
}

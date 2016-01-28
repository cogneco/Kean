//
//  Program.cs
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
using Kean.Extension;
using IO = Kean.IO;
using Uri = Kean.Uri;

namespace Kean.Platform.Settings.Test
{
	public class Program
	{
		public static void Run(string[] args)
		{
			if (args.Length == 0)
			{
				Command.Application application = new Command.Application();
				IDisposable telnet = Parser.Listen(application, "telnet://:23");
				IDisposable tcp = Parser.Listen(application, "tcp://:20");
				IDisposable console = Parser.Listen(application, "console:///");
			}
			else
			{
				Application application = new Application();
				Settings.Module settings = new Settings.Module()
				{
					//Header = "<link href=\"resources/settings.css\" rel=\"stylesheet\" type=\"text/css\"/>\n <link href=\"resources/settings.css\" rel=\"stylesheet\" type=\"text/css\"/>",
				};
				settings.Load("loader", new Loader(settings));
				settings.Load("old.object", new Object());
				settings.Load("application", new Command.Application());
				application.Load(settings);
				application.Start();
				System.Threading.AutoResetEvent wait = new System.Threading.AutoResetEvent(false);
				application.OnClosed += () => wait.Set();
				wait.WaitOne();
				//application.Execute();
			}
		}
	}
}

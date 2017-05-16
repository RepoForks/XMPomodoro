using System;
using System.Collections.Generic;
using AppKit;
namespace Pomodoro
{
	public class ActivityLogDataSource : NSTableViewDataSource
	{
		public ActivityLogDataSource()
		{
		}

		public List<Activity> Activities = new List<Activity>();

		public override nint GetRowCount(NSTableView tableView)
		{
			return Activities.Count;
		}
	}
}

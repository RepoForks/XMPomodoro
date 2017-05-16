using System;
using AppKit;
using System.Collections.Generic;
namespace Pomodoro
{
	public class ActivityLogDelegate : NSTableViewDelegate
	{
		private const string CellIdentifier = "ActivityCell";

		private ActivityLogDataSource DataSource;

		public ActivityLogDelegate(ActivityLogDataSource datasource)
		{
			this.DataSource = datasource;
		}

		public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
		{
			// This pattern allows you reuse existing views when they are no-longer in use.
			// If the returned view is null, you instance up a new view
			// If a non-null view is returned, you modify it enough to reflect the new data
			NSTextField view = (NSTextField)tableView.MakeView(CellIdentifier, this);
			if (view == null)
			{
				view = new NSTextField();
				view.Identifier = CellIdentifier;
				view.BackgroundColor = NSColor.Clear;
				view.Bordered = false;
				view.Selectable = false;
				view.Editable = false;
			}

			// Setup view based on the column and row
			switch (tableColumn.Title)
			{
				case "Time Completed":
					view.StringValue = DataSource.Activities[(int)row].TimeCompleted.ToString("H:mm");
					break;
				case "Task":
					view.StringValue = DataSource.Activities[(int)row].TaskDescription;
					break;
			}

			return view;
		}
	}
}

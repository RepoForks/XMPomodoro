using System;
using System.Timers;
using AppKit;
using CoreGraphics;
using Foundation;

namespace Pomodoro
{
	public partial class ViewController : NSViewController
	{
		Timer MainTimer;
		int TimeLeft = 1500; // 1500 seconds in 25 minutes
		ActivityLogDataSource log = new ActivityLogDataSource();

		public ViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ActivityLog.DataSource = log;
			ActivityLog.Delegate = new ActivityLogDelegate(log);

			// Fire the timer once a second
			MainTimer = new Timer(1000);
			MainTimer.Elapsed += (sender, e) => {
				TimeLeft--;
				// Format the remaining time nicely for the label
				TimeSpan time = TimeSpan.FromSeconds(TimeLeft);
				string timeString = time.ToString(@"mm\:ss");
				InvokeOnMainThread(() => { 
					// We want to interact with the UI from a different thread,
					// so we must invoke this change on the main thread
					TimerLabel.StringValue = timeString; 
				});

				// If 25 minutes have passed
				if (TimeLeft == 0)
				{
					// Stop the timer and reset
					MainTimer.Stop();
					TimeLeft = 1500;

                    // Trigger a local notification after the time has elapsed
					var notification = new NSUserNotification();
                    // Add text and sound to the notification
					notification.Title = "25 Minutes is up!";
					notification.InformativeText = "Add your task to your activity log";
					notification.SoundName = NSUserNotification.NSUserNotificationDefaultSoundName;
                    notification.HasActionButton = true; // Show "close" and "show" buttons when the notification is displayed as an alert
					NSUserNotificationCenter.DefaultUserNotificationCenter.DeliverNotification(notification);

					InvokeOnMainThread(() => {
						// Reset the UI
						TimerLabel.StringValue = "25:00";
						StartStopButton.Title = "Start";

						NSAlert alert = new NSAlert();
						// Set the style and message text
						alert.AlertStyle = NSAlertStyle.Informational;
						alert.MessageText = "25 Minutes elapsed! Take a 5 minute break and fill out your completed task.";
						// The text field we are going to add to the sheet
						var input = new NSTextField(new CGRect(0, 0, 300, 20));
						// Add the text field to the sheet
						alert.AccessoryView = input;

						// Get the current time
						DateTime CurrentTime = DateTime.Now;
						// Display the NSAlert from the current view
						alert.BeginSheetForResponse(View.Window, (result) =>
						{
							// When the sheet is dismissed add the activity to the log
							log.Activities.Add(new Activity(CurrentTime, input.StringValue));
							ActivityLog.ReloadData();
						});
					});
				}
			};
		}

		public override NSObject RepresentedObject
		{
			get
			{
				return base.RepresentedObject;
			}
			set
			{
				base.RepresentedObject = value;
				// Update the view, if already loaded.
			}
		}

		partial void StartStopButtonClicked(NSObject sender)
		{
			// If the timer is running, we want to stop it,
			// otherwise we want to start it
			if (MainTimer.Enabled)
			{
				MainTimer.Stop();
				StartStopButton.Title = "Start";
			}
			else 
			{
				MainTimer.Start();
				StartStopButton.Title = "Stop";
			}
		}
	}
}

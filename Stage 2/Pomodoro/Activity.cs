using System;
namespace Pomodoro
{
	public class Activity
	{
		public DateTime TimeCompleted { get; set; }
		public string TaskDescription { get; set; }

		public Activity()
		{
		}

		public Activity(DateTime timeCompleted, string taskDescription)
		{
			this.TimeCompleted = timeCompleted;
			this.TaskDescription = taskDescription;
		}
	}
}

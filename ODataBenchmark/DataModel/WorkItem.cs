namespace ODataBenchmark.DataModel
{
	public class WorkItem : Entity
	{
		public WorkItemType WorkItemType { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public long ProjectId { get; set; }
		public virtual Project Project { get; set; }
		public WorkItemState WorkItemState { get; set; }
	}

	public enum WorkItemType
	{
		Default,
		Bug,
		Task,
		Epic,
		Spike
	}

	public enum WorkItemState
	{
		New,
		InProgress,
		InTest,
		Cancelled,
		Done
	}
}

namespace Ranna.Task2.Business.Responses
{
	public class ResponseContainer<T>
	{
		public ResponseStatus Status { get; set; }
		public string Message { get; set; }
		public T Data { get; set; }
		public List<ValidationError> ValidationErrors { get; set; } = [];
	}

	public class ResponseContainer
	{
		public ResponseStatus Status { get; set; }
		public string Message { get; set; }
		public List<ValidationError> ValidationErrors { get; set; } = [];
	}
	public enum ResponseStatus
	{
		Success,
		Failed,
		ValidationError,
		Exception
	}
}

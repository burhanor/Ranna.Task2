namespace Ranna.Task2.Api.Helpers
{
	public static class IFormFileHelper
	{
		public static bool IsValidImage(IFormFile? file)
		{
			if (file is null || file.Length == 0)
				return true;
			if (file.ContentType.ToLower().StartsWith("image/"))
				return true;
			return false;
		}

		public static byte[]? ToByteArray(IFormFile? file)
		{
			if (file is null || file.Length == 0)
				return null;
			using var memoryStream = new MemoryStream();
			file.CopyTo(memoryStream);
			return memoryStream.ToArray();
		}
	}
}

namespace WMessageServiceApi.Authentication
{
	public struct TokenValidationResult
	{
		public bool IsValidationSuccess { get; set; }
		public int Status { get; set; }
		public string Message { get; set; }
	}
}
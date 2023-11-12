namespace BackendAPI.Response
{
    public class ResponseReport
    {
        public int? Status { get; set; }
        public string? Message { get; set; }
        public string? Errors { get; set; }
    }
}

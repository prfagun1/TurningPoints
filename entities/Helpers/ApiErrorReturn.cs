namespace entities.Helpers
{
    public class ApiErrorReturn
    {
        public string Error { get; set; }
        public DateTime Date { get; set; }
        public string Request { get; set; }

        public ApiErrorReturn(string error, DateTime date, string request)
        {
            Error = error;
            Date = date;
            Request = request;
        }
    }
}

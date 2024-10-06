using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entities;

namespace entity
{
    public class APIResponse
    {
        public APIResponse(Enumerators.APIResponseStatus apiResponseStatus, string message, string request, int requestDurationSeconds = 0)
        {
            APIResponseStatus = apiResponseStatus;
            Message = message;
            Request = request;
            StartDate = DateTime.Now.AddSeconds(-requestDurationSeconds);
            EndDate = DateTime.Now;
            RequestDurationSeconds = requestDurationSeconds;
        }

        public Enumerators.APIResponseStatus APIResponseStatus { get; set; }
        public string Message { get; set; }
        public string Request { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RequestDurationSeconds { get; set; }
    }
}

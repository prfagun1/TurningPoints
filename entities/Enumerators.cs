namespace entities
{
    public class Enumerators
    {
        public enum SortOrder
        {
            Ascending = 0,
            Descending = 1
        }

        public enum Permission
        {
            Admin = 1,
            Manager = 2,
            Standard = 3
        }

        public enum Status
        {
            Active = 1,
            Deactivated = 2
        }

        public enum YesNoStatus
        {
            Yes = 1,
            No = 2
        }

        public enum APIConnection
        {
            api = 1,
        }

        public enum APIResponseStatus
        {
            Error = 0,
            OK = 1
        }
    }
}

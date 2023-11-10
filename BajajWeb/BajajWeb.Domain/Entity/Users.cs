using System;

namespace BajajWeb.Domain.Entity
{
    public class Users
    {
        public int id { get; set; }
        public string Login { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string mail { get; set; }
        public string password { get; set; }
        public DateTime date_of_birth { get; set; }
        public string phone_number { get; set; }
        public int id_role { get; set; }
        public int id_job_title { get; set; }
    }
}

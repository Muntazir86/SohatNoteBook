namespace SohatNotebook.Entities.DbSet
{
    public class User : BaseEntity {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime Datebirth { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string Sex { get; set; }
    }    
}
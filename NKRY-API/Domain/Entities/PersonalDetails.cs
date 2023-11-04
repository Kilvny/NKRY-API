namespace NKRY_API.Domain.Entities
{
    public class PersonalDetails
    {
        public int Id { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime? VisaExpiryDate { get; set; }
        public DateTime? FlightTicketsDueDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DuesPayDate { get; set; }
        
    }
}

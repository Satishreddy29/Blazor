namespace TrackingApi.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? ProductName { get; set; }
        
        
        public string? TrackingId { get; set; }
        public string? TrackingStatus { get; set; }
    }
}

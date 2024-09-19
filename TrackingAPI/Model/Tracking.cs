namespace TrackingApi.Model
{
    public class Tracking
    {
        public List<Customer>? customers { get; set; }
        public List<Order>? orders { get; set; }
        public string? TrackingId { get; set; }
        public string? TrackingStatus { get; set; }


    }
}

namespace NET_MVC_Environment.Models
{
    public class Data
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime UploadDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }

    public class DataViewModel
    {
        public List<Data> dataClass { get; set; }
    }
}

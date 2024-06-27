namespace InHues.Domain.DTO.Custom
{
    public class OdataRequestBase
    {
        public string Id { get; set; } = string.Empty;
        public bool Count { get; set; }
        public string Expand { get; set; } = string.Empty;
        public int Top { get; set; }
        public int Skip { get; set; }
        public string OrderBy { get; set; } = string.Empty;
        public string SearchString { get; set; } = string.Empty;
        public string Select { get; set; } = string.Empty;
        public string Apply { get; set; } = string.Empty;
    }
}

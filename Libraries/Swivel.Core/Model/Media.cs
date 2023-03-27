namespace Swivel.Core.Model
{
    public class Media : BaseEntity
    {
        public string Title { get; set; }
        public string OriginalFileName { get; set; }
        public string Format { get; set; }
        public string PublicId { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public long Bytes { get; set; }
        public long Length { get; set; }
        public int JobId { get; set; }
        public virtual Job Job { get; set; }
    }
}

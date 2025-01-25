namespace ZurlocloudRunningWebAPI.Entities
{
    public class VectorSearchResult
    {
        public required int StoreID { get; set; }
        public required string Details { get; set; }
        public required string Source { get; set; }
        public required float SimilarityScore { get; set; }
    }
}

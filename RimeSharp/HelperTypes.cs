namespace RimeSharp
{
    public record class CandidateItem
    {
        public string Text { get; }
        public string? Comment { get; }

        public CandidateItem(string text, string? comment)
        {
            Text = text;
            Comment = comment;
        }
    };

    public record class SchemaListItem
    {
        public string SchemaId { get; }
        public string Name { get; }

        public SchemaListItem(string schemaId, string name)
        {
            SchemaId = schemaId;
            Name = name;
        }
    };
}

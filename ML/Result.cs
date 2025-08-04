namespace ML
{
    public class Result
    {
        public bool Correct { get; set; }
        public string? ErrorMesage { get; set; }
        public Exception? exception { get; set; }
        public Object? Object { get; set; }
        public List<Object>? Objects { get; set; }
    }
}

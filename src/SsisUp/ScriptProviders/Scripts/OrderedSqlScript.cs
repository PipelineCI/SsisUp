namespace SsisUp.Configurations
{
    public class OrderedSqlScript
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public string Contents { get; set; }
        public string FullName { get { return string.Format("{0} - {1}", Order, Name); } }
    }
}
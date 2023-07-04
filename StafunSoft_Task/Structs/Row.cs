namespace ShtafunSoft_Task.Structs
{
    public class Row
    {
        public List<object> objects { get; set; } = new List<object>();

        public Row Add(object anObject)
        {
            objects.Add(anObject);
            return this;
        }
    }
}

namespace ShtafunSoft_Task.Structs
{
    public class Column
    {
        public List<object> objects { get; set; } = new List<object>();

        public Column Add(object anObject)
        {
            objects.Add(anObject);
            return this;
        }
    }
}

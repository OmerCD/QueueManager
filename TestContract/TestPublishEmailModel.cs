using System.Collections.Generic;
using QueueManager.Contract;

namespace TestContract
{
    public class TestPublishEmailModel : IQueueMessage
    {
        public string Content { get; set; }
    }
    public class EmailModel : IEmailQueueMessage
    {
        public int EmailTemplate { get; set; }
        public Parameter[] Parameters { get; set; }
    }

    public interface IEmailQueueMessage : IQueueMessage
    {
        Parameter[] Parameters { get; set; }
    }
    
    public class Parameter 
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public Parameter(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public Parameter()
        {
            
        }
        public IEnumerator<Parameter> GetEnumerator()
        {
            yield return this;
        }
        //
        // IEnumerator IEnumerable.GetEnumerator()
        // {
        //     return GetEnumerator();
        // }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Util
{
    [Serializable]
    public class UserFriendlyException : ApplicationException
    {
        public UserFriendlyException()
        {

        }

        public UserFriendlyException(string message) : base(message)
        {

        }

        public UserFriendlyException(string message, Exception inner) : base(message, inner)
        {

        }

        public UserFriendlyException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
            
        }
    }
}

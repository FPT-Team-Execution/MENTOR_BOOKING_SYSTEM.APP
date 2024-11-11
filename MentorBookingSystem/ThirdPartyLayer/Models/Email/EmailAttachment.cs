using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdPartyLayer.Models.Email
{
    public class EmailAttachment
    {
        public byte[] Value { get; private set; }

        public string Name { get; private set; }

        public static EmailAttachment Create(byte[] value, string name)
        {
            return new EmailAttachment
            {
                Value = value,
                Name = name
            };
        }
    }
}

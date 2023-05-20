using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickHouseApp.Dto
{
    public enum TagTypeInfo
    {
        None = 0,
        Int,
        String,
        Float,
        Bool
    }

    public class Signal
    {
        public Guid SignalId { get; set; }

        public string TagName { get; set; }

        public TagTypeInfo TagType { get; set; }

        public object TagValue { get; set; }
    }
}

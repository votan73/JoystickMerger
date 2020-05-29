using System;
using System.Collections.Generic;
using System.Linq;

namespace JoystickMerger.DualT16000M
{
    class IdentifierList : List<Guid>
    {
        static readonly char[] separator = new char[] { ';' };
        public IdentifierList(string list)
        {
            if (!String.IsNullOrEmpty(list))
                AddRange(list.Split(separator, StringSplitOptions.RemoveEmptyEntries).Select<string, Guid>(x => new Guid(x)));
        }
        public IdentifierList(IEnumerable<Guid> list)
            : base(list)
        {
        }
        //public new Guid this[int index]
        //{
        //    get
        //    {
        //        if (index >= Count)
        //            return Guid.Empty;
        //        else
        //            return base[index];
        //    }
        //    set
        //    {
        //        if (index < Count)
        //            base[index] = value;
        //        else
        //        {
        //            for (int i = Count; i < index; i++)
        //                Add(Guid.Empty);
        //            Add(value);
        //        }
        //    }
        //}
        public override string ToString()
        {
            return String.Join(";", this.ToArray());
        }
    }
}

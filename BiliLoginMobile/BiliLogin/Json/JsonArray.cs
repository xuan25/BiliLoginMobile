using System.Collections.Generic;
using System.Dynamic;

namespace Json
{
    /// <summary>
    /// Class <c>JsonArray</c> models an Array in json.
    /// Author: Xuan525
    /// Date: 07/04/2019
    /// </summary>
    public class JsonArray : DynamicObject
    {
        private List<object> list = new List<object>();

        /// <summary>
        /// The number of items in the Array
        /// </summary>
        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        /// <summary>
        /// Add a value to the Array
        /// </summary>
        /// <param name="value">The Value</param>
        public void Add(object value)
        {
            list.Add(value);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            HashSet<string> set = new HashSet<string>();
            for (int i = 0; i < list.Count; i++)
                set.Add(i.ToString());
            return set;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            int index = int.Parse(binder.Name);
            if (index < list.Count)
            {
                result = list[index];
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            list[int.Parse(binder.Name)] = value;
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (list.Count > (int)indexes[0])
                result = list[(int)indexes[0]];
            else
                throw new System.NullReferenceException();
            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (list.Count > (int)indexes[0])
                list[(int)indexes[0]] = value;
            else
            {
                while (list.Count < (int)indexes[0])
                    list.Add(null);
                list.Add(value);
            }
            return true;
        }

    }
}

using Moba.Domain.Core;
using System;
using System.Linq;

namespace Moba.Common.Helpers
{
    public static class EnumHelper
    {
        public static string GetEnumItemDisplayName(Type enumList, string value)
        {
            if (enumList.IsEnum)
            {
                var item = enumList.GetMember(value);
                var attr = item.FirstOrDefault(a => a.Name == value);

                var displayName = attr.GetCustomAttributes(true).Where(a => a is TitleAttribute).Select(t => t as TitleAttribute).FirstOrDefault();
                if (displayName == null)
                {
                    throw new Exception("title attribute not found");
                }
                return displayName.Title;


            }
            throw new Exception("item is not a valied enum");
        }
    }
}

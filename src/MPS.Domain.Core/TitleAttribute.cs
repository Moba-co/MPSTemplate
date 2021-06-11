using System;

namespace MPS.Domain.Core
{
    public class TitleAttribute : Attribute
    {
        public string Title;

        public TitleAttribute(string title)
        {
            Title = title;
        }
    }
}

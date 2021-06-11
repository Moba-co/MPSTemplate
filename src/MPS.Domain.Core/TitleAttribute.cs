using System;

namespace Moba.Domain.Core
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

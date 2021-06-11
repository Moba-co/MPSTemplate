using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Moba.Data.EF.Helpers
{
    public class MapBy<TViewModel>
    {
        private readonly List<Expression<Func<TViewModel, object>>> _allExpressions = new List<Expression<Func<TViewModel, object>>>();


        //public List<string> AllStringIncludes { get; } = new List<string>();

        public MapBy(params Expression<Func<TViewModel, object>>[] expressions)
        {
            _allExpressions.AddRange(expressions);
        }

        //public MapBy(params string[] includes)
        //{
        //    AllStringIncludes.AddRange(includes);
        //}

        public void AddMapBy(Expression<Func<TViewModel, object>> expression)
        {
            _allExpressions.Add(expression);
        }

        public void ClearAllMaps()
        {
            _allExpressions.Clear();
        }
        public IEnumerable<string> GetIncludeProperties()
        {
            var result = new List<string>();
            foreach (var byDot in _allExpressions.Select(expression => expression.Body.ToString().Split('.')
                .Where(w => w!= "And()").Skip(1)).Select(splitedByDot => splitedByDot as string[] ?? splitedByDot.ToArray()))
            {
                if(byDot.Any(a => a.Contains("Select(")))
                {
                    throw new Exception("به جای سلکت از اند استفاده کنید");
                }
                result.Add(string.Join(".", byDot));
            }
            return result;
        }
    }
    // public static class SelectHelper
    // {
    //     public static T And<T>(this IEnumerable<T> enumerable)
    //     {
    //         return enumerable.First();
    //     }
    // }
    // public enum MapByType
    // {
    //     StringInclude,
    //     Expression
    // }
}
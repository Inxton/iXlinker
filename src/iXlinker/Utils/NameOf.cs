using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace iXlinker.Utils
{
    public static class EventLoggerDetails
    {
        public static string NameOf<TRoot>(Expression<Func<TRoot, object>> pathExpression)
        {
            var members = new Stack<string>();
            for (
              var memberExpression = pathExpression.Body as MemberExpression;
              memberExpression != null;
              memberExpression = memberExpression.Expression as MemberExpression
            )
            {
                members.Push(memberExpression.Member.Name);
            }
            members.Push(typeof(TRoot).Name);
            return string.Join(".", members);
        }
    }
}

/*
 * User: Thorsten Lorenz
 * Date: 6/29/2009
 * 
 */
using System;
using System.Reflection;
using Tools.Extensions;

namespace Tools.GenericUtilities
{
    /// <summary>
    /// Creates a string of this format: MemberType MemberName=MemberValue
    /// Usage:
    /// <code>
    ///     string testMember = "testing";
    ///     Console.WriteLine(Member.State(() => testMember));
    /// </code>
    /// Writes  ' string testMember="testing" ' to the Console.
    /// </summary>
    public static class Member
    {
        public static string State<T>(Func<T> expr)
        {
            var member = ExtractMemberFromLambdaExpression(expr);
            
            Type memberType = GetTypeOfMember(member);
            
            string contents = ExtractContentsFromLambdaExpression(expr);
            
            return string.Format("{0} {1}={2}",memberType.Name,  member.Name, contents);
        }

        static string ExtractContentsFromLambdaExpression<T>(Func<T> expr)
        {
            if (expr() == null) {
                return "NULL";
            }
            
            string contents = string.Empty;
            if (expr().GetType().IsArray) {
                foreach (var item in (expr() as Array)) {
                    contents += item.ToStringNullSafe() + ", ";
                }
                contents = contents.Trim().TrimEnd(',');
            } else {
                contents = expr().ToString();
            }
            
            return contents;
        }
        
        static MemberInfo ExtractMemberFromLambdaExpression<T>(Func<T> expr)
        {
            // get IL code behind the delegate
            var il = expr.Method.GetMethodBody().GetILAsByteArray();
            // bytes 2-6 represent the member handle
            var memberHandle = BitConverter.ToInt32(il, 2);
            // resolve the handle
            return expr.Target.GetType().Module.ResolveMember(memberHandle);
        }


        static Type GetTypeOfMember(MemberInfo member)
        {
            Type memberType;
            if (member.MemberType == MemberTypes.Field) {
                memberType = GetFieldType(member as FieldInfo);
            }
            else if (member.MemberType == MemberTypes.Property) {
                memberType = GetPropertyType(member as PropertyInfo);
            }
            else {
                memberType = typeof(object);
            }
            return memberType;
        }
        
        static Type GetFieldType(FieldInfo fieldInfo)
        {
            return fieldInfo.FieldType;
        }
        
        static Type GetPropertyType(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType;
        }
    }
}
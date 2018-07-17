using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GaoJD.Club.Core
{
    /// <summary>
    /// 表达式树拓展类
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// 将Lambda表达式解析成SQL语句
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public static string ToMSSqlString(this Expression expression)
        {
            string itemstr = AiExpressionWriterSql.BizWhereWriteToString(expression, AiExpSqlType.aiWhere);
            return itemstr;
        }
        /// <summary>
        /// 将Lambda表达式解析成参数化的SQL语句
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public static string ToParameterizedMSSqlString(this Expression expression, out List<object> parameters)
        {
            parameters = new List<object>();
            StringWriter writer = new StringWriter();
            SimpleParameterizedWriterSql.Write(writer, expression, parameters);
            return writer.ToString();
        }

        #region 方法存在bug，舍弃，不要再使用
        ///// <summary>
        ///// 将Lambda表达式解析成SQL语句
        ///// </summary>
        ///// <param name="expressioin"></param>
        ///// <param name="parameters">格式化时是否使用模版</param>
        ///// <returns></returns>
        //internal static string InternalToMSSqlString(this Expression expression, List<object> parameters) {
        //    switch (expression.NodeType) {
        //        case ExpressionType.AndAlso://条件 AND 运算 a && b
        //            var andAlso = expression as BinaryExpression;
        //            return andAlso.Left.InternalToMSSqlString(parameters) + " AND " + andAlso.Right.InternalToMSSqlString(parameters);
        //        case ExpressionType.ArrayLength://获取一维数组的长度 array.Length
        //            var arrayLength = expression as UnaryExpression;
        //            string arrayStr2 = arrayLength.Operand.InternalToMSSqlString(null);
        //            int arrayLen = arrayStr2.Split(',').Length;
        //            if (parameters != null) {
        //                parameters.Add(arrayLen);
        //            }
        //            return arrayLen.ToString();
        //        case ExpressionType.Call://方法调用 obj.Method()
        //            var call = expression as MethodCallExpression;
        //            MethodInfo mInfo = call.Method;
        //            if (mInfo.Name == "Contains") {//contais特殊处理
        //                return ConvertContainsToMSSqlString(call);
        //            }
        //            object ret = null;
        //            if (mInfo.ReturnParameter == null || !IsPrimitive(mInfo.ReturnType)) {
        //                throw new ArgumentException("方法必须有返回值并且返回值必须是基元类型");
        //            }
        //            object[] parms = call.Arguments.Select(p => {
        //                if (p is MemberExpression) {//参数是变量
        //                    //由于是参数所以不需要sql字符串
        //                    string sql = p.InternalToMSSqlString(null).Trim((char)39);
        //                    if (p.Type == typeof(string)) {//提高效率
        //                        return sql;
        //                    }
        //                    IConvertible convertible = sql as IConvertible;
        //                    return convertible.ToType(p.Type, null);
        //                } else if (p is ConstantExpression) {//参数是常量
        //                    return GetConstantExpressionValue((ConstantExpression)p);
        //                } else {
        //                    throw new ArgumentException("不支持的写法: " + p.ToString());
        //                }
        //            }).ToArray();
        //            if (mInfo.IsStatic) {//静态方法
        //                ret = mInfo.Invoke(null, parms);
        //            } else {//实例方法
        //                if (call.Object is MemberExpression) {//实例对象
        //                    MemberExpression m = call.Object as MemberExpression;
        //                    if (m.Expression is ConstantExpression) {
        //                        object instance = GetConstantExpressionValue((ConstantExpression)m.Expression, m.Member.Name);
        //                        ret = mInfo.Invoke(instance, parms);
        //                    }
        //                }
        //            }
        //            if (ret == null) {
        //                throw new NotSupportedException("不支持的写法: " + call.ToString());
        //            }
        //            //注入参数
        //            if (parameters != null) {
        //                parameters.Add(ret);
        //            }
        //            return FormatExpressionValueToSQL(ret);
        //        case ExpressionType.Convert://强制转换或转换操作
        //            var convert = expression as UnaryExpression;
        //            return convert.Operand.InternalToMSSqlString(parameters);
        //        case ExpressionType.Constant://一个常量值
        //            var constant = expression as ConstantExpression;
        //            //注入参数
        //            if (parameters != null) {
        //                parameters.Add(constant.Value);
        //            }
        //            return FormatExpressionValueToSQL(constant.Value);
        //        case ExpressionType.Equal://等于 a == b
        //            var equal = expression as BinaryExpression;
        //            return LogicBinaryExpressionToString(equal, "=", parameters);
        //        case ExpressionType.GreaterThan://大于 a > b
        //            var greaterThan = expression as BinaryExpression;
        //            return LogicBinaryExpressionToString(greaterThan, ">", parameters);
        //        case ExpressionType.GreaterThanOrEqual://大于登录 a >= b
        //            var greaterThanOrEqual = expression as BinaryExpression;
        //            return LogicBinaryExpressionToString(greaterThanOrEqual, ">=", parameters);
        //        case ExpressionType.Lambda://lambda表达式
        //            var lambda = expression as LambdaExpression;
        //            string sqlStr = lambda.Body.InternalToMSSqlString(parameters);
        //            if (sqlStr.StartsWith("(") && sqlStr.EndsWith(")")) {
        //                sqlStr = sqlStr.Trim('(', ')');
        //            }
        //            return sqlStr;
        //        case ExpressionType.LessThan://小于 a < b
        //            var lessThan = expression as BinaryExpression;
        //            return LogicBinaryExpressionToString(lessThan, "<", parameters);
        //        case ExpressionType.LessThanOrEqual://小于等于 a <= b
        //            var lessThanOrEqual = expression as BinaryExpression;
        //            return LogicBinaryExpressionToString(lessThanOrEqual, "<=", parameters);
        //        case ExpressionType.ListInit://创建 IEnumerable 对象并初始化 new List<int>(){ 1, 2, 3 }
        //            var listInit = expression as ListInitExpression;
        //            string str = string.Empty;
        //            foreach (var init in listInit.Initializers) {
        //                foreach (var arg in init.Arguments) {
        //                    str += arg.InternalToMSSqlString(parameters) + ",";
        //                }
        //            }
        //            return str.TrimEnd(',');
        //        case ExpressionType.MemberAccess://从字段或属性进行读取运算 obj.SomeProperty
        //            var memberAccess = expression as MemberExpression;
        //            if (memberAccess.Expression is ParameterExpression) {//左值是参数
        //                return string.Format("[{0}]", memberAccess.Member.Name);
        //            } else if (memberAccess.Expression is ConstantExpression) {//右值是常量
        //                object o = GetConstantExpressionValue((ConstantExpression)memberAccess.Expression, memberAccess.Member.Name);
        //                //注入参数
        //                if (parameters != null) {
        //                    parameters.Add(o);
        //                }
        //                return FormatExpressionValueToSQL(o);
        //            } else {
        //                return memberAccess.Expression.InternalToMSSqlString(parameters);
        //            }
        //        case ExpressionType.NewArrayInit://创建一维数组并初始化数组 new int[]{ 1, 2, 3}
        //            var newArrayInit = expression as NewArrayExpression;
        //            string str2 = string.Empty;
        //            foreach (var n in newArrayInit.Expressions) {
        //                str2 += n.InternalToMSSqlString(parameters) + ",";
        //            }
        //            return str2.TrimEnd(',');
        //        case ExpressionType.Not://按位求补 ~a 或者 按逻辑求反 !a
        //            var not = expression as UnaryExpression;
        //            return not.Operand.InternalToMSSqlString(parameters);
        //        case ExpressionType.NotEqual://不相等比较 a != b
        //            var notEqual = expression as BinaryExpression;
        //            return LogicBinaryExpressionToString(notEqual, "<>", parameters);
        //        case ExpressionType.OrElse://按条件Or运算 a || b
        //            var orElse = expression as BinaryExpression;
        //            return string.Format("({0} OR {1})", orElse.Left.InternalToMSSqlString(parameters), orElse.Right.InternalToMSSqlString(parameters));
        //        //return orElse.Left.ToParameterizedMSSqlString(useTemplate) + " OR " + orElse.Right.ToParameterizedMSSqlString(useTemplate);
        //        #region NotImplemented
        //        case ExpressionType.LeftShift://左移 a << b
        //        case ExpressionType.Modulo://求余 a % b
        //        case ExpressionType.Multiply://乘法 a * b
        //        case ExpressionType.Subtract://减法 a -b
        //        case ExpressionType.Add://加法 a + b
        //        case ExpressionType.Coalesce://null合并运算 a ?? b
        //        case ExpressionType.Divide://除法 a / b
        //        case ExpressionType.ExclusiveOr://按位或逻辑 XOR a ^ b
        //        case ExpressionType.Increment://一元递增运算 a + 1
        //        case ExpressionType.Or://按位Or运算 a | b
        //        case ExpressionType.Negate://求反运算 -a
        //        case ExpressionType.Power://幂运算 a ^ b
        //        case ExpressionType.OnesComplement://反码运算 ~a
        //        case ExpressionType.PostDecrementAssign://一元后缀递减 a--
        //        case ExpressionType.PostIncrementAssign://一元后缀递增 a++
        //        case ExpressionType.PreDecrementAssign://一元前缀递减 --a
        //        case ExpressionType.PreIncrementAssign://一元前缀递增 ++a
        //        case ExpressionType.Conditional://条件运算 a > b ? a : b
        //        case ExpressionType.ArrayIndex://一维数组的索引运算 array[index]
        //        case ExpressionType.IsFalse://false
        //        case ExpressionType.IsTrue://true
        //        case ExpressionType.Parameter://对在表达式中定义的参数或变量的引用
        //        case ExpressionType.PowerAssign://幂运算并赋值 a ^= b
        //        case ExpressionType.Quote://包含对表达式上下文参数的引用
        //        case ExpressionType.RightShift://右移运算 a>> b
        //        case ExpressionType.RightShiftAssign://右移并赋值 a >>= b
        //        case ExpressionType.RuntimeVariables://运行时变量的列表
        //        case ExpressionType.SubtractAssign://减法并赋值 a -= b
        //        case ExpressionType.SubtractAssignChecked://减法并赋值 a -= b 并check检查
        //        case ExpressionType.SubtractChecked://减法 a - b 并check检查
        //        case ExpressionType.Switch://switch表达式
        //        case ExpressionType.Throw://throw表达式
        //        case ExpressionType.Try://try-catch表达式
        //        case ExpressionType.TypeAs://as运算
        //        case ExpressionType.TypeEqual://确切类型运算 a = b
        //        case ExpressionType.TypeIs://类型测试 a is SomeType
        //        case ExpressionType.UnaryPlus://一元加法运算 +a
        //        case ExpressionType.Unbox://取消装箱运算
        //        case ExpressionType.AddAssign://加法并赋值 a += b
        //        case ExpressionType.AddAssignChecked://加法并赋值 a += b 并check
        //        case ExpressionType.AddChecked://加法 a + b 并check
        //        case ExpressionType.And://按位或逻辑 AND 运算 a & b
        //        case ExpressionType.AndAssign://按位或逻辑 AND 运算并赋值 a &= b
        //        case ExpressionType.Assign://赋值运算 a = b
        //        case ExpressionType.Block://表达式快
        //        case ExpressionType.ConvertChecked://强制转换或转换运算并check (SomeType)obj
        //        case ExpressionType.DebugInfo://调试信息
        //        case ExpressionType.Decrement://一元递减 a - 1
        //        case ExpressionType.DivideAssign://除法并赋值 a /= b
        //        case ExpressionType.Dynamic://动态操作
        //        case ExpressionType.ExclusiveOrAssign://按位或逻辑 XOR 并赋值 a ^= b
        //        case ExpressionType.Extension://拓展表达式
        //        case ExpressionType.Goto://goto语句
        //        case ExpressionType.Index://索引运算
        //        case ExpressionType.Invoke://调用 委托或lambda表达式 运算
        //        case ExpressionType.Label://标签
        //        case ExpressionType.LeftShiftAssign://左移并赋值 a <<= b
        //        case ExpressionType.Loop://循环 for  while 
        //        case ExpressionType.MemberInit://创建对象并初始化成员 new SomeType(){ X = 1 }
        //        case ExpressionType.ModuloAssign://求余并赋值 a %= b
        //        case ExpressionType.MultiplyAssign://乘法并赋值 a *= b
        //        case ExpressionType.MultiplyAssignChecked://乘法 a * b 并赋值并check
        //        case ExpressionType.MultiplyChecked://乘法 a * b 并check
        //        case ExpressionType.NegateChecked://求反运算 -a 并check
        //        case ExpressionType.New://调用构造初始化对象 new SomeType();
        //        case ExpressionType.NewArrayBounds://创建数组并制定数组维度 new int[3, 5]
        //        case ExpressionType.OrAssign://按位Or运算并赋值 a |= b
        //        case ExpressionType.Default://默认值
        //            goto Label1;
        //            #endregion
        //    }
        //    Label1:
        //    throw new NotImplementedException("未实现写法：" + expression.GetType().ToString() + " " + expression.NodeType.ToString());
        //}

        ///// <summary>
        ///// 二进制运算符表达式ToString
        ///// </summary>
        ///// <param name="expression">二进制运算符表达式</param>
        ///// <param name="operation">操作</param>
        ///// <param name="parameters">注入的参数</param>
        ///// <returns></returns>
        //private static string LogicBinaryExpressionToString(BinaryExpression expression, string operation, List<object> parameters) {
        //    if (parameters != null) {
        //        List<object> temp = new List<object>();
        //        string parameterizedStr = expression.Left.InternalToMSSqlString(null) + "=" + string.Format("@p{0}", parameters.Count);
        //        expression.Right.InternalToMSSqlString(temp);
        //        parameters.AddRange(temp);
        //        temp = null;
        //        return parameterizedStr;
        //    }
        //    return expression.Left.InternalToMSSqlString(null) + operation + expression.Right.InternalToMSSqlString(null);
        //}

        ///// <summary>
        ///// 格式化值使其符合Sql语法
        ///// </summary>
        ///// <param name="value">值</param>
        ///// <returns></returns>
        //private static string FormatExpressionValueToSQL(object value) {
        //    if (value == null) {
        //        return "NULL";
        //    } else if (value is bool) {
        //        return (bool)value ? "1=1" : "1=0";
        //    } else if (value is string) {
        //        return string.Format("'{0}'", value);
        //    } else if (value is DateTime) {
        //        DateTime date = (DateTime)value;
        //        return date.ToString("yyyy-MM-dd HH:mm:ss:fff");
        //    } else if (value is IEnumerable) {
        //        string str = string.Empty;
        //        foreach (var val in (IEnumerable)value) {
        //            str += FormatExpressionValueToSQL(val) + ",";
        //        }
        //        return str.TrimEnd(',');
        //    } else {
        //        return value.ToString();
        //    }
        //}

        ///// <summary>
        ///// 判断type是不是基元类型(包含string类型)
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //private static bool IsPrimitive(Type type) {
        //    if (type == null) {
        //        return false;
        //    }
        //    return type.IsPrimitive || (type == typeof(string));
        //}

        ///// <summary>
        ///// 获取常量中的值或常量中属性的值
        ///// </summary>
        ///// <param name="constant">常量表达式</param>
        ///// <param name="memberName">字段/属性名称</param>
        ///// <returns></returns>
        //private static object GetConstantExpressionValue(ConstantExpression constant, string memberName = null) {
        //    Utilities.CheckNotEmpty(constant, "constant");
        //    if (string.IsNullOrEmpty(memberName)) {
        //        return constant.Value;
        //    } else {
        //        object obj = constant.Value;
        //        object value = obj.GetType().GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField).GetValue(obj);
        //        return value;
        //    }
        //}

        ///// <summary>
        ///// 将contains转换成SQL
        ///// </summary>
        ///// <param name="expression"></param>
        ///// <returns></returns>
        //private static string ConvertContainsToMSSqlString(MethodCallExpression expression) {
        //    Utilities.CheckNotEmpty(expression, "expression");
        //    if (expression.Arguments.Count > 2) {
        //        goto Label1;
        //    }
        //    Expression obj = expression.Object/*实例方法*/ ?? expression.Arguments[0];/*拓展方法*/
        //    if (obj == null) {
        //        goto Label1;
        //    }
        //    if (!typeof(IEnumerable).IsAssignableFrom(obj.Type)) {//如果不是从IEnumerable派生
        //        goto Label1;
        //    }
        //    Expression parm = expression.Arguments.Count == 1 ? expression.Arguments[0] : expression.Arguments[1];
        //    if (parm == null) {
        //        goto Label1;
        //    }
        //    bool isLike = false;//区别list.Contains(p.Name) in还是p.Name.Contains("123") like
        //    MemberExpression left = obj as MemberExpression;
        //    if (left != null && left.Expression is ParameterExpression) {
        //        isLike = true;
        //    }
        //    if (isLike) {
        //        return string.Format("{0} LIKE '%{1}%'", obj.InternalToMSSqlString(null), parm.InternalToMSSqlString(null).Trim((char)39));
        //    }
        //    return string.Format("{0} IN ({1})", parm.InternalToMSSqlString(null), obj.InternalToMSSqlString(null));
        //    Label1:
        //    throw new ArgumentException("不支持的写法: " + expression.ToString());
        //} 
        #endregion
    }
}

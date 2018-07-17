
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;
using System.Collections;

namespace GaoJD.Club.Core
{
	/// <summary>
	/// 简单的参数化的SqlWriter
	/// </summary>
	internal sealed class SimpleParameterizedWriterSql : AiExpressionVisitor {
		/// <summary>
		/// writer
		/// </summary>
		private TextWriter m_writer;
		/// <summary>
		/// 参数列表
		/// </summary>
		private List<object> m_parameters;
		//表达式的左值，类似于p.Name==1的p.Name
		private Expression m_left;
		//二元表达式的类型，类似于p.Name==1的==
		private ExpressionType? m_binaryType;

		/// <summary>
		/// 构造SimpleParameterizedWriterSql
		/// </summary>
		/// <param name="writer">sql写入器</param>
		public SimpleParameterizedWriterSql(TextWriter writer) :
			this(writer, new List<object>()) {
		}
		/// <summary>
		/// 构造SimpleParameterizedWriterSql
		/// </summary>
		/// <param name="writer">sql写入器</param>
		/// <param name="parameters">参数列表</param>
		public SimpleParameterizedWriterSql(TextWriter writer, List<object> parameters) {
			m_writer = writer;
			m_parameters = parameters;
		}

		public static void Write(TextWriter writer, Expression expression, List<object> parameters) {
			expression = AiPartialEvaluator.Eval(expression);
			var simple = new SimpleParameterizedWriterSql(writer, parameters);
			simple.Visit(expression);
		}

		/// <summary>
		/// 访问二进制表达式
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		protected override Expression VisitBinary(BinaryExpression b) {
			this.Write("(");
			this.Visit(b.Left);
			this.Write(" ");
			this.Write(GetOperator(b.NodeType));
			m_binaryType = b.NodeType;
			this.Write(" ");
			this.Visit(b.Right);
			this.Write(")");
			return b;
		}
		/// <summary>
		/// 访问属性
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		protected override Expression VisitMemberAccess(MemberExpression m) {
			//访问属性的时候设置左值
			m_left = m;
			this.Write("[" + m.Member.Name + "]");
			return m;
		}
		/// <summary>
		/// 访问常量
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		protected override Expression VisitConstant(ConstantExpression c) {
			//如果左值是访问成员，类似与p=>p.ID == 1
			if (m_left != null && m_left.NodeType == ExpressionType.MemberAccess) {
				//in操作无法参数化，如果value是数组
				if (c.Type.IsArray) {
					Array array = (Array)c.Value;
					string str = string.Empty;
					foreach (var o in array) {
						switch (Type.GetTypeCode(o.GetType())) {
							case TypeCode.Boolean:
								str += (bool)o ? "1" : "0";
								break;
							case TypeCode.DateTime:
								str += string.Format("'{0}'", ((DateTime)o).ToString("yyyy-MM-dd HH:mm:ss"));
								break;
							case TypeCode.String:
							case TypeCode.Object://guid
								str += string.Format("'{0}'", o);
								break;
							default:
								str += o.ToString();
								break;
						}
						str += ",";
					}
					this.Write(str.TrimEnd(','));
				} else {
					this.Write("@p" + m_parameters.Count);
					m_parameters.Add(c.Value);
					if (c.Value == null) {//如果value为null
						MemberExpression member = (MemberExpression)m_left;
						if (m_binaryType.HasValue) {
							switch (m_binaryType) {
								case ExpressionType.Equal:
									this.Write(string.Format(" OR [{0}] IS NULL", member.Member.Name));
									break;
								case ExpressionType.NotEqual:
									this.Write(string.Format(" OR [{0}] IS NOT NULL", member.Member.Name));
									break;
							}
							m_binaryType = null;
						}
					}
				}
			} else {
				//如果存在p => true或者p => false
				if (c.Value.Equals(true)) {
					this.Write("1=1");
				} else if (c.Value.Equals(false)) {
					this.Write("1=0");
				}
			}
			//访问完右值之后清空左值
			m_left = null;
			return c;
		}
		/// <summary>
		/// 方法调用
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		protected override Expression VisitMethodCall(MethodCallExpression m) {
			if (m.Method.DeclaringType == typeof(string)) {
				switch (m.Method.Name) {
					case "Contains":
						this.Write("(");
						this.Visit(m.Object);
						this.Write(" LIKE '%' + ");
						this.Visit(m.Arguments[0]);
						this.Write(" + '%')");
						return m;
				}
			} else if (m.Method.DeclaringType == typeof(Enumerable)) {//array.Contains(p.Name)只支持数组
				ConstantExpression constant = m.Arguments[0] as ConstantExpression;
				if (constant == null || !constant.Type.IsArray) {
					throw new NotSupportedException("不支持的写法");
				}
				Array array = (Array)constant.Value;
				if (array == null || array.Length == 0) {
					this.Write("1=0");
					return m;
				} else {
					switch (m.Method.Name) {
						case "Contains":
							this.Write("(");
							this.Visit(m.Arguments[1]);
							this.Write(" IN (");
							this.Visit(m.Arguments[0]);
							this.Write("))");
							return m;
					}
				}
			}
			throw new NotSupportedException("不支持的写法");
		}

		/// <summary>
		/// 写入操作
		/// </summary>
		/// <param name="text"></param>
		private void Write(string text) {
			m_writer.Write(text);
		}
		/// <summary>
		/// 获取表达式树的操作
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private string GetOperator(ExpressionType type) {
			switch (type) {
				case ExpressionType.Not:
					return "!";
				case ExpressionType.Add:
				case ExpressionType.AddChecked:
					return "+";
				case ExpressionType.Negate:
				case ExpressionType.NegateChecked:
				case ExpressionType.Subtract:
				case ExpressionType.SubtractChecked:
					return "-";
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyChecked:
					return "*";
				case ExpressionType.Divide:
					return "/";
				case ExpressionType.Modulo:
					return "%";
				case ExpressionType.And:
					return "&";
				case ExpressionType.AndAlso:
					return "AND";
				case ExpressionType.Or:
					return "|";
				case ExpressionType.OrElse:
					return "OR";
				case ExpressionType.LessThan:
					return "<";
				case ExpressionType.LessThanOrEqual:
					return "<=";
				case ExpressionType.GreaterThan:
					return ">";
				case ExpressionType.GreaterThanOrEqual:
					return ">=";
				case ExpressionType.Equal:
					return "=";
				case ExpressionType.NotEqual:
					return "!=";
				case ExpressionType.Coalesce:
					return "??";
				case ExpressionType.RightShift:
					return ">>";
				case ExpressionType.LeftShift:
					return "<<";
				case ExpressionType.ExclusiveOr:
					return "^";
				default:
					return null;
			}
		}
	}
}

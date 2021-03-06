﻿using Bing.Datas.Queries;
using Bing.Utils;

namespace Bing.Datas.Sql.Builders.Conditions
{
    /// <summary>
    /// 范围过滤条件
    /// </summary>
    public class SegmentCondition:ICondition
    {
        /// <summary>
        /// 列名
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// 最小值
        /// </summary>
        private readonly string _min;

        /// <summary>
        /// 最大值
        /// </summary>
        private readonly string _max;
        /// <summary>
        /// 包含边界
        /// </summary>
        private readonly Boundary _boundary;

        /// <summary>
        /// 初始化一个<see cref="SegmentCondition"/>类型的实例
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        public SegmentCondition(string name, string min, string max, Boundary boundary)
        {
            _name = name;
            _min = min;
            _max = max;
            _boundary = boundary;
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        public string GetCondition()
        {
            if (string.IsNullOrWhiteSpace(_name))
            {
                return null;
            }
            var condition = new AndCondition(CreateLeftCondition(), CreateRightCondition());
            return condition.GetCondition();
        }

        /// <summary>
        /// 创建左条件
        /// </summary>
        /// <returns></returns>
        private ICondition CreateLeftCondition()
        {
            if (string.IsNullOrWhiteSpace(_min))
            {
                return NullCondition.Instance;
            }

            return SqlConditionFactory.Create(_name, _min, CreateLeftOperator());
        }

        /// <summary>
        /// 创建左操作符
        /// </summary>
        /// <returns></returns>
        private Operator CreateLeftOperator()
        {
            switch (_boundary)
            {
                case Boundary.Left:
                    return Operator.GreaterEqual;
                case Boundary.Both:
                    return Operator.GreaterEqual;
                default:
                    return Operator.Greater;
            }
        }

        /// <summary>
        /// 创建右条件
        /// </summary>
        /// <returns></returns>
        private ICondition CreateRightCondition()
        {
            if (string.IsNullOrWhiteSpace(_max))
            {
                return NullCondition.Instance;
            }

            return SqlConditionFactory.Create(_name, _max, CreateRightOperator());
        }

        /// <summary>
        /// 创建右操作符
        /// </summary>
        /// <returns></returns>
        private Operator CreateRightOperator()
        {
            switch (_boundary)
            {
                case Boundary.Right:
                    return Operator.LessEqual;
                case Boundary.Both:
                    return Operator.LessEqual;
                default:
                    return Operator.Less;
            }
        }
    }
}

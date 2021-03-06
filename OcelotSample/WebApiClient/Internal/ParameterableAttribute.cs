﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiClient.Contexts;
using WebApiClient.Interfaces;

namespace WebApiClient
{
    /// <summary>
    /// 表示参数内容为IApiParameterable对象或其数组
    /// 不可继承
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    sealed class ParameterableAttribute : Attribute, IApiParameterAttribute
    {
        /// <summary>
        /// http请求之前
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="parameter">特性关联的参数</param>
        /// <returns></returns>
        public async Task BeforeRequestAsync(ApiActionContext context, ApiParameterDescriptor parameter)
        {
            var ables = this.GetApiParameterables(parameter);
            foreach (var item in ables)
            {
                if (item != null)
                {
                    await item.BeforeRequestAsync(context, parameter);
                }
            }
        }

        /// <summary>
        /// 从参数值获取IApiParameterable对象
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private IEnumerable<IApiParameterable> GetApiParameterables(ApiParameterDescriptor parameter)
        {
            if (parameter.Value == null)
            {
                yield break;
            }

            var array = parameter.Value as IEnumerable<IApiParameterable>;
            if (array == null)
            {
                yield return parameter.Value as IApiParameterable;
            }
            else
            {
                foreach (var item in array)
                {
                    yield return item;
                }
            }
        }
    }
}

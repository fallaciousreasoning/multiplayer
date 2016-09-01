using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Reflection
{
    public delegate T ObjectActivator<T>(params object[] args);

    public static class ObjectActivatorHelpers
    {
        public static ObjectActivator<T> GetActivator<T>()
        {
            var type = typeof(T);
            var constructorInfo = type.GetConstructors().First();

            return GetActivator<T>(constructorInfo);
        }

        public static ObjectActivator<T> GetActivator<T>(ConstructorInfo constructorInfo)
        {
            var parameterInfo = constructorInfo.GetParameters();

            var param = Expression.Parameter(typeof(object[]), "args");
            var argsExp = new Expression[parameterInfo.Length];

            for (var i = 0; i < parameterInfo.Length; ++i)
            {
                var index = Expression.Constant(i);
                var paramType = parameterInfo[i].ParameterType;

                var paramAcessorExp = Expression.ArrayIndex(param, index);
                var paramCastExp = Expression.Convert(paramAcessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            var newExp = Expression.New(constructorInfo, argsExp);
            var lambda = Expression.Lambda(typeof(ObjectActivator<T>), newExp, param);
            var compiled = (ObjectActivator<T>) lambda.Compile();

            return compiled;
        }
    }
}

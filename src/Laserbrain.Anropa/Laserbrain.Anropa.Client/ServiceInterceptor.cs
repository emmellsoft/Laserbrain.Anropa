using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Laserbrain.Anropa.Client
{
    internal class ServiceInterceptor : IInterceptor
    {
        private static readonly MethodInfo GenericConvertTaskMethod;
        private readonly IServerCaller _serverCaller;
        private static readonly ConcurrentDictionary<Type, MethodInfo> SpecificConvertTaskMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<MethodInfo, AsyncServiceMethodInfo> AsyncServiceMethodInfos = new ConcurrentDictionary<MethodInfo, AsyncServiceMethodInfo>();

        static ServiceInterceptor()
        {
            GenericConvertTaskMethod = typeof(ServiceInterceptor).GetMethod(nameof(ConvertTask), BindingFlags.Static | BindingFlags.NonPublic);
        }

        public ServiceInterceptor(IServerCaller serverCaller)
        {
            _serverCaller = serverCaller;
        }

        void IInterceptor.Intercept(IInvocation invocation)
        {
            try
            {
                // Get the AsyncServiceMethodInfo of the method to call.
                AsyncServiceMethodInfo asyncServiceMethodInfo = AsyncServiceMethodInfos.GetOrAdd(
                    invocation.Method,
                    AsyncServiceMethodInfoFactory.CreateFromMethodInfo);

                // Get the (optional) parameter for the method call.
                //object requestObject = invocation.Arguments.Length == 0 ? null : invocation.Arguments[0];

                ParameterInfo[] parameters = invocation.Method.GetParameters();
                if (parameters.Length != invocation.Arguments.Length)
                {
                    throw new InvalidOperationException("Hm. The number of method parameters doesn't match the number of arguments...");
                }

                CallParam[] callParams = parameters
                    .Zip(invocation.Arguments, (param, arg) => new CallParam(param.Name, param.ParameterType, arg))
                    .ToArray();

                // Call the service (as a task, i.e. the work is not performed right here but at the next await or GetResult statement).
                Task<object> taskOfObject = _serverCaller.Call(asyncServiceMethodInfo, callParams);

                // Get a convert method from Task<object> to Task<T> where T is the actual return type of this particular method.
                MethodInfo specificConvertMethod = SpecificConvertTaskMethods.GetOrAdd(
                    asyncServiceMethodInfo.ReturnType,
                    GenericConvertTaskMethod.MakeGenericMethod(asyncServiceMethodInfo.ReturnType));

                // Convert the Task<object> to Task<T>.
                object taskOfT = specificConvertMethod.Invoke(null, new object[] { taskOfObject });

                // Assign the resulting Task<T> as the return value.
                invocation.ReturnValue = taskOfT;
            }
            catch (Exception exception)
            {
                // Crikey!
                Debug.WriteLine($"Exception in {nameof(ServiceInterceptor)} calling {invocation.Method.Name} on the service {invocation.Method.DeclaringType}: \r\n{exception}");
                Debugger.Break();
                throw;
            }
        }

        /// <summary>
        /// Support method to convert a Task&lt;object&gt; to a Task&lt;T&gt; called by the Intercept method using reflection.
        /// </summary>
        private static async Task<T> ConvertTask<T>(Task<object> task) => (T)await task.ConfigureAwait(false);
    }
}
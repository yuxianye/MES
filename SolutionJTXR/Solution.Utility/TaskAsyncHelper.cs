using System;
using System.Threading.Tasks;
using System.Windows;

namespace Solution.Utility
{
    /// <summary>
    /// 异步任务辅助类
    /// </summary>
    public static class TaskAsyncHelper
    {
        /// <summary>
        /// 将一个方法function异步运行，在执行完毕时执行回调callback。执行方法无参数无返回值，回调方法无返回值
        /// </summary>
        /// <param name="function">异步方法，该方法没有参数，返回类型必须是void</param>
        /// <param name="callback">异步方法执行完毕时执行的回调方法，该方法没有参数，返回类型必须是void</param>
        public static async void RunAsync(Action function, Action callback)
        {
            Func<Task> taskFunc = () => { return Task.Run(() => { function(); }); };
            await taskFunc();
            if (callback != null)
            {
                callback();
                //Application.Current.Dispatcher.Invoke(callback);
            }
        }

        /// <summary>
        /// 将一个方法function异步运行，在执行完毕时执行回调callback。执行方法无参数有返回值，回调方法有返回值
        /// </summary>
        /// <typeparam name="TResult">异步方法的返回类型</typeparam>
        /// <param name="function">异步方法，该方法没有参数，返回类型必须是TResult</param>
        /// <param name="callback">异步方法执行完毕时执行的回调方法，该方法参数为TResult，返回类型必须是void</param>
        public static async void RunAsync<TResult>(Func<TResult> function, Action<TResult> callback)
        {
            Func<Task<TResult>> taskFunc = () => { return Task.Run(() => { return function(); }); };
            var rlt = await taskFunc();
            if (callback != null)
            {
                callback(rlt);
                //Application.Current.Dispatcher.Invoke(callback, rlt);
            }
        }

        /// <summary>
        /// （方法内有try catch）将一个方法function异步运行，在执行完毕时执行回调callback。执行方法无参数有返回值，回调方法有返回值、有异常
        /// </summary>
        /// <typeparam name="TResult">异步方法的返回类型</typeparam>
        /// <param name="function">异步方法，该方法没有参数，返回类型必须是TResult</param>
        /// <param name="callback">异步方法执行完毕时执行的回调方法，该方法参数为TResult和System.Exception（执行function时的异常），返回类型必须是void</param>
        public static async void RunAsync<TResult>(Func<TResult> function, Action<TResult, Exception> callback)
        {
            var exception = default(Exception);
            var rlt = default(TResult);
            try
            {
                Func<Task<TResult>> taskFunc = () => { return Task.Run(() => { return function(); }); };

                rlt = await taskFunc();
            }
            catch (Exception ex)
            {
                rlt = default(TResult);
                exception = ex;
            }
            finally
            {
                if (callback != null)
                {
                    callback(rlt, exception);
                }
                //Application.Current.Dispatcher.Invoke(callback, rlt, exception);
            }
        }

        /// <summary>
        /// （方法内有try catch）将一个方法function异步运行，在执行完毕时执行回调callback。执行方法有参数有返回值，回调方法有返回值、有异常
        /// </summary>
        /// <typeparam name="TResult">异步方法的返回类型</typeparam>
        /// <param name="function">异步方法，该方法参数类型为object，返回类型必须是TResult</param>
        /// <param name="callback">异步方法执行完毕时执行的回调方法，该方法参数为TResult和System.Exception（执行function时的异常），返回类型必须是void</param>
        /// <param name="inParamForFunction">传入function的参数</param>
        public static async void RunAsync<TResult>(Func<object, TResult> function, Action<TResult, Exception> callback,
            object inParamForFunction)
        {
            var exception = default(Exception);
            var rlt = default(TResult);
            try
            {
                Func<object, Task<TResult>> taskFunc = inObj => { return Task.Run(() => { return function(inObj); }); };

                rlt = await taskFunc(inParamForFunction);
            }
            catch (Exception ex)
            {
                rlt = default(TResult);
                exception = ex;
            }
            finally
            {
                if (callback != null)
                {
                    callback(rlt, exception);
                }
                //Application.Current.Dispatcher.Invoke(callback, rlt, exception);
            }
        }
    }
}

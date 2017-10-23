using System;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.Extensions
{
    public static class ObjectExtensions
    {
        public static void ThrowIfFails<TException>(this object value, Action action, string message = null) where TException: Exception
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (!(Activator.CreateInstance(typeof(TException), message ?? ex.Message, ex) is Exception exception))
                    throw;

                throw exception;
            }
        }

        public static TResult ThrowIfFails<TException, TResult>(this object value, Func<TResult> action, string message = null) where TException : Exception
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                if (!(Activator.CreateInstance(typeof(TException), message ?? ex.Message, ex) is Exception exception))
                    throw;

                throw exception;
            }
        }

        public static async Task ThrowIfFailsAsync<TException>(this object value, Func<Task> action, string message = null) where TException : Exception
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                if (!(Activator.CreateInstance(typeof(TException), message ?? ex.Message, ex) is Exception exception))
                    throw;

                throw exception;
            }
        }

        public static async Task<TResult> ThrowIfFailsAsync<TException, TResult>(this object value, Func<Task<TResult>> action, string message = null) where TException : Exception
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                if (!(Activator.CreateInstance(typeof(TException), message ?? ex.Message, ex) is Exception exception))
                    throw;

                throw exception;
            }
        }
    }
}
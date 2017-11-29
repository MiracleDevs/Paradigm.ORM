using System;
using System.Threading.Tasks;

namespace Paradigm.ORM.Data.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="object"/> class.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Throws if certain method fails.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="action">The action.</param>
        /// <param name="message">The message.</param>
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

        /// <summary>
        /// Throws if certain method fails.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="action">The action.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Throws if certain method fails.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="action">The action.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Throws if certain method fails.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="action">The action.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
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
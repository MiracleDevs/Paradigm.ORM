namespace Paradigm.ORM.DbPublisher.Builders
{
    public class Script
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content { get; }

        /// <summary>
        /// Gets a value indicating whether to ignore errors or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [ignore errors]; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreErrors { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Script"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="content">The content.</param>
        /// <param name="ignoreErrors">if set to <c>true</c> [ignore errors].</param>
        public Script(string name, string content, bool ignoreErrors)
        {
            Name = name;
            Content = content;
            IgnoreErrors = ignoreErrors;
        }
    }
}
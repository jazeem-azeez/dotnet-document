using System.Collections.Generic;
using System.Linq;
using DotnetDocument.Configuration;
using DotnetDocument.Format;
using DotnetDocument.Strategies.Abstractions;
using Humanizer;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace DotnetDocument.Strategies
{
    /// <summary>
    /// The field documentation strategy class
    /// </summary>
    /// <seealso cref="DocumentationStrategyBase{T}" />
    [Strategy(nameof(SyntaxKind.FieldDeclaration))]
    public class FieldDocumentationStrategy : DocumentationStrategyBase<FieldDeclarationSyntax>
    {
        /// <summary>
        /// The formatter
        /// </summary>
        private readonly IFormatter _formatter;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<FieldDocumentationStrategy> _logger;

        /// <summary>
        /// The options
        /// </summary>
        private readonly FieldDocumentationOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldDocumentationStrategy" /> class
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="formatter">The formatter</param>
        /// <param name="options">The options</param>
        public FieldDocumentationStrategy(ILogger<FieldDocumentationStrategy> logger,
            IFormatter formatter, FieldDocumentationOptions options) =>
            (_logger, _formatter, _options) = (logger, formatter, options);

        /// <summary>
        /// Gets the supported kinds
        /// </summary>
        /// <returns>An enumerable of syntax kind</returns>
        public override IEnumerable<SyntaxKind> GetSupportedKinds() => new[]
        {
            SyntaxKind.FieldDeclaration
        };

        /// <summary>
        /// Applies the node
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>The field declaration syntax</returns>
        public override FieldDeclarationSyntax Apply(FieldDeclarationSyntax node)
        {
            // Retrieve field name (fields can have multiple variables, take the first one)
            var fieldName = node.Declaration.Variables.FirstOrDefault()?.Identifier.Text ?? string.Empty;

            // Humanize the field name
            var humanizedFieldName = fieldName.Humanize().ToLower();

            var summary = new List<string>
            {
                // Declare the summary by using the template from configuration
                _formatter.FormatName(_options.Summary.Template,
                    (TemplateKeys.Name, humanizedFieldName))
            };

            return GetDocumentationBuilder()
                .For(node)
                .WithSummary(summary.ToArray())
                .Build();
        }
    }
}


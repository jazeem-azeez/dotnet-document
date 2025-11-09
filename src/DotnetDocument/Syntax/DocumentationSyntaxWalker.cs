using System.Collections.Generic;
using System.Linq;
using DotnetDocument.Configuration;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DotnetDocument.Syntax
{
    /// <summary>
    /// The documentation syntax walker class
    /// </summary>
    /// <seealso cref="CSharpSyntaxWalker" />
    public class DocumentationSyntaxWalker : CSharpSyntaxWalker
    {
        /// <summary>
        /// The kinds
        /// </summary>
        private readonly IEnumerable<SyntaxKind> _kinds;

        /// <summary>
        /// The documentation options
        /// </summary>
        private readonly DocumentationOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentationSyntaxWalker" /> class
        /// </summary>
        /// <param name="kinds">The kinds</param>
        /// <param name="options">The documentation options</param>
        public DocumentationSyntaxWalker(IEnumerable<SyntaxKind> kinds, DocumentationOptions options)
        {
            _kinds = kinds;
            _options = options;
        }

        /// <summary>
        /// Gets the value of the nodes with xml doc
        /// </summary>
        public IList<SyntaxNode> NodesWithXmlDoc { get; } = new List<SyntaxNode>();

        /// <summary>
        /// Gets the value of the nodes without xml doc
        /// </summary>
        public IList<SyntaxNode> NodesWithoutXmlDoc { get; } = new List<SyntaxNode>();

        /// <summary>
        /// Cleans this instance
        /// </summary>
        public void Clean()
        {
            NodesWithXmlDoc.Clear();
            NodesWithoutXmlDoc.Clear();
        }

        /// <summary>
        /// Describes whether this instance is documentable
        /// </summary>
        /// <param name="kind">The kind</param>
        /// <returns>The bool</returns>
        private bool IsDocumentable(SyntaxKind kind) => _kinds.Any(k => k == kind);

        /// <summary>
        /// Visits the core using the specified node
        /// </summary>
        /// <param name="node">The node</param>
        private void VisitCore(SyntaxNode node)
        {
            if (IsDocumentable(node.Kind()))
            {
                // Check if we should skip private members
                if (SyntaxUtils.IsPrivate(node) && !ShouldIncludePrivate(node))
                {
                    base.DefaultVisit(node);
                    return;
                }

                if (SyntaxUtils.IsDocumented(node))
                    NodesWithXmlDoc.Add(node);
                else
                    NodesWithoutXmlDoc.Add(node);
            }

            base.DefaultVisit(node);
        }

        /// <summary>
        /// Determines if private members should be included based on configuration
        /// </summary>
        /// <param name="node">The syntax node</param>
        /// <returns>True if private members should be included for this node type</returns>
        private bool ShouldIncludePrivate(SyntaxNode node)
        {
            // Map syntax kind to the appropriate options
            MemberDocumentationOptionsBase? options = node.Kind() switch
            {
                SyntaxKind.ClassDeclaration => _options.Class,
                SyntaxKind.InterfaceDeclaration => _options.Interface,
                SyntaxKind.ConstructorDeclaration => _options.Constructor,
                SyntaxKind.MethodDeclaration => _options.Method,
                SyntaxKind.PropertyDeclaration => _options.Property,
                SyntaxKind.EnumDeclaration => _options.Enum,
                SyntaxKind.EnumMemberDeclaration => _options.EnumMember,
                SyntaxKind.FieldDeclaration => _options.Field, // Fields use field-specific options
                SyntaxKind.EventDeclaration => _options.DefaultMember, // Events use default member options
                _ => _options.DefaultMember
            };

            return options?.IncludePrivate ?? true;
        }

        /// <summary>
        /// Defaults the visit using the specified node
        /// </summary>
        /// <param name="node">The node</param>
        public override void DefaultVisit(SyntaxNode node) => VisitCore(node);

        /*
        /// <summary>Called when the visitor visits a ClassDeclarationSyntax node.</summary>
        public override void VisitClassDeclaration(ClassDeclarationSyntax node) => VisitCore(node);

        /// <summary>Called when the visitor visits a RecordDeclarationSyntax node.</summary>
        public override void VisitRecordDeclaration(RecordDeclarationSyntax node) => VisitCore(node);

        /// <summary>Called when the visitor visits a ConstructorDeclarationSyntax node.</summary>
        public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node) => VisitCore(node);

        /// <summary>Called when the visitor visits a DestructorDeclarationSyntax node.</summary>
        public override void VisitDestructorDeclaration(DestructorDeclarationSyntax node) => VisitCore(node);

        /// <summary>Called when the visitor visits a EnumDeclarationSyntax node.</summary>
        public override void VisitEnumDeclaration(EnumDeclarationSyntax node) => VisitCore(node);

        /// <summary>Called when the visitor visits a EventDeclarationSyntax node.</summary>
        public override void VisitEventDeclaration(EventDeclarationSyntax node) => VisitCore(node);

        /// <summary>Called when the visitor visits a FieldDeclarationSyntax node.</summary>
        public override void VisitFieldDeclaration(FieldDeclarationSyntax node) => VisitCore(node);

        /// <summary>Called when the visitor visits a DelegateDeclarationSyntax node.</summary>
        public override void VisitDelegateDeclaration(DelegateDeclarationSyntax node) => VisitCore(node);

        /// <summary>Called when the visitor visits a IndexerDeclarationSyntax node.</summary>
        public override void VisitIndexerDeclaration(IndexerDeclarationSyntax node) => VisitCore(node);

        /// <summary>Called when the visitor visits a InterfaceDeclarationSyntax node.</summary>
        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node) => VisitCore(node);

        /// <summary>Called when the visitor visits a MethodDeclarationSyntax node.</summary>
        public override void VisitMethodDeclaration(MethodDeclarationSyntax node) => VisitCore(node);

        /// <summary>Called when the visitor visits a PropertyDeclarationSyntax node.</summary>
        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node) => VisitCore(node);

        /// <summary>Called when the visitor visits a StructDeclarationSyntax node.</summary>
        public override void VisitStructDeclaration(StructDeclarationSyntax node) => VisitCore(node);

        /// <summary>Called when the visitor visits a EnumMemberDeclarationSyntax node.</summary>
        public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node) => VisitCore(node);
        */
    }
}

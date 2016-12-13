using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Rubberduck.Parsing.Annotations;
using Rubberduck.Parsing.VBA;
using Rubberduck.VBEditor;

namespace Rubberduck.Parsing.Symbols
{
    [DataContract]
    public class SerializableDeclarationTree
    {
        private readonly SerializableDeclaration _node;
        private readonly IEnumerable<SerializableDeclarationTree> _children;

        public SerializableDeclarationTree(Declaration declaration)   
            : this(new SerializableDeclaration(declaration)) { }

        public SerializableDeclarationTree(SerializableDeclaration node)
            : this(node, Enumerable.Empty<SerializableDeclarationTree>()) { }

        public SerializableDeclarationTree(SerializableDeclaration node, IEnumerable<SerializableDeclarationTree> children)
        {
            _node = node;
            _children = children;
        }

        [DataMember]
        public SerializableDeclaration Node { get { return _node; } }
        
        [DataMember]
        public IEnumerable<SerializableDeclarationTree> Children { get { return _children; } }
    }

    public class SerializableDeclaration
    {
        public SerializableDeclaration()
        { }

        public SerializableDeclaration(Declaration declaration)
        {
            IdentifierName = declaration.IdentifierName;

            ParentScope = declaration.ParentScope;
            QualifiedMemberName = declaration.QualifiedName;
            Annotations = declaration.Annotations.Cast<AnnotationBase>().ToArray();
            Attributes = declaration.Attributes.ToArray();
            TypeHint = declaration.TypeHint;
            AsTypeName = declaration.AsTypeName;
            IsArray = declaration.IsArray;
            IsBuiltIn = declaration.IsBuiltIn;
            IsSelfAssigned = declaration.IsSelfAssigned;
            IsWithEvents = declaration.IsWithEvents;
            Accessibility = declaration.Accessibility;
            DeclarationType = declaration.DeclarationType;
        }

        public string IdentifierName { get; set; }

        public QualifiedMemberName QualifiedMemberName { get; set; }
        public AnnotationBase[] Annotations { get; set; }
        public KeyValuePair<string, IEnumerable<string>>[] Attributes { get; set; }

        public string ParentScope { get; set; }
        public string AsTypeName { get; set; }
        public string TypeHint { get; set; }
        public bool IsArray { get; set; }
        public bool IsBuiltIn { get; set; }
        public bool IsSelfAssigned { get; set; }
        public bool IsWithEvents { get; set; }
        public Accessibility Accessibility { get; set; }
        public DeclarationType DeclarationType { get; set; }

        public Declaration Unwrap(Declaration parent)
        {
            var attributes = new Attributes();
            foreach (var keyValuePair in Attributes)
            {
                attributes.Add(keyValuePair.Key, keyValuePair.Value);
            }
            return new Declaration(QualifiedMemberName, parent, ParentScope, AsTypeName, TypeHint, IsSelfAssigned, IsWithEvents, Accessibility, DeclarationType, null, Selection.Empty, IsArray, null, IsBuiltIn, Annotations, attributes);
        }
    }
}
namespace MeleeLib.System.Node
{
    public abstract class SiblingNode<TRoot, TParent> : ChildNode<TRoot, TParent>
        where TParent : Node<TRoot>
    {
        public abstract SiblingNode<TRoot, TParent> Next { get; }
        public abstract SiblingNode<TRoot, TParent> Previous { get; }
        public bool IsLast { get { return Next == null; } }
        public bool IsFirst { get { return Previous == null; } }
    }
}

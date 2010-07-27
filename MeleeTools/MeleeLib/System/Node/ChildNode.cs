using MeleeLib.DatHandler;

namespace MeleeLib.System.Node
{
    public abstract class ChildNode<TRoot,TParent> : Node<TRoot>
        where TParent : Node<TRoot>
    {
        public abstract TParent Parent { get; }
        public override TRoot Root
        {
            get { return Parent.Root; }
        }
    }
}

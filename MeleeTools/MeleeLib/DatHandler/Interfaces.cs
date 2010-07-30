using System.Collections.Generic;

namespace MeleeLib.DatHandler {
    public interface IFilePiece {
        int Offset { get; }
        int Size { get; }
        byte[] RawData { get; }
    }
    public interface IHasIndex { int Index { get; } }
    public interface IHeader : IFilePiece { }
    public interface IDefinition : IHeader {
        int StringOffset { get; }
        int DataOffset { get; }
    }
    public interface ISection<out TDefinition, out THeader>
        : IHasDefinition<TDefinition>, IHasHeader<THeader>
        where TDefinition : IDefinition
        where THeader : IHeader { }
    public interface IHasHeader<out THeader> where THeader : IHeader { THeader Header { get; } }
    public interface IHasDefinition<out TDefinition> where TDefinition : IDefinition {
        TDefinition Definition { get; }
        string Name { get; }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using MeleeLib.Utility;

//TODO: Before edit functionality is added, everything needs to be initialized to members rather than read directly from the byte array every time.
//TODO: Use extender functions to manage file integrity and reconstruction
namespace MeleeLib.DatHandler {
    public class File : IEnumerable<IFilePiece>, IHasHeader<FileHeader> {
        public readonly String Filename;
        private File() { }
        public File(string filename) {
            Filename = filename;
            var stream = System.IO.File.OpenRead(filename);
            var rawData = new byte[(int)stream.Length].Slice();
            stream.Read(rawData.Array, 0, rawData.Count);
            stream.Close();

            ArraySlice<byte> data;
            string name;

            #region FileHeader
            data = rawData.Slice(0x00, 0x20);
            Header = new FileHeader(data.Offset, data.Count,
                                    filesize: data.GetInt32(0x04 * 0),
                                    datasize: data.GetInt32(0x04 * 1),
                                    offsetCount: data.GetInt32(0x04 * 2),
                                    ftDataCount: data.GetInt32(0x04 * 3),
                                    sectionType2Count: data.GetInt32(0x04 * 4),
                                    version: data.Slice(0x04 * 5, 0x4).ToArray(),
                                    unknown1: data.GetInt32(0x04 * 6),
                                    unknown2: data.GetInt32(0x04 * 7));
            #endregion
            #region Data
            //The offset of everything else is based off the header
            rawData = rawData.Slice(Header.Size);
            var stringOffsetBase = Header.Datasize
                                   + Header.OffsetCount * SizeOf.Offset
                                   + Header.FtDataCount * SizeOf.FtDefinition
                                   + Header.SectionType2Count * SizeOf.SectionType2Definition;
            #region FtIndex
            FtIndex = new List<FtData>();
            int i;
            for (i = 0; i < Header.FtDataCount; i++) {
                var offset = Header.Datasize + Header.OffsetCount * SizeOf.Offset + i * SizeOf.FtDefinition;
                #region FtDefinition
                data = rawData.Slice(offset, 8);
                var ftDefinition = new FtDefinition(data.Offset, data.Count,
                                                    data.GetInt32(0x04 * 0),
                                                    data.GetInt32(0x04 * 1));
                name = rawData.GetAsciiString(stringOffsetBase + ftDefinition.StringOffset);
                #endregion
                #region FtHeader
                data = rawData.Slice(ftDefinition.DataOffset, 0x60);
                var ftHeader = new FtHeader(data.Offset, data.Count,
                                            attributesStart: data.GetInt32(0x00),
                                            attributesEnd: data.GetInt32(0x04),
                                            unknown1: data.GetInt32(0x08),
                                            subactionStart: data.GetInt32(0x0C),
                                            unknown2: data.GetInt32(0x10),
                                            subactionEnd: data.GetInt32(0x14),
                                            values: data.Slice(0x18, 18).ToArray()
                );
                #endregion
                #region Attributes
                data = rawData.Slice(ftHeader.AttributesStart, ftHeader.AttributesEnd - ftHeader.AttributesStart);
                var attributes = new List<Attribute>();
                for (var j = 0; j < data.Count / SizeOf.Attribute; j++)
                    attributes.Add(new Attribute(j, data.GetSingle(j * SizeOf.Attribute)));
                #endregion
                #region Subactions
                data = rawData.Slice(ftHeader.SubactionStart, ftHeader.SubactionEnd - ftHeader.SubactionStart);
                var subactions = new List<Subaction>();
                for (var j = 0; j < data.Count / SizeOf.SubactionDefinition; j++) {
                    var definitionData = data.Slice(j * SizeOf.SubactionDefinition, SizeOf.SubactionDefinition);
                    var subactionDefinition = new SubactionDefinition(definitionData.Offset, definitionData.Count,
                                                                      stringOffset: definitionData.GetInt32(0x00),
                                                                      unknown1: definitionData.GetInt32(0x04),
                                                                      unknown2: definitionData.GetInt32(0x08),
                                                                      dataOffset: definitionData.GetInt32(0x0C),
                                                                      unknown3: definitionData.GetInt32(0x10),
                                                                      unknown4: definitionData.GetInt32(0x14));
                    name = subactionDefinition.StringOffset != 0
                           ? rawData.GetAsciiString(subactionDefinition.StringOffset) : null;
                    Console.WriteLine(name);
                    var scripts = new List<ScriptCommand>();
                    var scriptData = rawData.Slice(subactionDefinition.DataOffset);
                    i = 0;
                    while (true) {
                        Console.Write(i++);
                        var command = ScriptCommand.Factory(scriptData);
                        scripts.Add(command);
                        scriptData = scriptData.Slice(command.Size);
                        if (command.Type == ScriptCommand.NullType)
                            break;
                        Console.Write("WTF");

                    }
                    subactions.Add(new Subaction(j, name, scripts, subactionDefinition));
                }
                #endregion
                FtIndex.Add(new FtData(ftHeader, ftDefinition, name, attributes, subactions));
            }
            #endregion
            //TODO: SectionType2Index = new SectionType2Index(this, rawData);
            #endregion
        }
        public FileHeader Header { get; internal set; }
        public List<FtData> FtIndex { get; internal set; }
        //TODO public SectionType2Index SectionType2Index { get; internal set; }
        public IEnumerator<IFilePiece> GetEnumerator() {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
    public class FileHeader : IHeader {
        private FileHeader() { }
        internal FileHeader(int offset, int size, int filesize,
                        int datasize, int offsetCount, int ftDataCount,
                        int sectionType2Count, byte[] version,
                        int unknown1, int unknown2) {
            Offset = offset;
            Size = size;
            Filesize = filesize;
            Datasize = datasize;
            OffsetCount = offsetCount;
            FtDataCount = ftDataCount;
            SectionType2Count = sectionType2Count;
            Version = version;
            Unknown1 = unknown1;
            Unknown2 = unknown2;
        }

        public int Filesize { get; internal set; }
        public int Datasize { get; internal set; }
        public int OffsetCount { get; internal set; }
        public int FtDataCount { get; internal set; }
        public int SectionType2Count { get; internal set; }
        public byte[] Version { get; internal set; }
        public int Unknown1 { get; internal set; }
        public int Unknown2 { get; internal set; }
        public int Offset { get; internal set; }
        public int Size { get; internal set; }

        public byte[] RawData {
            get { throw new NotImplementedException(); }
        }
    }
}

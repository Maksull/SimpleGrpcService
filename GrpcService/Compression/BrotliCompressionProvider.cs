using System.IO.Compression;
using Grpc.Net.Compression;

namespace GrpcService.Compression;

public sealed class BrotliCompressionProvider : ICompressionProvider
{
    private readonly CompressionLevel _compressionLevel;
    
    public BrotliCompressionProvider(CompressionLevel compressionLevel)
    {
        _compressionLevel = compressionLevel;
    }
    
    public string EncodingName => "br";
    
    public Stream CreateCompressionStream(Stream outputStream,
        CompressionLevel? compressionLevel)
    {
        return new BrotliStream(outputStream, compressionLevel ?? _compressionLevel, leaveOpen: true);
    }
    
    public Stream CreateDecompressionStream(Stream stream)
    {
        return new BrotliStream(stream, CompressionMode.Decompress, leaveOpen: true);
    }
}
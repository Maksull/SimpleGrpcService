using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Domain.Entities;

[ProtoContract]
public sealed class Product
{
    private string _productId = string.Empty;
    
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    [ProtoMember(1)]
    public required string ProductId {
        get => _productId;
        set
        {
            if (!Ulid.TryParse(value, out _))
            {
                throw new ArgumentException("Invalid ULID format");
            }

            _productId = value;
        }
    }
    [ProtoMember(2)]
    public string Name { get; set; } = string.Empty;
    [ProtoMember(3)]
    public string Description { get; set; } = string.Empty;
    [ProtoMember(4)]
    public required string CategoryId { get; set; }
}
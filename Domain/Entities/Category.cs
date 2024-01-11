using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Domain.Entities;

[ProtoContract]
public sealed class Category
{
    private string _categoryId = string.Empty;

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    [ProtoMember(1)]
    public required string CategoryId
    {
        get => _categoryId;
        set
        {
            if (!Ulid.TryParse(value, out _))
            {
                throw new ArgumentException("Invalid ULID format");
            }

            _categoryId = value;
        }
    }

    [ProtoMember(2)] 
    public required string Name { get; set; }
}
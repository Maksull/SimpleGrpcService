using ProtoBuf;

namespace Domain.Entities.Product;

[ProtoContract]
public sealed class DeletedProduct : Product
{
    private string _deletedProductId = string.Empty;
    
    [ProtoMember(5)] 
    public required string DeletedProductId 
    {
        get => _deletedProductId;
        set
        {
            if (!Ulid.TryParse(value, out _))
            {
                throw new ArgumentException("Invalid ULID format");
            }

            _deletedProductId = value;
        }
    }
    [ProtoMember(6)] 
    public required DateTime DeletedAt { get; set; }
    [ProtoMember(7)] 
    public required DateTime DeleteAt { get; set; }
}
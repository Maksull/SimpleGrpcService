using ProtoBuf;

namespace Domain.Entities.Category;

[ProtoContract]
public sealed class DeletedCategory : Category
{
    private string _deletedCategoryId = string.Empty;
    
    [ProtoMember(3)] 
    public required string DeletedCategoryId 
    {
        get => _deletedCategoryId;
        set
        {
            if (!Ulid.TryParse(value, out _))
            {
                throw new ArgumentException("Invalid ULID format");
            }

            _deletedCategoryId = value;
        }
    }
    [ProtoMember(4)] 
    public required DateTime DeletedAt { get; set; }
    [ProtoMember(5)] 
    public required DateTime DeleteAt { get; set; }
}
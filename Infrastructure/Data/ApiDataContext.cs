using Application.Serialization;
using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data;

public sealed class ApiDataContext
{
    private readonly IMongoCollection<BsonDocument> _categoriesDocuments;
    private readonly IMongoCollection<BsonDocument> _productsDocuments;
    
    public ApiDataContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        _categoriesDocuments = database.GetCollection<BsonDocument>("Categories");
        _productsDocuments = database.GetCollection<BsonDocument>("Products");

        // Seed initial data
        SeedData();
    }
    
    private void SeedData()
    {
        if (_categoriesDocuments.CountDocuments(new BsonDocument()) == 0 && _productsDocuments.CountDocuments(new BsonDocument()) == 0)
        {
            Category[] categories =
            [
                new() { CategoryId = Ulid.NewUlid().ToString(), Name = "Watersports" },
                new() { CategoryId = Ulid.NewUlid().ToString(), Name = "Football" },
                new() { CategoryId = Ulid.NewUlid().ToString(), Name = "Chess" }
            ];

            Product[] products =
            [
                new() { ProductId  = Ulid.NewUlid().ToString(), Name = "Kayak", Description = "A boat for one person", CategoryId = categories[0].CategoryId },
                new() { ProductId  = Ulid.NewUlid().ToString(), Name = "Lifejacket", Description = "Protective and fashionable", CategoryId = categories[1].CategoryId }
            ];
            
            var categoriesProtobuf = new List<BsonDocument>();
            foreach (var category in categories)
            {
                var serializedData = ProtoBufSerializer.ClassToByteArray(category);
                
                var document = new BsonDocument
                {
                    { "_id", category.CategoryId },
                    { "protobufData", serializedData }
                };
                
                categoriesProtobuf.Add(document);
            }
            
            var productsProtoBuf = new List<BsonDocument>();
            foreach (var product in products)
            {
                var serializedData = ProtoBufSerializer.ClassToByteArray(product);
                
                var document = new BsonDocument
                {
                    { "_id", product.ProductId },
                    { "protobufData", serializedData }
                };
                
                productsProtoBuf.Add(document);
            }
            
            _categoriesDocuments.InsertMany(categoriesProtobuf);
            _productsDocuments.InsertMany(productsProtoBuf);
        }
    }

    public IMongoCollection<BsonDocument> CategoriesDocuments => _categoriesDocuments;
    public IMongoCollection<BsonDocument> ProductsDocuments => _productsDocuments;
}
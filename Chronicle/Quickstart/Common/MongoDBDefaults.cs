using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace Quickstart.Common;

public static class MongoDBDefaults
{
    public static void Initialize()
    {
        #region Snippet:Quickstart-MongoDBDefaults
        BsonSerializer
            .RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        var pack = new ConventionPack
        {
            // We want to ignore extra elements that might be in the documents, Chronicle adds some metadata to the documents
            new IgnoreExtraElementsConvention(true),

            // Chronicle uses camelCase for element names, so we need to use this convention
            new CamelCaseElementNameConvention()
        };
        ConventionRegistry.Register("conventions", pack, t => true);
        #endregion Snippet:Quickstart-MongoDBDefaults
    }
}

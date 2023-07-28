using System.Text.Json;
using FluentAssertions;
using Letterbook.ActivityPub.Models;
using Object = Letterbook.ActivityPub.Models.Object;

namespace Letterbook.ActivityPub.Tests;

public abstract class ConvertContextTests
{
    public class ReadShould : ConvertContextTests
    {
        private string InputJson { set => _inputJson = value; }
        private string _inputJson = "";
    
        private List<LdContext> OutputContexts => _outputObject.Value;
        private readonly Lazy<List<LdContext>> _outputObject;

        public ReadShould()
        {
            _outputObject = new Lazy<List<LdContext>>(() =>
            {
                var opts = new JsonSerializerOptions(JsonOptions.ActivityPub);
                var contexts = JsonSerializer.Deserialize<Object>(_inputJson, opts)
                    ?.LdContext.ToList();
                return contexts ?? throw new JsonException("Failed to deserialize Object - returned null");
            });
        }
        
        [Fact]
        public void ReadStringAsSingleSuffix()
        {
            InputJson = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Object"}""";
            OutputContexts.Should().HaveCount(1);
            OutputContexts[0].Prefix.Should().BeNull();
            OutputContexts[0].Suffix.Should().Be("https://www.w3.org/ns/activitystreams");
        }
        
        [Fact]
        public void ReadObjectAsPairs()
        {
            InputJson = """
                {
                    "@context": {
                        "@import": "https://www.w3.org/ns/activitystreams",
                        "term1": "definition1",
                        "term2": "definition2"
                    },
                    "type": "Object"
                }
                """;
            
            OutputContexts.Should().HaveCount(3);
            OutputContexts.Should().Contain(context => context.Prefix == "@import" && context.Suffix == "https://www.w3.org/ns/activitystreams");
            OutputContexts.Should().Contain(context => context.Prefix == "term1" && context.Suffix == "definition1");
            OutputContexts.Should().Contain(context => context.Prefix == "term2" && context.Suffix == "definition2");
        }

        [Fact]
        public void ReadArrayOfStringsAsMultipleSuffixes()
        {
            InputJson = """
                {
                    "@context": [
                        "https://www.w3.org/ns/activitystreams",
                        "https://example.com"
                    ],
                    "type": "Object"
                }
                """;
            
            OutputContexts.Should().HaveCount(2);
            OutputContexts.Should().Contain(context => context.Prefix == null && context.Suffix == "https://www.w3.org/ns/activitystreams");
            OutputContexts.Should().Contain(context => context.Prefix == null && context.Suffix == "https://example.com");
        }

        [Fact]
        public void FlattenArrayOfObjects()
        {
            InputJson = """
                {
                    "@context": [
                        {
                            "@import": "https://www.w3.org/ns/activitystreams"
                        },
                        {
                            "term1": "definition1",
                            "term2": "definition2"
                        }
                    ],
                    "type": "Object"
                }
                """;
            
            OutputContexts.Should().HaveCount(3);
            OutputContexts.Should().Contain(context => context.Prefix == "@import" && context.Suffix == "https://www.w3.org/ns/activitystreams");
            OutputContexts.Should().Contain(context => context.Prefix == "term1" && context.Suffix == "definition1");
            OutputContexts.Should().Contain(context => context.Prefix == "term2" && context.Suffix == "definition2");
        }

        [Fact]
        public void MergeArrayOfMixedTypes()
        {
            InputJson = """
                {
                    "@context": [
                        "https://www.w3.org/ns/activitystreams",
                        {
                            "term1": "definition1",
                            "term2": "definition2"
                        }
                    ],
                    "type": "Object"
                }
                """;
            
            OutputContexts.Should().HaveCount(3);
            OutputContexts.Should().Contain(context => context.Prefix == null && context.Suffix == "https://www.w3.org/ns/activitystreams");
            OutputContexts.Should().Contain(context => context.Prefix == "term1" && context.Suffix == "definition1");
            OutputContexts.Should().Contain(context => context.Prefix == "term2" && context.Suffix == "definition2");
        }
    }

    public class WriteShould : ConvertContextTests
    {
        private List<LdContext> InputContexts { get; set; } = new();

        private JsonElement OutputJson => _outputJson.Value;
        private Lazy<JsonElement> _outputJson;

        public WriteShould()
        {
            _outputJson = new Lazy<JsonElement>(() =>
            {
                var obj = new Object
                {
                    Type = "Object",
                    LdContext = InputContexts
                };
                
                var opts = new JsonSerializerOptions(JsonOptions.ActivityPub);
                var json = JsonSerializer.Serialize(obj, opts);
                var elem = JsonSerializer.SerializeToElement(obj, opts);
                if (!elem.TryGetProperty("@context", out var context))
                    throw new JsonException("Bad json - missing @context");
                return context;
            });
        }
        
        [Fact]
        public void WriteSingleStringAsString()
        {
            InputContexts.Add(new LdContext("https://www.w3.org/ns/activitystreams"));
            
            OutputJson.ValueKind.Should().Be(JsonValueKind.String);
            OutputJson.GetString().Should().Be("https://www.w3.org/ns/activitystreams");
        }

        [Fact]
        public void WriteSinglePairAsObject()
        {
            InputContexts.Add(new LdContext("term", "definition"));
            
            OutputJson.ValueKind.Should().Be(JsonValueKind.Object);
            OutputJson.GetProperty("term").GetString().Should().Be("definition");
        }

        [Fact]
        public void WriteMultiplePairsAsObject()
        {
            InputContexts.Add(new LdContext("term1", "definition1"));
            InputContexts.Add(new LdContext("term2", "definition2"));
            
            OutputJson.ValueKind.Should().Be(JsonValueKind.Object);
            OutputJson.GetProperty("term1").GetString().Should().Be("definition1");
            OutputJson.GetProperty("term2").GetString().Should().Be("definition2");
        }

        [Fact]
        public void WriteMultipleStringsAsArray()
        {
            InputContexts.Add(new LdContext("https://www.w3.org/ns/activitystreams"));
            InputContexts.Add(new LdContext("https://example.com"));
            
            OutputJson.ValueKind.Should().Be(JsonValueKind.Array);
            var entries = OutputJson.EnumerateArray().ToList();
            entries.Should().HaveCount(2);
            entries.Should().Contain(e => e.ValueKind == JsonValueKind.String && e.GetString() == "https://www.w3.org/ns/activitystreams");
            entries.Should().Contain(e => e.ValueKind == JsonValueKind.String && e.GetString() == "https://example.com");
        }

        [Fact]
        public void WriteMixedAsArray()
        {
            InputContexts.Add(new LdContext("https://www.w3.org/ns/activitystreams"));
            InputContexts.Add(new LdContext("term", "definition"));
            
            OutputJson.ValueKind.Should().Be(JsonValueKind.Array);
            var entries = OutputJson.EnumerateArray().ToList();
            entries.Should().HaveCount(2);
            entries.Should().Contain(e => e.ValueKind == JsonValueKind.String && e.GetString() == "https://www.w3.org/ns/activitystreams");
            entries.Should().Contain(e => e.ValueKind == JsonValueKind.Object && e.GetProperty("term").GetString() == "definition");
        }
    }
}
using System.Text.Json;
using Letterbook.ActivityPub.Models;
using Object = Letterbook.ActivityPub.Models.Object;

// Disable this check - its bugged.
// https://resharper-support.jetbrains.com/hc/en-us/community/posts/206655415
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local

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
            
            Assert.Collection(OutputContexts, ctx =>
            {
                Assert.Null(ctx.Prefix);
                Assert.Equal("https://www.w3.org/ns/activitystreams", ctx.Suffix);
            });
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
            
            Assert.Collection(OutputContexts, 
                ctx =>
                {
                    Assert.Equal("@import", ctx.Prefix);
                    Assert.Equal("https://www.w3.org/ns/activitystreams", ctx.Suffix);
                },
                ctx =>
                {
                    Assert.Equal("term1", ctx.Prefix);
                    Assert.Equal("definition1", ctx.Suffix);
                },
                ctx =>
                {
                    Assert.Equal("term2", ctx.Prefix);
                    Assert.Equal("definition2", ctx.Suffix);
                }
            );
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
            
            Assert.Collection(OutputContexts, 
                ctx =>
                {
                    Assert.Null(ctx.Prefix);
                    Assert.Equal("https://www.w3.org/ns/activitystreams", ctx.Suffix);
                },
                ctx =>
                {
                    Assert.Null(ctx.Prefix);
                    Assert.Equal("https://example.com", ctx.Suffix);
                }
            );
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
            
            Assert.Collection(OutputContexts, 
                ctx =>
                {
                    Assert.Equal("@import", ctx.Prefix);
                    Assert.Equal("https://www.w3.org/ns/activitystreams", ctx.Suffix);
                },
                ctx =>
                {
                    Assert.Equal("term1", ctx.Prefix);
                    Assert.Equal("definition1", ctx.Suffix);
                },
                ctx =>
                {
                    Assert.Equal("term2", ctx.Prefix);
                    Assert.Equal("definition2", ctx.Suffix);
                }
            );
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
            
            Assert.Collection(OutputContexts, 
                ctx =>
                {
                    Assert.Null(ctx.Prefix);
                    Assert.Equal("https://www.w3.org/ns/activitystreams", ctx.Suffix);
                },
                ctx =>
                {
                    Assert.Equal("term1", ctx.Prefix);
                    Assert.Equal("definition1", ctx.Suffix);
                },
                ctx =>
                {
                    Assert.Equal("term2", ctx.Prefix);
                    Assert.Equal("definition2", ctx.Suffix);
                }
            );
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
            
            Assert.Equal(JsonValueKind.String, OutputJson.ValueKind);
            Assert.Equal("https://www.w3.org/ns/activitystreams", OutputJson.GetString());
        }

        [Fact]
        public void WriteSinglePairAsObject()
        {
            InputContexts.Add(new LdContext("term", "definition"));
            
            Assert.Equal(JsonValueKind.Object, OutputJson.ValueKind);
            Assert.Equal("definition", OutputJson.GetProperty("term").GetString());
        }

        [Fact]
        public void WriteMultiplePairsAsObject()
        {
            InputContexts.Add(new LdContext("term1", "definition1"));
            InputContexts.Add(new LdContext("term2", "definition2"));
            
            Assert.Equal(JsonValueKind.Object, OutputJson.ValueKind);
            Assert.Equal("definition1", OutputJson.GetProperty("term1").GetString());
            Assert.Equal("definition2", OutputJson.GetProperty("term2").GetString());
        }

        [Fact]
        public void WriteMultipleStringsAsArray()
        {
            InputContexts.Add(new LdContext("https://www.w3.org/ns/activitystreams"));
            InputContexts.Add(new LdContext("https://example.com"));
            
            Assert.Equal(JsonValueKind.Array, OutputJson.ValueKind);
            Assert.Collection(OutputJson.EnumerateArray().ToList(),
                e =>
                {
                    Assert.Equal(JsonValueKind.String, e.ValueKind);
                    Assert.Equal("https://www.w3.org/ns/activitystreams", e.GetString());
                },
                e =>
                {
                    Assert.Equal(JsonValueKind.String, e.ValueKind);
                    Assert.Equal("https://example.com", e.GetString());
                }
            );
        }

        [Fact]
        public void WriteMixedAsArray()
        {
            InputContexts.Add(new LdContext("https://www.w3.org/ns/activitystreams"));
            InputContexts.Add(new LdContext("term", "definition"));
            
            Assert.Equal(JsonValueKind.Array, OutputJson.ValueKind);
            Assert.Collection(OutputJson.EnumerateArray().ToList(),
                e =>
                {
                    Assert.Equal(JsonValueKind.String, e.ValueKind);
                    Assert.Equal("https://www.w3.org/ns/activitystreams", e.GetString());
                },
                e =>
                {
                    Assert.Equal(JsonValueKind.Object, e.ValueKind);
                    Assert.Equal("definition", e.GetProperty("term").GetString());
                }
            );
        }
    }
}
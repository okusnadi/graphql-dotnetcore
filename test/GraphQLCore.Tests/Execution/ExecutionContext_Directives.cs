﻿namespace GraphQLCore.Tests.Execution
{
    using GraphQLCore.Type;
    using Microsoft.CSharp.RuntimeBinder;
    using NUnit.Framework;
    using Schemas;

    [TestFixture]
    public class ExecutionContext_Directives
    {
        private GraphQLSchema schema;

        [Test]
        public void Execute_FragmentWithFalsyInclude_DoesntPrintFragment()
        {
            dynamic result = this.schema.Execute(@"
            query fetch {
                nested {
                    ...frag @include(if : false)
                }
            }

            fragment frag on NestedQueryType {
                a
                b
            }
            ");

            Assert.Throws<RuntimeBinderException>(new TestDelegate(() => { string a = result.data.nested.a; }));
            Assert.Throws<RuntimeBinderException>(new TestDelegate(() => { string b = result.data.nested.b; }));
        }

        [Test]
        public void Execute_FragmentWithFalsySkip_PrintsFragment()
        {
            dynamic result = this.schema.Execute(@"
            query fetch {
                nested {
                    ...frag @skip(if : false)
                }
            }

            fragment frag on NestedQueryType {
                a
                b
            }
            ");

            Assert.AreEqual("1", result.data.nested.a);
            Assert.AreEqual("2", result.data.nested.b);
        }

        [Test]
        public void Execute_FragmentWithTruthyInclude_PrintsFragment()
        {
            dynamic result = this.schema.Execute(@"
            query fetch {
                nested {
                    ...frag @include(if : true)
                }
            }

            fragment frag on NestedQueryType {
                a
                b
            }
            ");

            Assert.AreEqual("1", result.data.nested.a);
            Assert.AreEqual("2", result.data.nested.b);
        }

        [Test]
        public void Execute_FragmentWithTruthySkip_DoesntPrintFragment()
        {
            dynamic result = this.schema.Execute(@"
            query fetch {
                nested {
                    ...frag @skip(if : true)
                }
            }

            fragment frag on NestedQueryType {
                a
                b
            }
            ");

            Assert.Throws<RuntimeBinderException>(new TestDelegate(() => { string a = result.data.nested.a; }));
            Assert.Throws<RuntimeBinderException>(new TestDelegate(() => { string b = result.data.nested.b; }));
        }

        [Test]
        public void Execute_ScalarBasedFalsyIncludeAndFalsySkipDirective_PrintsTheField()
        {
            dynamic result = this.schema.Execute("{ a, b @include(if: false) @skip(if: false) }");

            Assert.AreEqual("world", result.data.a);
            Assert.Throws<RuntimeBinderException>(new TestDelegate(() => { string b = result.data.b; }));
        }

        [Test]
        public void Execute_ScalarBasedFalsyIncludeAndTruthySkipDirective_DoesntPrintTheField()
        {
            dynamic result = this.schema.Execute("{ a, b @include(if: false) @skip(if: true) }");

            Assert.AreEqual("world", result.data.a);
            Assert.Throws<RuntimeBinderException>(new TestDelegate(() => { string b = result.data.b; }));
        }

        [Test]
        public void Execute_ScalarBasedFalsyIncludeDirective_DoesntPrintTheField()
        {
            dynamic result = this.schema.Execute("{ a, b @include(if: false) }");

            Assert.AreEqual("world", result.data.a);
            Assert.Throws<RuntimeBinderException>(new TestDelegate(() => { string b = result.data.b; }));
        }

        [Test]
        public void Execute_ScalarBasedFalsySkipDirective_PrintsTheField()
        {
            dynamic result = this.schema.Execute("{ a, b @skip(if: false) }");

            Assert.AreEqual("world", result.data.a);
            Assert.AreEqual("test", result.data.b);
        }

        [Test]
        public void Execute_ScalarBasedTruthyIncludeAndFalsySkipDirective_PrintsTheField()
        {
            dynamic result = this.schema.Execute("{ a, b @include(if: true) @skip(if: false) }");

            Assert.AreEqual("world", result.data.a);
            Assert.AreEqual("test", result.data.b);
        }

        [Test]
        public void Execute_ScalarBasedTruthyIncludeAndTruthySkipDirective_DoesntPrintTheField()
        {
            dynamic result = this.schema.Execute("{ a, b @include(if: true) @skip(if: true) }");

            Assert.AreEqual("world", result.data.a);
            Assert.Throws<RuntimeBinderException>(new TestDelegate(() => { string b = result.data.b; }));
        }

        [Test]
        public void Execute_ScalarBasedTruthyIncludeDirective_PrintsTheField()
        {
            dynamic result = this.schema.Execute("{ a, b @include(if: true) }");

            Assert.AreEqual("world", result.data.a);
            Assert.AreEqual("test", result.data.b);
        }

        [Test]
        public void Execute_ScalarBasedTruthySkipDirective_DoesntPrintTheField()
        {
            dynamic result = this.schema.Execute("{ a, b @skip(if: true) }");

            Assert.AreEqual("world", result.data.a);
            Assert.Throws<RuntimeBinderException>(new TestDelegate(() => { string b = result.data.b; }));
        }

        [Test]
        public void Execute_CustomDirective_ResolvesCorrectly()
        {
            var schema = new TestSchema();
            dynamic result = schema.Execute("{ foo @onField }");

            Assert.AreEqual("replacedByDirective", result.data.foo);
        }

        [SetUp]
        public void SetUp()
        {
            this.schema = new GraphQLSchema();
            var nestedType = new NestedQueryType(this.schema);
            var rootType = new RootQueryType(nestedType, this.schema);

            this.schema.AddKnownType(nestedType);
            this.schema.AddKnownType(rootType);
            this.schema.Query(rootType);
        }

        private class NestedQueryType : GraphQLObjectType
        {
            public NestedQueryType(GraphQLSchema schema) : base("NestedQueryType", "")
            {
                this.Field("a", () => "1");
                this.Field("b", () => "2");
            }
        }

        private class RootQueryType : GraphQLObjectType
        {
            public RootQueryType(NestedQueryType nested, GraphQLSchema schema) : base("RootQueryType", "")
            {
                this.Field("a", () => "world");
                this.Field("b", () => "test");
                this.Field("nested", () => nested);
            }
        }
    }
}
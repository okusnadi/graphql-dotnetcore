﻿namespace GraphQLCore.Tests.Execution
{
    using GraphQLCore.Type;
    using NUnit.Framework;
    using Schemas;
    using System.Dynamic;

    [TestFixture]
    public class ExecutionContect_Variables
    {
        public IGraphQLSchema schema;
        private dynamic variables;

        [Test]
        public void Execute_WithIntVariable_AcceptsAsIntArgument()
        {
            var query = @"query getIntArg($intArgVar: Int) {
                            complicatedArgs {
                                intArgField(intArg: $intArgVar)
                            }
                        }";

            var result = this.schema.Execute(query, variables);

            Assert.AreEqual(3, result.complicatedArgs.intArgField);
        }

        [Test]
        public void Execute_WithNonNullIntVariable_AcceptsAsIntArgument()
        {
            var query = @"query getNonNullIntArg($intArgVar: Int) {
                            complicatedArgs {
                                nonNullIntArgField(nonNullIntArg: $intArgVar)
                            }
                        }";

            var result = this.schema.Execute(query, variables);

            Assert.AreEqual(3, result.complicatedArgs.nonNullIntArgField);
        }

        [Test]
        public void Execute_WithStringVariable_AcceptsAsStringArgument()
        {
            var query = @"query getStringArg($stringArgVar: String) {
                            complicatedArgs {
                                stringArgField(stringArg: $stringArgVar)
                            }
                        }";

            var result = this.schema.Execute(query, variables);

            Assert.AreEqual("sample", result.complicatedArgs.stringArgField);
        }

        [Test]
        public void Execute_WithBooleanVariable_AcceptsAsBooleanArgument()
        {
            var query = @"query getBooleanArg($booleanArgVar: Boolean) {
                            complicatedArgs {
                                booleanArgField(booleanArg: $booleanArgVar)
                            }
                        }";

            var result = this.schema.Execute(query, variables);

            Assert.AreEqual(true, result.complicatedArgs.booleanArgField);
        }

        [Test]
        public void Execute_WithEnumVariable_AcceptsAsEnumArgument()
        {
            var query = @"query getEnumArg($enumArgVar: FurColor) {
                            complicatedArgs {
                                enumArgField(enumArg: $enumArgVar)
                            }
                        }";

            var result = this.schema.Execute(query, variables);

            Assert.AreEqual("BROWN", result.complicatedArgs.enumArgField);
        }

        [Test]
        public void Execute_WithEnumVariableAsString_AcceptsAsEnumArgument()
        {
            var query = @"query getEnumArg($enumArgVarAsString: FurColor) {
                            complicatedArgs {
                                enumArgField(enumArg: $enumArgVarAsString)
                            }
                        }";

            var result = this.schema.Execute(query, variables);

            Assert.AreEqual("BROWN", result.complicatedArgs.enumArgField);
        }

        [Test]
        public void Execute_WithFloatVariable_AcceptsAsFloatArgument()
        {
            var query = @"query getFloatArg($floatArgVar: Float) {
                            complicatedArgs {
                                floatArgField(floatArg: $floatArgVar)
                            }
                        }";

            var result = this.schema.Execute(query, variables);

            Assert.AreEqual(1.6f, result.complicatedArgs.floatArgField);
        }

        [Test]
        public void Execute_WithStringListVariable_AcceptsAsStringListArgument()
        {
            var query = @"query getStringListArg($stringListArgVar: [String]) {
                            complicatedArgs {
                                stringListArgField(stringListArg: $stringListArgVar)
                            }
                        }";

            var result = this.schema.Execute(query, variables);

            Assert.AreEqual(new string[] { "a", "b", "c" }, result.complicatedArgs.stringListArgField);
        }

        [Test]
        public void Execute_WithNonNullIntListVariable_AcceptsAsIntListArgument()
        {
            var query = @"query getStringListArg($nonNullIntListArgVar: [Int!]) {
                            complicatedArgs {
                                nonNullIntListArgField(nonNullIntListArg: $nonNullIntListArgVar)
                            }
                        }";

            var result = this.schema.Execute(query, variables);

            Assert.AreEqual(new int[] { 1, 2, 3 }, result.complicatedArgs.nonNullIntListArgField);
        }

        [Test]
        public void Execute_WithIntListVariable_AcceptsAsIntListArgument()
        {
            var query = @"query getStringListArg($intListArgVar: [Int]) {
                            complicatedArgs {
                                intListArgField(intListArg: $intListArgVar)
                            }
                        }";

            var result = this.schema.Execute(query, variables);

            Assert.AreEqual(new int?[] { 1, null, 3 }, result.complicatedArgs.intListArgField);
        }

        [SetUp]
        public void SetUp()
        {
            this.variables = new ExpandoObject();
            this.variables.intArgVar = 3;
            this.variables.stringArgVar = "sample";
            this.variables.booleanArgVar = true;
            this.variables.enumArgVar = FurColor.BROWN;
            this.variables.enumArgVarAsString = "BROWN";
            this.variables.floatArgVar = 1.6f;
            this.variables.stringListArgVar = new string[] { "a", "b", "c" };
            this.variables.nonNullIntListArgVar = new int[] { 1, 2, 3 };
            this.variables.intListArgVar = new int?[] { 1, null, 3 };

            this.schema = new TestSchema();
        }
    }
}

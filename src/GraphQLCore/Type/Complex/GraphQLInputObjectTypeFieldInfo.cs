﻿namespace GraphQLCore.Type.Complex
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Translation;
    using Utils;

    public class GraphQLInputObjectTypeFieldInfo : GraphQLFieldInfo
    {
        public LambdaExpression Lambda { get; set; }
        public override Type SystemType { get; set; }

        public static GraphQLInputObjectTypeFieldInfo CreateAccessorFieldInfo(string fieldName, LambdaExpression accessor)
        {
            return new GraphQLInputObjectTypeFieldInfo()
            {
                Name = fieldName,
                Arguments = new Dictionary<string, GraphQLObjectTypeArgumentInfo>(),
                Lambda = accessor,
                SystemType = ReflectionUtilities.GetReturnValueFromLambdaExpression(accessor)
            };
        }

        protected override GraphQLBaseType GetSchemaType(Type type, ISchemaRepository schemaRepository)
        {
            return schemaRepository.GetSchemaInputTypeFor(type);
        }
    }
}
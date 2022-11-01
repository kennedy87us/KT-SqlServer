namespace Kent.SqlServer.Tests.Infrastructure
{
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Metadata;
    using System;
    using System.Collections.Generic;

    public class EntityType : IEntityType
    {
        public EntityType(Type clrType)
        {
            ClrType = clrType;
        }

        public IEntityType BaseType => throw new NotImplementedException();

        public string DefiningNavigationName => throw new NotImplementedException();

        public IEntityType DefiningEntityType => throw new NotImplementedException();

        public IModel Model => throw new NotImplementedException();

        public string Name => ClrType.FullName;

        public Type ClrType { get; set; }

        public object this[string name] => throw new NotImplementedException();

        public IKey FindPrimaryKey()
        {
            throw new NotImplementedException();
        }

        public IKey FindKey(IReadOnlyList<IProperty> properties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IKey> GetKeys()
        {
            throw new NotImplementedException();
        }

        public IForeignKey FindForeignKey(IReadOnlyList<IProperty> properties, IKey principalKey, IEntityType principalEntityType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IForeignKey> GetForeignKeys()
        {
            throw new NotImplementedException();
        }

        public IIndex FindIndex(IReadOnlyList<IProperty> properties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IIndex> GetIndexes()
        {
            throw new NotImplementedException();
        }

        public IProperty FindProperty(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IProperty> GetProperties()
        {
            throw new NotImplementedException();
        }

        public IServiceProperty FindServiceProperty(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IServiceProperty> GetServiceProperties()
        {
            throw new NotImplementedException();
        }

        public IAnnotation FindAnnotation(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAnnotation> GetAnnotations()
        {
            throw new NotImplementedException();
        }
    }
}
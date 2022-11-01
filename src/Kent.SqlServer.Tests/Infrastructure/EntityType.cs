namespace Kent.SqlServer.Tests.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Metadata;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

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

        InstantiationBinding IEntityType.ConstructorBinding => throw new NotImplementedException();

        IReadOnlyEntityType IReadOnlyEntityType.BaseType => throw new NotImplementedException();

        IReadOnlyModel IReadOnlyTypeBase.Model => throw new NotImplementedException();

        bool IReadOnlyTypeBase.HasSharedClrType => throw new NotImplementedException();

        bool IReadOnlyTypeBase.IsPropertyBag => throw new NotImplementedException();

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

        IEnumerable<IEntityType> IEntityType.GetDirectlyDerivedTypes()
        {
            throw new NotImplementedException();
        }

        IKey IEntityType.FindKey(IReadOnlyList<IReadOnlyProperty> properties)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IKey> IEntityType.GetDeclaredKeys()
        {
            throw new NotImplementedException();
        }

        IForeignKey IEntityType.FindForeignKey(IReadOnlyList<IReadOnlyProperty> properties, IReadOnlyKey principalKey, IReadOnlyEntityType principalEntityType)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IForeignKey> IEntityType.FindForeignKeys(IReadOnlyList<IReadOnlyProperty> properties)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IForeignKey> IEntityType.FindDeclaredForeignKeys(IReadOnlyList<IReadOnlyProperty> properties)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IForeignKey> IEntityType.GetDeclaredForeignKeys()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IForeignKey> IEntityType.GetDerivedForeignKeys()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IForeignKey> IEntityType.GetReferencingForeignKeys()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IForeignKey> IEntityType.GetDeclaredReferencingForeignKeys()
        {
            throw new NotImplementedException();
        }

        INavigation IEntityType.FindDeclaredNavigation(string name)
        {
            throw new NotImplementedException();
        }

        IEnumerable<INavigation> IEntityType.GetDeclaredNavigations()
        {
            throw new NotImplementedException();
        }

        IEnumerable<INavigation> IEntityType.GetNavigations()
        {
            throw new NotImplementedException();
        }

        ISkipNavigation IEntityType.FindSkipNavigation(string name)
        {
            throw new NotImplementedException();
        }

        IEnumerable<ISkipNavigation> IEntityType.GetSkipNavigations()
        {
            throw new NotImplementedException();
        }

        IIndex IEntityType.FindIndex(IReadOnlyList<IReadOnlyProperty> properties)
        {
            throw new NotImplementedException();
        }

        IIndex IEntityType.FindIndex(string name)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IIndex> IEntityType.GetDeclaredIndexes()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IIndex> IEntityType.GetDerivedIndexes()
        {
            throw new NotImplementedException();
        }

        IProperty IEntityType.FindDeclaredProperty(string name)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IProperty> IEntityType.GetDeclaredProperties()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IProperty> IEntityType.GetForeignKeyProperties()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IProperty> IEntityType.GetValueGeneratingProperties()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IServiceProperty> IEntityType.GetDeclaredServiceProperties()
        {
            throw new NotImplementedException();
        }

        ChangeTrackingStrategy IReadOnlyEntityType.GetChangeTrackingStrategy()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IDictionary<string, object>> IReadOnlyEntityType.GetSeedData(bool providerValues)
        {
            throw new NotImplementedException();
        }

        LambdaExpression IReadOnlyEntityType.GetQueryFilter()
        {
            throw new NotImplementedException();
        }

        string IReadOnlyEntityType.GetDiscriminatorPropertyName()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyEntityType> IReadOnlyEntityType.GetDerivedTypes()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyEntityType> IReadOnlyEntityType.GetDirectlyDerivedTypes()
        {
            throw new NotImplementedException();
        }

        IReadOnlyKey IReadOnlyEntityType.FindPrimaryKey()
        {
            throw new NotImplementedException();
        }

        IReadOnlyKey IReadOnlyEntityType.FindKey(IReadOnlyList<IReadOnlyProperty> properties)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyKey> IReadOnlyEntityType.GetKeys()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyKey> IReadOnlyEntityType.GetDeclaredKeys()
        {
            throw new NotImplementedException();
        }

        IReadOnlyForeignKey IReadOnlyEntityType.FindForeignKey(IReadOnlyList<IReadOnlyProperty> properties, IReadOnlyKey principalKey, IReadOnlyEntityType principalEntityType)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.FindForeignKeys(IReadOnlyList<IReadOnlyProperty> properties)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.FindDeclaredForeignKeys(IReadOnlyList<IReadOnlyProperty> properties)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetDeclaredForeignKeys()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetDerivedForeignKeys()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetForeignKeys()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetReferencingForeignKeys()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetDeclaredReferencingForeignKeys()
        {
            throw new NotImplementedException();
        }

        IReadOnlyNavigation IReadOnlyEntityType.FindDeclaredNavigation(string name)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyNavigation> IReadOnlyEntityType.GetDeclaredNavigations()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyNavigation> IReadOnlyEntityType.GetDerivedNavigations()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyNavigation> IReadOnlyEntityType.GetNavigations()
        {
            throw new NotImplementedException();
        }

        IReadOnlySkipNavigation IReadOnlyEntityType.FindSkipNavigation(string name)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlySkipNavigation> IReadOnlyEntityType.GetDeclaredSkipNavigations()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlySkipNavigation> IReadOnlyEntityType.GetDerivedSkipNavigations()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlySkipNavigation> IReadOnlyEntityType.GetSkipNavigations()
        {
            throw new NotImplementedException();
        }

        IReadOnlyIndex IReadOnlyEntityType.FindIndex(IReadOnlyList<IReadOnlyProperty> properties)
        {
            throw new NotImplementedException();
        }

        IReadOnlyIndex IReadOnlyEntityType.FindIndex(string name)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyIndex> IReadOnlyEntityType.GetDeclaredIndexes()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyIndex> IReadOnlyEntityType.GetDerivedIndexes()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyIndex> IReadOnlyEntityType.GetIndexes()
        {
            throw new NotImplementedException();
        }

        IReadOnlyProperty IReadOnlyEntityType.FindProperty(string name)
        {
            throw new NotImplementedException();
        }

        IReadOnlyList<IReadOnlyProperty> IReadOnlyEntityType.FindProperties(IReadOnlyList<string> propertyNames)
        {
            throw new NotImplementedException();
        }

        IReadOnlyProperty IReadOnlyEntityType.FindDeclaredProperty(string name)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyProperty> IReadOnlyEntityType.GetDeclaredProperties()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyProperty> IReadOnlyEntityType.GetDerivedProperties()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyProperty> IReadOnlyEntityType.GetProperties()
        {
            throw new NotImplementedException();
        }

        IReadOnlyServiceProperty IReadOnlyEntityType.FindServiceProperty(string name)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyServiceProperty> IReadOnlyEntityType.GetDeclaredServiceProperties()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyServiceProperty> IReadOnlyEntityType.GetDerivedServiceProperties()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IReadOnlyServiceProperty> IReadOnlyEntityType.GetServiceProperties()
        {
            throw new NotImplementedException();
        }

        PropertyAccessMode IReadOnlyTypeBase.GetPropertyAccessMode()
        {
            throw new NotImplementedException();
        }

        PropertyAccessMode IReadOnlyTypeBase.GetNavigationAccessMode()
        {
            throw new NotImplementedException();
        }

        PropertyInfo IReadOnlyTypeBase.FindIndexerPropertyInfo()
        {
            throw new NotImplementedException();
        }

        IAnnotation IAnnotatable.FindRuntimeAnnotation(string name)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IAnnotation> IAnnotatable.GetRuntimeAnnotations()
        {
            throw new NotImplementedException();
        }

        IAnnotation IAnnotatable.AddRuntimeAnnotation(string name, object value)
        {
            throw new NotImplementedException();
        }

        IAnnotation IAnnotatable.SetRuntimeAnnotation(string name, object value)
        {
            throw new NotImplementedException();
        }

        IAnnotation IAnnotatable.RemoveRuntimeAnnotation(string name)
        {
            throw new NotImplementedException();
        }

        TValue IAnnotatable.GetOrAddRuntimeAnnotationValue<TValue, TArg>(string name, Func<TArg, TValue> valueFactory, TArg factoryArgument)
        {
            throw new NotImplementedException();
        }
    }
}
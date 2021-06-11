using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MPS.Common.Attributes;

namespace MPS.IOC.Configurations.Mapper
{
    public static class AutoMapperConfig
    {
        public static void AddMapperConfigurations(this IServiceCollection services)
        {
            services.AddAutoMapper(AddAllConfigs);
        }
        
        private static void AddAllConfigs(IMapperConfigurationExpression expression)
        {

            var selectMethod = typeof(Enumerable).GetMethods().Where(m => m.Name == "Select")
                .FirstOrDefault(f => f.GetParameters().Any(a => a.Name != null && a.Name.Equals("selector")));

            var applyConfigMethod = typeof(AutoMapperConfig).GetMethod("ApplyConfig");
            //var x = typeof(AutoMapperConfig).GetMethods();

            var viewModels = typeof(DtoForAttribute).Assembly.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(DtoForAttribute), true).Any());

            foreach (var model in viewModels)
            {
                var entityType = ((DtoForAttribute)model.GetCustomAttribute(typeof(DtoForAttribute), true))?.EntityClass;
                applyConfigMethod?.MakeGenericMethod(entityType ?? throw new InvalidOperationException(), model)
                    .Invoke(null, new object[] {expression, selectMethod});
            }
        }

        public static void ApplyConfig<TSource, TDestination>(IMapperConfigurationExpression expression, MethodInfo selectMethod)
        {
            var createdMap = expression.CreateMap<TSource, TDestination>();

            Type viewModel = typeof(TDestination);
            Type entityType = typeof(TSource);

            var props = viewModel.GetProperties();

            //store mapping for reverse map
            var simpleMappings = new Dictionary<string, string>();
            foreach (var prop in props)
            {

                var propType = prop.PropertyType;
                if (propType != typeof(string) && prop.PropertyType.IsClass)
                {
                    if (entityType.GetProperty(prop.Name) != null
                         || prop.GetCustomAttributes(typeof(MapFromAttribute), false).Any())
                    {
                        Debug.WriteLine(prop.Name);
                        createdMap.ForMember(prop.Name, op => op.ExplicitExpansion());
                    }

                }

                var desc = (MapFromAttribute[])prop.GetCustomAttributes(typeof(MapFromAttribute), false);
                if (desc.Length == 0) continue;
                if (desc[0].PropertyName.Contains("."))
                {

                    var properySplit = desc[0].PropertyName.Split('.');
                    if (properySplit.Length > 2)
                        throw new Exception("بیشتر از یک نقطه در مپ فرام امکان پذیر نیست");
                    var relationProp = entityType.GetProperty(properySplit[0]);

                    if (relationProp == null) continue;
                    if (relationProp.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(relationProp.PropertyType))
                    {
                        ParameterExpression entityParameter = Expression.Parameter(entityType, "p");
                        Expression relationExpression = entityParameter;
                        relationExpression = Expression.Property(relationExpression, relationProp); // x => x.relation

                        var listItemType = relationProp.PropertyType.GetGenericArguments()[0];
                        ParameterExpression selectParameter = Expression.Parameter(listItemType);
                        Expression selectExpression = selectParameter;
                        var selectedProperty = listItemType.GetProperty(properySplit[1]);
                        selectExpression = Expression.Property(selectExpression, selectedProperty ?? throw new InvalidOperationException()); // rel => rel.Property
                        var selectFunc = typeof(Func<,>).MakeGenericType(listItemType, selectedProperty.PropertyType);

                        var mapFromExpression = Expression.Call(
                            null,
                            selectMethod.MakeGenericMethod(new Type[] { listItemType, selectedProperty.PropertyType }),
                            new Expression[] { relationExpression, Expression.Lambda(selectFunc, selectExpression, selectParameter) }); // x => x.relation.Select(rel => rel.property)

                        var destinationListType = typeof(IEnumerable<>).MakeGenericType(selectedProperty.PropertyType);

                        var mapFromFunc = typeof(Func<,>).MakeGenericType(entityType, destinationListType);

                        var lambda = Expression.Lambda(mapFromFunc, mapFromExpression, entityParameter);

                        createdMap.ForMember(prop.Name, m => m.DynamicMapFrom(destinationListType, lambda));

                    }
                    else if (relationProp.PropertyType != typeof(string) && relationProp.PropertyType.IsClass)
                    {
                        //expression.CreateMap<User, UserTestVM>().ForMember("Name", m => m.MapFrom(map => map.xPerson.xNationalID));
                        ParameterExpression entityParameter = Expression.Parameter(entityType, "p");
                        Expression relationExpression = entityParameter;
                        relationExpression = Expression.Property(relationExpression, relationProp); // x => x.relation
                        var selectedProperty = relationProp.PropertyType.GetProperty(properySplit[1]);
                        var mapFromExpression = Expression.Property(relationExpression, selectedProperty ?? throw new InvalidOperationException());
                        var mapFromFunc = typeof(Func<,>).MakeGenericType(entityType, selectedProperty.PropertyType);
                        var lambda = Expression.Lambda(mapFromFunc, mapFromExpression, entityParameter);
                        createdMap.ForMember(prop.Name, m => m.DynamicMapFrom(selectedProperty.PropertyType, lambda));
                    }
                }
                else
                {
                    simpleMappings.Add(desc[0].PropertyName, prop.Name);
                    createdMap.ForMember(prop.Name, m => m.MapFrom(desc[0].PropertyName));
                }
            }
            var xReverseMap = createdMap.ReverseMap();

            foreach (var item in simpleMappings)
                xReverseMap.ForMember(item.Key, m => m.MapFrom(item.Value));
        }
        
        public static void DynamicMapFrom<TSource, TDestination, TMember>(this IMemberConfigurationExpression<TSource, TDestination, TMember> xExpression, Type xSelectedPropertyType, LambdaExpression xMapExpression)
        {
            MethodInfo method = xExpression.GetType().GetMethods().Where(m => m.Name == "MapFrom").FirstOrDefault(f => f.GetParameters().Any(a => a.Name != null && a.Name.Equals("mapExpression")));
            MethodInfo generic = method?.MakeGenericMethod(xSelectedPropertyType);
            generic?.Invoke(xExpression, new object[] { xMapExpression });
        }
    }
}
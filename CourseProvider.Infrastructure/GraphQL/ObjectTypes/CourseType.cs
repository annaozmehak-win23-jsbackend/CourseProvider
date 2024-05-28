using CourseProvider.Infrastructure.Data.Entities;
using CourseProvider.Infrastructure.Models;

namespace CourseProvider.Infrastructure.GraphQL.ObjectTypes;

public class CourseType : ObjectType<CourseEntity>
{
    protected override void Configure(IObjectTypeDescriptor<CourseEntity> descriptor)
    {
        descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
        descriptor.Field(c => c.IsBestSeller).Type<BooleanType>();
        descriptor.Field(c => c.IsDigital).Type<BooleanType>();
        descriptor.Field(c => c.Categories).Type<ListType<StringType>>();
        descriptor.Field(c => c.Title).Type<StringType>();
        descriptor.Field(c => c.Ingress).Type<StringType>();
        descriptor.Field(c => c.StarRating).Type<DecimalType>();
        descriptor.Field(c => c.Reviews).Type<StringType>();
        descriptor.Field(c => c.LikesInProcent).Type<StringType>();
        descriptor.Field(c => c.Likes).Type<StringType>();
        descriptor.Field(c => c.Hours).Type<StringType>();
        descriptor.Field(c => c.Authors).Type<ListType<AuthorType>>();
        descriptor.Field(c => c.Prices).Type<ListType<PricesType>>();
        descriptor.Field(c => c.Content).Type<ListType<ContentType>>();
    }
}

public class AuthorType : ObjectType<AuthorEntity>
{
    protected override void Configure(IObjectTypeDescriptor<AuthorEntity> descriptor)
    {
        descriptor.Field(a => a.Name).Type<StringType>();
    }
}

public class PricesType : ObjectType<PricesEntity>
{
    protected override void Configure(IObjectTypeDescriptor<PricesEntity> descriptor)
    {
        descriptor.Field(a => a.Currency).Type<StringType>();
        descriptor.Field(a => a.Price).Type<DecimalType>();
        descriptor.Field(a => a.Discount).Type<DecimalType>();
    }
}

public class ContentType : ObjectType<ContentEntity>
{
    protected override void Configure(IObjectTypeDescriptor<ContentEntity> descriptor)
    {
        descriptor.Field(a => a.Description).Type<StringType>();
        descriptor.Field(a => a.Includes).Type<ListType<StringType>>();
        descriptor.Field(a => a.ProgramDetails).Type<ListType<ProgramDetailItemType>>();
    }
}

public class ProgramDetailItemType : ObjectType<ProgramDetailItemEntity>
{
    protected override void Configure(IObjectTypeDescriptor<ProgramDetailItemEntity> descriptor)
    {
        descriptor.Field(a => a.Id).Type<IntType>();
        descriptor.Field(a => a.Title).Type<StringType>();
        descriptor.Field(a => a.Description).Type<StringType>();
    }
}
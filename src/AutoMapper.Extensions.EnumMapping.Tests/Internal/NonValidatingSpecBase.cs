using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AutoMapper.Extensions.EnumMapping.Tests.Internal
{
    public abstract class NonValidatingSpecBase : SpecBase
    {
        private IMapper mapper;

        protected abstract MapperConfiguration Configuration { get; }
        protected IConfigurationProvider ConfigProvider => Configuration;

        protected IMapper Mapper => mapper ?? (mapper = Configuration.CreateMapper());

        protected IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, object parameters = null, params Expression<Func<TDestination, object>>[] membersToExpand) =>
            Mapper.ProjectTo(source, parameters, membersToExpand);

        protected IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, IDictionary<string, object> parameters, params string[] membersToExpand) =>
            Mapper.ProjectTo<TDestination>(source, parameters, membersToExpand);
    }
}

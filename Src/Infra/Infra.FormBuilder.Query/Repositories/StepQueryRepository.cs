using Si24.Infra.FormBuilder.Query.Configs.Contexts;
using Si24.Infra.FormBuilder.Query.Framework;

namespace Si24.Infra.FormBuilder.Query.Repositories;

public class StepQueryRepository(FormBuilderQueryContext dbContext) :BaseQueryRepository<Step,int>(dbContext),IStepQueryRepository
{
    
}
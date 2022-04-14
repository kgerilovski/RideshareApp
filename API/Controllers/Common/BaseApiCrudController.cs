using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideshareApp.Services.Infrastructure.Services;

namespace API.Controllers.Common
{
    [Authorize]
    [ApiController]
    public abstract class BaseApiCrudController<TInDTO, TOutDTO, TEntity, TPk> : BaseAsyncProtectiveController<TInDTO, TOutDTO, TEntity, TPk>
        where TPk : struct
    {
        public BaseApiCrudController(IBaseAsyncService<TInDTO, TOutDTO, TEntity, TPk> aBaseService) : base(aBaseService)
        {
        }
    }
}
